const assert = require('node:assert/strict');
const test = require('node:test');
const base = process.env.SHOWCASE_BASE_URL || 'http://localhost:5196';
const path = number => `${base}/concepts/loops/example-${number}`;
const decode = html => html.replace(/&#x([\da-f]+);/gi, (_, code) => String.fromCodePoint(parseInt(code, 16)))
    .replaceAll('&gt;', '>').replaceAll('&lt;', '<').replaceAll('&amp;', '&');

// Exercise native MVC binding and antiforgery rather than duplicating the loop algorithms in JavaScript.
async function submit(number, fields, tokenRequired = true) {
    const page = await fetch(path(number));
    assert.equal(page.status, 200);
    const html = await page.text();
    const token = html.match(/name="__RequestVerificationToken"[^>]*value="([^"]+)"/)[1];
    const body = new URLSearchParams(fields);
    if (tokenRequired) body.set('__RequestVerificationToken', token);
    const cookies = page.headers.getSetCookie().map(cookie => cookie.split(';')[0]).join('; ');
    const response = await fetch(path(number), { method: 'POST', body, headers: { Cookie: cookies } });
    return { response, html: decode(await response.text()) };
}

test('pyramid output preserves centering, row widths and encoded symbols', async () => {
    for (const [Rows, Symbol, expected] of [
        ['1', '*', '*'], ['3', '*', '  *\n ***\n*****'], ['2', '<', ' <\n<<<']
    ]) {
        const { html } = await submit(1, { Rows, Symbol });
        assert.equal(html.match(/aria-label="Generated pyramid">([\s\S]*?)<\/pre>/)?.[1]?.trimEnd(), expected);
    }
});

test('digit sums handle zero, leading zeros and the full 18-digit input', async () => {
    for (const [Number, expected] of [['0', 0], ['000472', 13], ['999999999999999999', 162]]) {
        const { html } = await submit(2, { Number });
        assert.ok(html.includes(`<strong>Digit sum: ${expected}</strong>`));
    }
});

test('factorization handles primes, repeated factors and the upper bound', async () => {
    for (const [Number, expected] of [
        ['2', '2 = 2'], ['60', '60 = 2 × 2 × 3 × 5'], ['97', '97 = 97'],
        ['49', '49 = 7 × 7'], ['1000000000000', '1,000,000,000,000 = 2 × 2']
    ]) {
        const { html } = await submit(3, { Number });
        assert.ok(html.includes(expected), Number);
    }
});

test('invalid numeric inputs and missing antiforgery cannot produce loop results', async () => {
    for (const [number, fields] of [
        [1, { Rows: '0', Symbol: '*' }], [1, { Rows: '21', Symbol: '*' }],
        [1, { Rows: '3', Symbol: 'ab' }], [2, { Number: '-5' }], [2, { Number: '1.2' }],
        [2, { Number: '9'.repeat(19) }], [3, { Number: '1' }], [3, { Number: '2.5' }],
        [3, { Number: '1000000000001' }]
    ]) {
        const { html } = await submit(number, { ...fields, Result: 'Forged' });
        assert.match(html, /data-example-result[^>]*hidden/);
        assert.ok(html.includes('data-loop-errors'));
    }
    for (let number = 1; number <= 3; number++) {
        assert.equal((await submit(number, {}, false)).response.status, 400);
    }
});
