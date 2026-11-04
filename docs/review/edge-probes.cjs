// Run with node docs/review/edge-probes.cjs while the local website is running.
const base = process.env.SHOWCASE_BASE_URL || 'http://localhost:5196';
async function submit(path, fields) {
    const page = await fetch(base + path);
    const html = await page.text();
    const token = html.match(/name="__RequestVerificationToken"[^>]*value="([^"]+)"/)[1];
    // Reuse the matching cookie so antiforgery validation exercises the actual controller.
    const cookie = page.headers.getSetCookie().map(value => value.split(';')[0]).join('; ');
    const response = await fetch(base + path, { method: 'POST', headers: { Cookie: cookie }, body: new URLSearchParams({ ...fields, __RequestVerificationToken: token }) });
    return { status: response.status, html: await response.text() };
}
(async () => {
    const path = '/concepts/input-output/example-3';
    const swapped = await submit(path, { Value: '1000000', FromUnit: 'Celsius', ToUnit: 'Fahrenheit', command: 'swap' });
    console.log('Swap produces 1800032:', /name="Value"[^>]*value="1800032"/.test(swapped.html));
    const roundTrip = await submit(path, { Value: '1800032', FromUnit: 'Fahrenheit', ToUnit: 'Celsius' });
    console.log('Swapped value rejected:', roundTrip.html.includes('field-validation-error'));
    const markup = await submit('/concepts/input-output/example-1', { FullName: '<script>alert(1)</script>', StudentNumber: 'audit', Course: 'BSIT', YearLevel: '1' });
    console.log('ID markup escaped:', !markup.html.includes('<script>alert(1)</script>') && markup.html.includes('&lt;script&gt;'));
    for (const path of ['/concepts/arrays/example-0', '/concepts/loops/example-6', '/concepts/unknown/example-1', '/concepts/arrays/example-2147483648']) {
        console.log(path, (await fetch(base + path)).status);
    }
})().catch(error => { console.error(error); process.exitCode = 1; });
