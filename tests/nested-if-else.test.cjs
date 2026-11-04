const assert = require('node:assert/strict');
const test = require('node:test');

// Run against the local MVC app: these requests exercise binding, Razor and antiforgery too.
const base = process.env.SHOWCASE_BASE_URL || 'http://localhost:5196';
const path = number => `/concepts/nested-if-else/example-${number}`;
const decode = text => text.replace(/&#x([\da-f]+);/gi, (_, code) => String.fromCodePoint(parseInt(code, 16)))
    .replace(/&#(\d+);/g, (_, code) => String.fromCodePoint(Number(code)))
    .replaceAll('&gt;', '>').replaceAll('&lt;', '<').replaceAll('&amp;', '&');

async function submit(number, fields, { token = true } = {}) {
    const page = await fetch(base + path(number));
    assert.equal(page.status, 200);
    const html = await page.text();
    const input = html.match(/<input[^>]*name="__RequestVerificationToken"[^>]*>/)?.[0];
    assert.ok(input, 'the native form has an antiforgery token');
    const body = new URLSearchParams(fields);
    if (token) body.set('__RequestVerificationToken', input.match(/value="([^"]+)"/)[1]);
    const cookies = page.headers.getSetCookie().map(cookie => cookie.split(';')[0]).join('; ');
    const response = await fetch(base + path(number), { method: 'POST', body, headers: { Cookie: cookies } });
    return { response, html: decode(await response.text()) };
}

function result(html) {
    // A rejected input must never expose a calculated output, even if forged output is posted.
    const panel = html.match(/<div class="example-result"[^>]*>/)?.[0];
    assert.ok(panel);
    return panel.includes('hidden') ? null : html.match(/<div class="example-result"[^>]*>[\s\S]*?<strong>(.*?)<\/strong>/)?.[1];
}

test('all 20 slots and the five partial forms keep their canonical routes', async () => {
    for (const category of ['input-output', 'nested-if-else', 'loops', 'arrays']) {
        for (let number = 1; number <= 5; number++) {
            const response = await fetch(`${base}/concepts/${category}/example-${number}`);
            assert.equal(response.status, 200, `${category}/${number}`);
        }
    }
    for (let number = 1; number <= 5; number++) {
        const response = await fetch(base + path(number), { headers: { 'X-Showcase-Partial': 'true' } });
        const html = await response.text();
        assert.equal(response.status, 200);
        assert.ok(html.includes('data-example-form'));
        assert.ok(!html.includes('<!DOCTYPE html>'));
        assert.ok(!html.includes('Not implemented yet'));
        assert.equal(result(html), null);
    }
    for (const suffix of ['example-0', 'example-6', 'example-nope']) {
        assert.equal((await fetch(`${base}/concepts/nested-if-else/${suffix}`)).status, 404);
    }
});

test('login checks credentials before trusted sample roles and clears passwords', async () => {
    for (const [Username, Password, expected] of [
        ['admin', 'admin123', 'Welcome, Admin'], ['student', 'student123', 'Welcome, Student'],
        ['guest', 'guest123', 'Access denied'], ['admin', 'wrong', 'Invalid credentials'],
        ['unknown', 'admin123', 'Invalid credentials'], ['Admin', 'admin123', 'Invalid credentials']
    ]) {
        const { response, html } = await submit(1, { Username, Password, Role: 'Admin', Result: 'Forged' });
        assert.equal(response.status, 200);
        assert.equal(result(html), expected);
        assert.ok(!html.includes('value="' + Password + '"'), 'password is never returned in a field');
        assert.ok(response.headers.get('cache-control').includes('no-store'));
        if (expected === 'Invalid credentials') assert.ok(!html.includes('<li>Admin role?'));
    }
});

test('all five decisions use compact connected text branches in full and partial routes', async () => {
    for (let number = 1; number <= 5; number++) {
        for (const partial of [false, true]) {
        const html = decode(await (await fetch(base + path(number), { headers: partial ? { 'X-Showcase-Partial': 'true' } : {} })).text());
        const treeMarkup = html.match(/<pre class="decision-tree"[^>]*>([\s\S]*?)<\/pre>/)?.[1];
        const tree = treeMarkup?.replace(/<\/?strong>/g, '');
        assert.ok(tree, 'the tree is keyboard-focusable preserved text');
        const boldLabels = [...treeMarkup.matchAll(/<strong>(.*?)<\/strong>/g)].map(match => match[1]);
        assert.ok(boldLabels.length > 0);
        assert.ok(boldLabels.every(label => label === 'Yes' || label === 'No'));
        assert.ok(tree.includes('├── Yes:'));
        assert.ok(tree.includes('└── No:'));
        assert.ok(!tree.includes('+--'));
        assert.ok(!tree.split('\n').some(line => /^[│\s]*$/.test(line)), 'no unnecessary vertical-only rows');
        assert.ok(!html.includes('login-flow-tree'));
        }
    }
});

test('Latin honors evaluates every hundredth of GWA and preserves precise threshold decisions', async () => {
    for (let hundredths = 100; hundredths <= 500; hundredths++) {
        const Gwa = (hundredths / 100).toFixed(2);
        const expected = hundredths <= 125 ? 'Summa Cum Laude' : hundredths <= 150 ? 'Magna Cum Laude' : hundredths <= 175 ? 'Cum Laude' : 'No Latin Honors';
        const { html } = await submit(2, { Gwa, Result: 'Forged' });
        assert.equal(result(html), expected, `GWA ${Gwa}`);
        assert.ok(html.includes('Based on GWA only.'));
    }
    // Extra precision must not be rounded into the preceding honors category.
    for (const [Gwa, expected] of [['1.2501', 'Magna Cum Laude'], ['1.5001', 'Cum Laude'], ['1.7501', 'No Latin Honors']]) {
        const { html } = await submit(2, { Gwa });
        assert.equal(result(html), expected);
        assert.ok(html.includes(`GWA: ${Gwa}.`));
    }
});

test('BMI uses unrounded adult boundaries and kilograms/meters', async () => {
    for (const [Weight, Height, expected] of [
        ['18.49', '1', 'Underweight'], ['18.5', '1', 'Healthy weight'],
        ['24.999', '1', 'Healthy weight'], ['25', '1', 'Overweight'],
        ['29.999', '1', 'Overweight'], ['30', '1', 'Obesity'], ['60', '1.65', 'Healthy weight']
    ]) {
        const { html } = await submit(3, { Weight, Height });
        assert.equal(result(html), expected);
        if (Height === '1.65') assert.ok(html.includes('BMI: 22.04'));
    }
});

test('triangle validity precedes side equality, including all pair permutations', async () => {
    for (const [A, B, C, expected] of [
        ['5', '5', '5', 'Equilateral'], ['5', '5', '6', 'Isosceles'],
        ['5', '6', '5', 'Isosceles'], ['6', '5', '5', 'Isosceles'],
        ['3', '4', '5', 'Scalene'], ['0.3', '0.4', '0.5', 'Scalene'],
        ['1', '2', '3', 'Not a valid triangle'], ['3', '1', '2', 'Not a valid triangle'],
        ['1', '3', '2', 'Not a valid triangle'], ['1', '1', '5', 'Not a valid triangle']
    ]) {
        const { html } = await submit(4, { A, B, C });
        assert.equal(result(html), expected);
        if (expected === 'Not a valid triangle') assert.ok(!html.includes('<li>All three sides equal?'));
    }
});

test('largest handles each nested branch, negative numbers, decimals and ties', async () => {
    for (const [A, B, C, expected] of [
        ['9', '5', '3', '9'], ['5', '3', '9', '9'], ['3', '9', '5', '9'], ['3', '5', '9', '9'],
        ['-12', '-25', '-18', '-12'], ['1.25', '1.26', '1.255', '1.26'],
        ['5', '5', '3', '5'], ['5', '3', '5', '5'], ['3', '5', '5', '5'],
        ['0', '0', '0', '0'], ['-2', '-2', '-2', '-2']
    ]) {
        const { html } = await submit(5, { A, B, C, Result: 'Forged' });
        assert.equal(result(html), `Largest: ${expected}`);
    }
});

test('missing, malformed, negative and excessive inputs fail without an output', async () => {
    const cases = [
        [1, {}], [1, { Username: 'admin', Password: 'x'.repeat(81) }],
        [2, { Gwa: '0.99' }], [2, { Gwa: '5.01' }],
        [2, { Gwa: 'oops' }], [2, { Gwa: '-1' }], [2, {}],
        [3, { Weight: '60', Height: '0' }], [3, { Weight: '-1', Height: '1.65' }],
        [3, { Weight: '60', Height: '0.00000000001' }], [3, { Weight: 'NaN', Height: '1.65' }],
        [4, { A: '0', B: '5', C: '5' }], [4, { A: '-1', B: '5', C: '5' }],
        [5, { A: '1', B: '2' }], [5, { A: '1000000000001', B: '2', C: '3' }]
    ];
    for (const [number, fields] of cases) {
        const { response, html } = await submit(number, { ...fields, Result: 'Forged', Detail: 'Forged' });
        assert.equal(response.status, 200);
        assert.equal(result(html), null, JSON.stringify(fields));
        assert.ok(html.includes('field-validation-error'));
        assert.ok(html.includes('data-decision-errors'), 'invalid inputs have an accessible error summary');
        assert.ok(html.includes('aria-invalid="true"'), 'the invalid field is identified for assistive technology');
    }
    for (let number = 1; number <= 5; number++) {
        const { response } = await submit(number, {}, { token: false });
        assert.equal(response.status, 400, 'antiforgery is required for every form');
    }
});
