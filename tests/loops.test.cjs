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
        ['1', '*', '*'], ['3', '*', '  *\n * *\n* * *'], ['2', '<', ' <\n< <'],
        ['5', '*', '    *\n   * *\n  * * *\n * * * *\n* * * * *']
    ]) {
        const { html } = await submit(1, { Rows, Symbol });
        // Compare exact whitespace so extra trailing spaces cannot hide behind trimming.
        assert.equal(html.match(/aria-label="Generated pyramid">([\s\S]*?)<\/pre>/)?.[1], expected);
        if (Rows === '5') assert.ok(html.includes('15 symbols, separated by one space.'));
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

function correctQuizAnswer(html) {
    if (html.includes('Which loop executes')) return 2;
    if (html.includes('When does a while')) return 0;
    assert.ok(html.includes('How many iterations'));
    return 1;
}

test('quiz retries keep the same question and finish on a correct answer', async () => {
    const round = await freshRound(5);
    const correct = correctQuizAnswer(round.html);
    const wrong = (correct + 1) % 3;
    const retry = await submit(5, { Answer: String(wrong), StateToken: round.token, QuestionId: '999' });
    assert.equal(outcome(retry.html), 'Try again');
    assert.ok(retry.html.includes('2 of 3 attempts remaining'));
    assert.equal(correctQuizAnswer(retry.html), correct);
    const success = await submit(5, { Answer: String(correct), StateToken: roundToken(retry.html) });
    assert.equal(outcome(success.html), 'Correct!');
    assert.ok(success.html.includes('1 of 3 attempts remaining'));
    const extra = await submit(5, { Answer: String(wrong), StateToken: roundToken(success.html) });
    assert.equal(outcome(extra.html), 'Correct!');
});

test('quiz stops after three incorrect answers and explains the correct answer', async () => {
    const round = await freshRound(5);
    const wrong = (correctQuizAnswer(round.html) + 1) % 3;
    let token = round.token;
    for (let attempt = 1; attempt <= 3; attempt++) {
        const { html } = await submit(5, { Answer: String(wrong), StateToken: token });
        token = roundToken(html);
        assert.equal(outcome(html), attempt === 3 ? 'Attempts used' : 'Try again');
        if (attempt === 3) assert.ok(html.includes('Answer:'));
    }
    const extra = await submit(5, { Answer: '0', StateToken: token });
    assert.equal(outcome(extra.html), 'Attempts used');
    assert.ok(extra.html.includes('0 of 3 attempts remaining'));
});

test('altered, missing and cross-game round values are rejected without progress', async () => {
    const guess = await freshRound(4);
    const quiz = await freshRound(5);
    for (const [number, fields] of [
        [4, { Guess: '50' }], [4, { Guess: '50', StateToken: 'altered' }],
        [4, { Guess: '50', StateToken: quiz.token }], [5, { Answer: '0', StateToken: guess.token }]
    ]) {
        const { html } = await submit(number, fields);
        assert.match(html, /data-example-result[^>]*hidden/);
        assert.ok(html.includes('data-loop-errors'));
    }
    const invalid = await submit(4, { Guess: '0', StateToken: guess.token });
    assert.ok(invalid.html.includes('10 of 10 attempts remaining'));
    const invalidAnswer = await submit(5, { Answer: '3', StateToken: quiz.token });
    assert.ok(invalidAnswer.html.includes('3 of 3 attempts remaining'));
});
