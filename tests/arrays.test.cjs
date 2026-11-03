const assert = require('node:assert/strict');
const test = require('node:test');
const base = process.env.SHOWCASE_BASE_URL || 'http://localhost:5196';
const path = number => `${base}/concepts/arrays/example-${number}`;
const decode = html => html.replace(/&#x([\da-f]+);/gi, (_, code) => String.fromCodePoint(parseInt(code, 16)))
    .replaceAll('&gt;', '>').replaceAll('&lt;', '<').replaceAll('&amp;', '&');

// Real HTTP forms exercise MVC binding, antiforgery, C# calculations and Razor output together.
async function submit(number, fields, withToken = true) {
    const page = await fetch(path(number));
    assert.equal(page.status, 200);
    const html = await page.text();
    const body = new URLSearchParams(fields);
    if (withToken) body.set('__RequestVerificationToken', html.match(/name="__RequestVerificationToken"[^>]*value="([^"]+)"/)[1]);
    const cookies = page.headers.getSetCookie().map(cookie => cookie.split(';')[0]).join('; ');
    const response = await fetch(path(number), { method: 'POST', body, headers: { Cookie: cookies } });
    const raw = await response.text();
    return { response, raw, html: decode(raw) };
}

const values = html => [...html.matchAll(/data-array-value>(.*?)<\/span>/gs)].map(match => match[1]);
const outcome = html => html.match(/<strong>(.*?)<\/strong>/s)?.[1];
const stateToken = html => html.match(/name="StateToken"[^>]*value="([^"]+)"/)?.[1];

test('all five array slots support complete and partial canonical routes with antiforgery', async () => {
    for (let number = 1; number <= 5; number++) {
        for (const headers of [{}, { 'X-Showcase-Partial': 'true' }]) {
            const response = await fetch(path(number), { headers });
            const html = await response.text();
            assert.equal(response.status, 200);
            assert.ok(html.includes('data-example-form'));
            assert.ok(!html.includes('Not implemented yet'));
            if (headers['X-Showcase-Partial']) assert.ok(!html.includes('<!DOCTYPE html>'));
        }
        assert.equal((await submit(number, {}, false)).response.status, 400);
    }
});

test('duplicates normalize numbers and compare trimmed names without letter case', async () => {
    const numeric = await submit(1, { ValueKind: 'Numbers', Values: '1,1.0,-2,0,-0' });
    assert.equal(outcome(numeric.html), '2 repeated values');
    assert.deepEqual(values(numeric.html), ['1', '1', '-2', '0', '0']);
    assert.ok(numeric.html.includes('<td>0, 1</td>'));
    const names = await submit(1, { ValueKind: 'Names', Values: ' Ana , Ben,ana,BEN,Carlo' });
    assert.equal(outcome(names.html), '2 repeated values');
    assert.ok(names.html.includes('<td>0, 2</td>'));
    const unique = await submit(1, { ValueKind: 'Numbers', Values: '1,2,3' });
    assert.equal(outcome(unique.html), 'No duplicates');
});

async function play(moves) {
    let html = await (await fetch(path(3))).text();
    let token = stateToken(html);
    for (const Cell of moves) {
        ({ html } = await submit(3, { Cell: String(Cell), StateToken: token, Player: 'X', Winner: 'X' }));
        token = stateToken(html);
    }
    return { html, token };
}

test('tic-tac-toe alternates turns and detects rows, columns and diagonals', async () => {
    for (const [moves, winner] of [
        [[0, 3, 1, 4, 2], 'X'], [[0, 1, 2, 4, 6, 7], 'O'],
        [[0, 1, 4, 2, 8], 'X'], [[2, 0, 4, 1, 6], 'X']
    ]) {
        const result = await play(moves);
        assert.equal(outcome(result.html), `${winner} wins!`);
        assert.equal((result.html.match(/winning square/g) || []).length, 3);
        const extra = await submit(3, { Cell: '8', StateToken: result.token });
        assert.equal(outcome(extra.html), `${winner} wins!`);
        assert.ok(extra.html.includes('This game has finished'));
    }
    const first = await play([0]);
    assert.equal(outcome(first.html), 'O to move');
    const occupied = await submit(3, { Cell: '0', StateToken: first.token });
    assert.ok(occupied.html.includes('That square is occupied'));
    const next = await submit(3, { Cell: '1', StateToken: stateToken(occupied.html) });
    assert.equal(outcome(next.html), 'X to move');
});

