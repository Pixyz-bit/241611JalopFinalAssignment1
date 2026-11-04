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

test('duplicates normalize numeric equivalents and retain zero-based positions', async () => {
    const numeric = await submit(1, { ValueKind: 'Numbers', Values: '1,1.0,-2,0,-0' });
    assert.equal(outcome(numeric.html), '2 repeated values');
    assert.deepEqual(values(numeric.html), ['1', '1', '-2', '0', '0']);
    assert.ok(numeric.html.includes('<td>0, 1</td>'));
    const unique = await submit(1, { ValueKind: 'Numbers', Values: '1,2,3' });
    assert.equal(outcome(unique.html), 'No duplicates');
});

// Read both results and their positions from server-rendered HTML.
const largest = html => html.match(/data-array-largest>(.*?)<\/strong>/s)?.[1];
const smallest = html => html.match(/data-array-smallest>(.*?)<\/strong>/s)?.[1];

const rowFields = rows => ({ Row1: rows[0], Row2: rows[1], Row3: rows[2] });

test('2D extremes scan exactly three rows with one to twelve columns', async () => {
    for (const [rows, maximum, minimum, highIndex, lowIndex] of [
        [['8,3,12','-4,7,2','6,0,9'], '12', '-4', '[0, 2]', '[1, 0]'],
        [['-8,-3,-12','-4,-7,-2','-9,-5,-6'], '-2', '-12', '[1, 2]', '[0, 2]'],
        [['1','2','3'], '3', '1', '[2, 0]', '[0, 0]'],
        [['-1000000000','1000000000','0'], '1000000000', '-1000000000', '[1, 0]', '[0, 0]']
    ]) {
        const { response, html } = await submit(3, { ...rowFields(rows), Largest: '999', Smallest: '-999', LargestRow: '9', Result: 'Forged' });
        assert.equal(response.status, 200);
        assert.equal(largest(html), maximum);
        assert.equal(smallest(html), minimum);
        assert.ok(html.includes('data-largest-index>Index ' + highIndex));
        assert.ok(html.includes('data-smallest-index>Index ' + lowIndex));
        assert.ok(html.includes('3 rows ×'));
    }
    const rows = Array.from({ length: 3 }, (_, row) => Array.from({ length: 12 }, (_, col) => row * 12 + col).join(','));
    const { html } = await submit(3, rowFields(rows));
    assert.equal(largest(html), '35');
    assert.equal(smallest(html), '0');
    assert.equal((html.match(/<td>/g) || []).length, 36);
});

test('2D extremes preserve decimals and first indexes for ties across three rows', async () => {
    const { html } = await submit(3, rowFields([' 2.75,-1.25,2.75 ', '-1.25,0,1', '1,2,-1']));
    assert.equal(largest(html), '2.75');
    assert.equal(smallest(html), '-1.25');
    assert.ok(html.includes('data-largest-index>Index [0, 0]'));
    assert.ok(html.includes('data-smallest-index>Index [0, 1]'));
    const equal = await submit(3, rowFields(['0,0','0,0','0,0']));
    assert.equal(largest(equal.html), '0');
    assert.equal(smallest(equal.html), '0');
    assert.ok(equal.html.includes('data-largest-index>Index [0, 0]'));
    assert.ok(equal.html.includes('data-smallest-index>Index [0, 0]'));
});

test('2D extremes reject missing rows, unequal lengths and invalid row values', async () => {
    for (const row of ['',' ', '1,2', '1,,2', '1,', '1\n2', 'hello', '<script>alert(1)</script>',
        'NaN','Infinity','1e3','1000000001','-1000000001','9'.repeat(41),
        Array(13).fill('1').join(','), '1'.repeat(501)]) {
        const { response, html, raw } = await submit(3, { Row1:'1', Row2:row, Row3:'3', Result:'Forged' });
        assert.equal(response.status, 200);
        assert.match(html, /data-example-result[^>]*hidden/);
        assert.ok(html.includes('data-array-errors'));
        assert.ok(html.includes('aria-invalid="true"'));
        assert.ok(html.includes('href="#Row2"'), 'the error links to the invalid row');
        assert.ok(!raw.includes('<script>alert(1)</script>'));
    }
    for (const omitted of ['Row1','Row2','Row3']) {
        const fields = rowFields(['1','2','3']);
        delete fields[omitted];
        const { html } = await submit(3, fields);
        assert.match(html, /data-example-result[^>]*hidden/);
        assert.ok(html.includes('href="#' + omitted + '"'));
    }
    const page = await (await fetch(path(3))).text();
    assert.equal((page.match(/name="Row[123]"/g) || []).length, 3);
    assert.ok(!page.includes('name="Values"'));
    assert.equal((await submit(3, rowFields(['1','2','3']), false)).response.status, 400);
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

});

test('one-dimensional forms reject non-numbers even with a forged Names option', async () => {
    for (const number of [1, 2]) {
        const page = await (await fetch(path(number))).text();
        assert.ok(!page.includes('name="ValueKind"'));
        for (const Values of ['', ',,,', 'Ana,Ben', '1,hello', '<script>alert(1)</script>',
            '-79228162514264337593543950335', '1000000001', Array(31).fill('1').join(','), '9'.repeat(41)]) {
            const { response, html, raw } = await submit(number, { Values, ValueKind:'Names', Direction:'Ascending', Result:'Forged' });
            assert.equal(response.status, 200);
            assert.match(html, /data-example-result[^>]*hidden/);
            assert.ok(html.includes('data-array-errors'));
            assert.ok(!raw.includes('<script>alert(1)</script>'));
        }
        // Unknown legacy fields are ignored, but the submitted values are still parsed numerically.
        const numeric = await submit(number, { Values:'10,2', ValueKind:'Names', Direction:'Ascending' });
        assert.ok(!numeric.html.match(/data-example-result[^>]*hidden/));
    }
    const invalidDirection = await submit(2, { Values:'1,2', Direction:'99' });
    assert.match(invalidDirection.html, /data-example-result[^>]*hidden/);
});
