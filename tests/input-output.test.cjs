const assert = require('node:assert/strict');
const test = require('node:test');

const base = process.env.SHOWCASE_BASE_URL || 'http://localhost:5196';

// Native HTTP submissions verify the forms and antiforgery tokens without JavaScript.
async function submit(number, fields, headers = {}) {
    const url = `${base}/concepts/input-output/example-${number}`;
    const page = await fetch(url);
    assert.equal(page.status, 200);
    const html = await page.text();
    const token = html.match(/<input[^>]*name="__RequestVerificationToken"[^>]*value="([^"]+)"/);
    assert.ok(token, 'the form includes an antiforgery token');
    const body = new URLSearchParams({ ...fields, __RequestVerificationToken: token[1] });
    const cookies = page.headers.getSetCookie().map(cookie => cookie.split(';')[0]).join('; ');
    const response = await fetch(url, { method: 'POST', body, headers: { ...headers, Cookie: cookies } });
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

test('rectangle area and perimeter handle decimals, bounds and forged outputs', async () => {
    for (const [length, width, area, perimeter] of [
        ['5', '3', '15', '16'], ['2.5', '1.2', '3', '7.4'],
        ['0.01', '0.01', '0.0001', '0.04'], ['1000000', '1000000', '1000000000000', '4000000']
    ]) {
        const html = await submit(2, { Length: length, Width: width, Area: '999', Perimeter: '999' });
        assert.ok(html.includes(`data-rectangle-area>${area} square units`));
        assert.ok(html.includes(`data-rectangle-perimeter>${perimeter} units`));
        assert.ok(!html.match(/data-example-result[^>]*hidden/));
    }
    for (const dimensions of [{}, { Length: '0', Width: '3' }, { Length: '-1', Width: '3' }, { Length: '2', Width: '1000001' }, { Length: 'abc', Width: '3' }]) {
        const html = await submit(2, dimensions);
        assert.ok(html.includes('field-validation-error'));
        assert.match(html, /data-example-result[^>]*hidden/);
    }
    const missingToken = await fetch(`${base}/concepts/input-output/example-2`, { method: 'POST', body: new URLSearchParams({ Length: '5', Width: '3' }) });
    assert.equal(missingToken.status, 400);
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

// Check the actual labels returned by both rendering paths, including the special 1st/2nd/3rd endings.
test('ID year labels agree between native and enhanced previews for all supported years', async () => {
    const labels = ['1st year', '2nd year', '3rd year', '4th year', '5th year', '6th year'];
    for (const [index, label] of labels.entries()) {
        const fields = { FullName: 'Mira Santos', StudentNumber: '241611', Course: 'BSIT', YearLevel: String(index + 1) };
        const html = await submit(1, fields);
        assert.ok(html.includes(`data-id-year>${label}</dd>`));
        const preview = JSON.parse(await submit(1, fields, { 'X-Id-Preview': 'true' }));
        assert.equal(preview.yearLabel, label);
        assert.equal(preview.yearLevel, index + 1);
    }
});