test('tic-tac-toe detects a draw and rejects invalid or cross-example state', async () => {
    const draw = await play([0, 1, 2, 4, 3, 5, 7, 6, 8]);
    assert.equal(outcome(draw.html), "It's a draw");
    const loopPage = await (await fetch(`${base}/concepts/loops/example-4`)).text();
    for (const fields of [{ Cell: '0' }, { Cell: '0', StateToken: 'altered' }, { Cell: '0', StateToken: stateToken(loopPage) }]) {
        const { response, html } = await submit(3, fields);
        assert.equal(response.status, 200);
        assert.match(html, /data-example-result[^>]*hidden/);
        assert.ok(html.includes('This board is invalid'));
    }
    const fresh = await (await fetch(path(3))).text();
    const invalid = await submit(3, { Cell: '9', StateToken: stateToken(fresh) });
    assert.match(invalid.html, /data-example-result[^>]*hidden/);
});

test('tic-tac-toe recognizes every winning row, column and diagonal', async () => {
    for (const line of [[0, 1, 2], [3, 4, 5], [6, 7, 8], [0, 3, 6], [1, 4, 7], [2, 5, 8], [0, 4, 8], [2, 4, 6]]) {
        const other = Array.from({ length: 9 }, (_, index) => index).filter(cell => !line.includes(cell));
        const { html } = await play([line[0], other[0], line[1], other[1], line[2]]);
        assert.equal(outcome(html), 'X wins!');
    }
});

test('multiplication table computes every cell for minimum, rectangular and maximum dimensions', async () => {
    for (const [Rows, Columns] of [[1, 1], [3, 5], [12, 12]]) {
        const { html } = await submit(4, { Rows: String(Rows), Columns: String(Columns), Result: 'Forged' });
        assert.equal(outcome(html), `${Rows} × ${Columns} table`);
        const table = html.match(/<table class="array-table">[\s\S]*?<\/table>/)[0];
        const products = [...table.matchAll(/<td>(\d+)<\/td>/g)].map(match => Number(match[1]));
        assert.deepEqual(products, Array.from({ length: Rows * Columns }, (_, index) => (Math.floor(index / Columns) + 1) * (index % Columns + 1)));
    }
});

test('house finder searches all four rows with trimmed, case-insensitive complete names', async () => {
    const rosters = { Gryffindor: ' Harry Potter, Hermione Granger, Harry Potter ', Slytherin: 'Draco Malfoy', Ravenclaw: 'Luna Lovegood', Hufflepuff: 'Cedric Diggory' };
    for (const [Member, expected] of [[' harry potter ', 'Gryffindor'], ['DRACO MALFOY', 'Slytherin'], ['Luna Lovegood', 'Ravenclaw'], ['Cedric Diggory', 'Hufflepuff'], ['Harry', 'Member not found'], ['Unknown', 'Member not found']]) {
        const { html } = await submit(5, { ...rosters, Member, Result: 'Forged' });
        assert.equal(outcome(html), expected);
    }
    const shared = await submit(5, { Gryffindor: 'Ana, Ana', Ravenclaw: ' Ana ', Member: 'ana' });
    assert.equal(outcome(shared.html), 'Gryffindor, Ravenclaw');
    const unequal = await submit(5, { Hufflepuff: 'Ben', Member: 'ben' });
    assert.equal(outcome(unequal.html), 'Hufflepuff');
    const encoded = await submit(5, { Gryffindor: '<script>alert(1)</script>', Member: '<script>alert(1)</script>' });
    assert.equal(outcome(encoded.html), 'Gryffindor');
    assert.ok(!encoded.raw.includes('<script>alert(1)</script>'));
    const maximum = await submit(5, { Slytherin: Array.from({ length: 20 }, (_, index) => `Member ${index}`).join(','), Member: 'Member 19' });
    assert.equal(outcome(maximum.html), 'Slytherin');
    assert.ok(maximum.html.includes('[1, 19]'));
});

