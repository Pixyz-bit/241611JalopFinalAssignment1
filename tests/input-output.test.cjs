const assert = require('node:assert/strict');
const test = require('node:test');

const base = process.env.SHOWCASE_BASE_URL || 'http://localhost:5196';

// Native HTTP submissions verify the forms and antiforgery tokens without JavaScript.
async function submit(number, fields) {
    const url = `${base}/concepts/input-output/example-${number}`;
    const page = await fetch(url);
    assert.equal(page.status, 200);
    const html = await page.text();
    const token = html.match(/<input[^>]*name="__RequestVerificationToken"[^>]*value="([^"]+)"/);
    assert.ok(token, 'the form includes an antiforgery token');
    const body = new URLSearchParams({ ...fields, __RequestVerificationToken: token[1] });
    const cookies = page.headers.getSetCookie().map(cookie => cookie.split(';')[0]).join('; ');
    const response = await fetch(url, { method: 'POST', body, headers: { Cookie: cookies } });
    assert.equal(response.status, 200);
    return await response.text();
}

test('temperature conversion and native Swap preserve the confirmed value', async () => {
    const fields = { Value: '25', FromUnit: 'Celsius', ToUnit: 'Fahrenheit' };
    const result = await submit(3, fields);
    assert.match(result, />25[^<]*77[^<]*<\/strong>/);
    assert.ok(!result.match(/data-example-result[^>]*hidden/));
    const swapped = await submit(3, { ...fields, command: 'swap' });
    assert.match(swapped, /name="Value"[^>]*value="77"/);
    assert.match(swapped, /selected="selected" value="Fahrenheit"|value="Fahrenheit" selected="selected"/);
    assert.match(swapped, /data-example-result[^>]*hidden/);
});

test('time conversion, counting and ID preview render native results', async () => {
    const time = await submit(4, { Value: '23', FromUnit: 'Hours', ToUnit: 'Seconds' });
    assert.ok(time.includes('82,800'));
    const counter = await submit(5, { Text: 'Hello student' });
    assert.match(counter, /data-character-count>13<\/strong>/);
    assert.match(counter, /data-word-count>2<\/strong>/);
    const card = await submit(1, { FullName: 'Mira Santos', StudentNumber: '241611', YearLevel: '2', Course: 'Computer Science' });
    assert.match(card, /data-id-name>Mira Santos</);
    assert.ok(!card.match(/data-id-preview[^>]*hidden/));
    assert.ok(!card.match(/id="id-photo"[^>]*name=/), 'the local photo is excluded from form submissions');
});

test('invalid converter inputs show validation without exposing a result', async () => {
    for (const [number, fields] of [
        [3, { Value: '-274', FromUnit: 'Celsius', ToUnit: 'Fahrenheit' }],
        [4, { Value: '-1', FromUnit: 'Hours', ToUnit: 'Seconds' }]
    ]) {
        const html = await submit(number, fields);
        assert.ok(html.includes('field-validation-error'));
        assert.match(html, /data-example-result[^>]*hidden/);
        assert.ok(!html.match(/data-example-empty[^>]*hidden/));
    }
});
