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
    const raw = await response.text(); // Keep original HTML for encoding checks before decoding readable text.
    return { response, raw, html: decode(raw) };
}

test('pyramid output preserves centering, row widths and encoded symbols', async () => {
    for (const [Rows, Symbol, expected] of [
        ['1', '*', '*'], ['3', '*', '  *\n * *\n* * *'], ['2', '<', ' <\n< <'],
        ['5', '*', '    *\n   * *\n  * * *\n * * * *\n* * * * *']
    ]) {
        const { html } = await submit(1, { Rows, Symbol });
        // Compare exact whitespace so extra trailing spaces cannot hide behind trimming.
        assert.equal(html.match(/aria-label="Generated pyramid">([\s\S]*?)<\/pre>/)?.[1], expected);
        assert.ok(!html.includes('symbols, separated by one space.'));
        assert.ok(!html.includes(`<strong>${Rows} rows</strong>`));
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
        [1, {}],
        [1, { Rows: '0', Symbol: '*' }], [1, { Rows: '21', Symbol: '*' }],
        [1, { Rows: '3', Symbol: 'ab' }], [2, { Number: '-5' }], [2, { Number: '1.2' }],
        [2, { Number: '9'.repeat(19) }], [3, { Number: '1' }], [3, { Number: '2.5' }],
        [3, { Number: '1000000000001' }]
    ]) {
        const { html } = await submit(number, { ...fields, Result: 'Forged' });
        assert.match(html, /data-example-result[^>]*hidden/);
        assert.ok(html.includes('data-loop-errors'));
    }
    for (let number = 1; number <= 5; number++) {
        assert.equal((await submit(number, {}, false)).response.status, 400);
    }
});

const roundToken = html => html.match(/name="StateToken"[^>]*value="([^"]+)"/)?.[1];

test('all five loop slots support canonical pages and partial navigation', async () => {
    for (let number = 1; number <= 5; number++) {
        for (const headers of [{}, { 'X-Showcase-Partial': 'true' }]) {
            const response = await fetch(path(number), { headers });
            const html = await response.text();
            assert.equal(response.status, 200);
            assert.ok(!html.includes('Not implemented yet'));
            assert.ok(html.includes('data-example-form'));
            assert.ok(html.includes(`href="/concepts/loops/example-${number}"`));
            if (headers['X-Showcase-Partial']) assert.ok(!html.includes('<!DOCTYPE html>'));
        }
    }
});
const outcome = html => html.match(/<strong>([^<]+)<\/strong>/)?.[1];
async function freshRound(number) {
    const html = await (await fetch(path(number))).text();
    return { html: decode(html), token: roundToken(html) };
}

test('guessing game keeps one target across retries and stops after a correct guess', async () => {
    let { token } = await freshRound(4);
    let low = 1;
    let high = 100;
    let won = false;
    for (let attempt = 1; attempt <= 7; attempt++) {
        const Guess = Math.floor((low + high) / 2);
        const { html } = await submit(4, { Guess: String(Guess), StateToken: token, Target: String(Guess), Result: 'Forged' });
        token = roundToken(html);
        const result = outcome(html);
        if (result === 'You found it!') {
            won = true;
            const repeated = await submit(4, { Guess: '1', StateToken: token });
            assert.equal(outcome(repeated.html), 'You found it!');
            assert.ok(repeated.html.includes(`${10 - attempt} of 10 attempts remaining`));
            break;
        }
        assert.ok(['Too low', 'Too high'].includes(result));
        if (result === 'Too low') low = Guess + 1;
        else high = Guess - 1;
        assert.ok(html.includes(`${10 - attempt} of 10 attempts remaining`));
    }
    assert.ok(won, 'consistent higher/lower feedback finds a number from 1 to 100 in at most seven guesses');
});

test('a known incorrect guess eventually exhausts the bounded round', async () => {
    let { token } = await freshRound(4);
    for (let attempt = 1; attempt <= 10; attempt++) {
        const { html } = await submit(4, { Guess: '50', StateToken: token });
        token = roundToken(html);
        if (outcome(html) === 'You found it!') {
            assert.equal(attempt, 1); // A target of 50 finishes immediately rather than consuming more attempts.
            return;
        }
        if (attempt === 10) {
            assert.equal(outcome(html), 'No attempts left');
            const extra = await submit(4, { Guess: '1', StateToken: token });
            assert.equal(outcome(extra.html), 'No attempts left');
            assert.ok(extra.html.includes('0 of 10 attempts remaining'));
        }
    }
});

test('digit counter uses one iteration for zero and counts significant decimal digits', async () => {
    for (const [Number, expected] of [['0', 1], ['7', 1], ['10', 2], ['4725', 4],
        ['000472', 3], ['000000', 1], ['999999999999999999', 18], ['100000000000000000', 18]]) {
        const { response, html } = await submit(5, { Number, Count: '999', Result: 'Forged' });
        assert.equal(response.status, 200);
        assert.equal(outcome(html), 'Digit count: ' + expected);
        assert.equal((html.match(/<li>Count /g) || []).length, expected);
        assert.ok(html.includes('Leading zeros are ignored; zero counts as one digit.'));
    }
});

test('digit counter rejects missing, malformed and excessive input without results', async () => {
    for (const fields of [{}, { Number: '' }, { Number: ' ' }, { Number: '-5' }, { Number: '+5' },
        { Number: '2.5' }, { Number: '1e3' }, { Number: '12 34' }, { Number: '1,000' },
        { Number: '９' }, { Number: '9'.repeat(19) }, { Number: '<script>alert(1)</script>' }]) {
        const { response, html, raw } = await submit(5, { ...fields, Result: 'Forged' });
        assert.equal(response.status, 200);
        assert.match(html, /data-example-result[^>]*hidden/);
        assert.ok(html.includes('data-loop-errors'));
        assert.ok(html.includes('aria-invalid="true"'));
        assert.ok(!raw.includes('<script>alert(1)</script>'));
    }
    const page = await (await fetch(path(5))).text();
    assert.ok(!page.includes('name="StateToken"'), 'the digit counter needs no protected game round');
});

test('guessing game rejects missing or altered round values and invalid guesses', async () => {
    const guess = await freshRound(4);
    for (const fields of [{ Guess: '50' }, { Guess: '50', StateToken: 'altered' }]) {
        const { html } = await submit(4, fields);
        assert.match(html, /data-example-result[^>]*hidden/);
        assert.ok(html.includes('data-loop-errors'));
    }
    const invalid = await submit(4, { Guess: '0', StateToken: guess.token });
    assert.ok(invalid.html.includes('10 of 10 attempts remaining'));
});