test('new 2D examples reject missing, malformed and oversized inputs without results', async () => {
    for (const [number, fields] of [
        [4, {}], [4, { Rows: '0', Columns: '2' }], [4, { Rows: '13', Columns: '2' }],
        [4, { Rows: '2', Columns: '-1' }], [4, { Rows: '2', Columns: '13' }],
        [4, { Rows: '2.5', Columns: '3' }], [4, { Rows: 'oops', Columns: '2' }],
        [5, {}], [5, { Member: 'Ana' }], [5, { Gryffindor: 'Ana', Member: ' ' }],
        [5, { Gryffindor: 'Ana\nBen', Member: 'Ana' }],
        [5, { Gryffindor: Array(21).fill('Ana').join(','), Member: 'Ana' }],
        [5, { Gryffindor: 'A'.repeat(61), Member: 'Ana' }],
        [5, { Gryffindor: 'A'.repeat(1221), Member: 'Ana' }],
        [5, { Gryffindor: 'Ana', Member: 'A'.repeat(61) }],
        [5, { Gryffindor: 'Ana', Member: 'Ana,Ben' }]
    ]) {
        const { response, html } = await submit(number, { ...fields, Result: 'Forged' });
        assert.equal(response.status, 200);
        assert.match(html, /data-example-result[^>]*hidden/);
        assert.ok(html.includes('data-array-errors'));
        assert.ok(html.includes('aria-invalid="true"'));
    }
});

test('sort uses numeric order, supports both directions and retains duplicates', async () => {
    const ascending = await submit(2, { ValueKind: 'Numbers', Values: '10,2,-1,2.5,2', Direction: 'Ascending' });
    assert.deepEqual(values(ascending.html), ['-1', '2', '2', '2.5', '10']);
    const descending = await submit(2, { ValueKind: 'Numbers', Values: '10,2,-1,2.5,2', Direction: 'Descending' });
    assert.deepEqual(values(descending.html), ['10', '2.5', '2', '2', '-1']);
    const names = await submit(2, { ValueKind: 'Names', Values: 'Zara\nAna\nbea', Direction: 'Ascending' });
    assert.deepEqual(values(names.html), ['Ana', 'bea', 'Zara']);
});

test('one-dimensional forms reject invalid data and encode names safely', async () => {
    for (const [number, fields] of [
        [1, {}], [1, { ValueKind: 'Numbers', Values: ',,,' }],
        [1, { ValueKind: 'Numbers', Values: 'hello' }],
        [1, { ValueKind: 'Numbers', Values: '-79228162514264337593543950335' }],
        [1, { ValueKind: 'Numbers', Values: '1000000001' }],
        [1, { ValueKind: 'Names', Values: Array(31).fill('Ana').join(',') }],
        [1, { ValueKind: 'Names', Values: 'A'.repeat(41) }],
        [2, { ValueKind: '99', Values: '1,2', Direction: 'Ascending' }],
        [2, { ValueKind: 'Numbers', Values: '1,2', Direction: '99' }]
    ]) {
        const { response, html } = await submit(number, { ...fields, Result: 'Forged' });
        assert.equal(response.status, 200);
        assert.match(html, /data-example-result[^>]*hidden/);
        assert.ok(html.includes('data-array-errors'));
    }
    const encoded = await submit(1, { ValueKind: 'Names', Values: '<script>alert(1)</script>,Ana' });
    assert.ok(!encoded.raw.includes('<script>alert(1)</script>'));
    for (let number = 1; number <= 2; number++) assert.equal((await submit(number, {}, false)).response.status, 400);
});
