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
