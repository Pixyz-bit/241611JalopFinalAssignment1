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
const cells = entries => Object.fromEntries(entries.map((value, index) => [`Cells[${index}]`, String(value)]));
const members = entries => Object.fromEntries(entries.map((value, index) => [`Members[${index}]`, value]));

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

test('grid search finds all matches with row and column positions', async () => {
    const matrix = cells([5, 2, 5, 4, 5, 6, 7, 8, 9]);
    const found = await submit(4, { ...matrix, SearchValue: '5' });
    assert.equal(outcome(found.html), '3 matches');
    for (const position of ['Row 1, column 1', 'Row 1, column 3', 'Row 2, column 2']) assert.ok(found.html.includes(position));
    const missing = await submit(4, { ...matrix, SearchValue: '100' });
    assert.equal(outcome(missing.html), 'Not found');
    const decimal = await submit(4, { ...cells([-1.5, 0, 1, 2, 3, 4, 5, 6, 7]), SearchValue: '-1.50' });
    assert.equal(outcome(decimal.html), '1 match');
});

test('group organizer preserves row membership, trims and encodes names', async () => {
    const names = [' Ana ', 'Ben', 'Carlo', 'Dana', 'Eli', 'Faye', 'Gio', 'Hana', '<b>Ivan</b>'];
    const result = await submit(5, members(names));
    assert.equal(outcome(result.html), '3 groups organized');
    assert.ok(result.html.includes('<td>Ana</td>'));
    assert.ok(result.html.includes('<td>Faye</td>'));
    assert.ok(!result.raw.includes('<td><b>Ivan</b></td>'));
});

test('incomplete, malformed and oversized grid collections return validation instead of errors', async () => {
    for (const [number, fields] of [
        [4, { SearchValue: '5' }], [4, { ...cells([1, 2]), SearchValue: '1' }],
        [4, { ...cells([1, 2, 3, 4, 5, 6, 7, 8, 'oops']), SearchValue: '1' }],
        [4, { ...cells(Array(10).fill(1)), SearchValue: '1' }],
        [4, { ...cells(Array(9).fill(1)), SearchValue: '1000000001' }],
        [5, {}], [5, members(Array(10).fill('Ana'))],
        [5, members(['', ...Array(8).fill('Ana')])], [5, members(['A'.repeat(41), ...Array(8).fill('Ana')])]
    ]) {
        const { response, html } = await submit(number, fields);
        assert.equal(response.status, 200);
        assert.match(html, /data-example-result[^>]*hidden/);
        assert.ok(html.includes('data-array-errors'));
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
