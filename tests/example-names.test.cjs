const assert = require('node:assert/strict');
const test = require('node:test');
const base = process.env.SHOWCASE_BASE_URL || 'http://localhost:5196';

// Real responses verify all navigation names, canonical destinations and selected states without JavaScript.
test('all categories expose five named links in full and partial pages', async () => {
    const categories = {
        'input-output': ['ID Card Generator', 'Area and Perimeter Calculator', 'Temperature Converter', 'Time Unit Converter', 'Character and Word Counter'],
        'nested-if-else': ['Login and Access Checker', 'Latin Honors Evaluator', 'BMI Calculator', 'Triangle Classifier', 'Largest of Three Numbers'],
        loops: ['Pyramid Generator', 'Sum of Digits', 'Prime Factorization', 'Number Guessing Game', 'Quiz Retry Simulator'],
        arrays: ['Duplicate Finder', 'Array Sort', 'Tic-Tac-Toe', 'Multiplication Table', 'House Member Finder']
    };
    for (const [category, names] of Object.entries(categories)) {
        for (let selected = 1; selected <= 5; selected++) {
            for (const partial of [false, true]) {
                const response = await fetch(`${base}/concepts/${category}/example-${selected}`, {
                    headers: partial ? { 'X-Showcase-Partial': 'true' } : {}
                });
                assert.equal(response.status, 200);
                const html = await response.text();
                assert.ok(!html.includes('app-statusbar'), 'examples reserve no space for a status footer');
                assert.ok(!html.includes('detail-description'), 'example subtitles are removed');
                assert.ok(!html.includes('href="/about"'), 'navigation has no retired About links');
                const links = [...html.matchAll(/<a\b[^>]*class="[^"]*sidebar-example[^>]*>[\s\S]*?<\/a>/g)].map(match => match[0]);
                assert.equal(links.length, 5);
                names.forEach((name, index) => {
                    assert.ok(links[index].includes(`href="/concepts/${category}/example-${index + 1}"`));
                    assert.ok(links[index].includes(`<span>${name}</span>`));
                    assert.equal(links[index].includes('aria-current="page"'), index + 1 === selected);
                });
            }
        }
    }
});

test('category entry links open Example 1 and retired About URLs return 404', async () => {
    const home = await (await fetch(base)).text();
    assert.ok(home.includes('Developed by: Arlan Martin N. Jalop'));
    assert.ok(home.includes('Section: SBIT3C'));
    assert.ok(!home.includes('href="/about"'));
    const categoryLinks = [...home.matchAll(/href="(\/concepts\/[^"\s]+)"/g)].map(match => match[1]);
    assert.ok(categoryLinks.length >= 4);
    assert.ok(categoryLinks.every(link => link.endsWith('/example-1')));
    for (const category of ['input-output', 'nested-if-else', 'loops', 'arrays']) {
        // Test server redirects too: bookmarks and JavaScript-disabled visits must reach the form.
        const response = await fetch(`${base}/concepts/${category}`, { redirect: 'manual' });
        assert.equal(response.status, 302);
        assert.equal(response.headers.get('location'), `/concepts/${category}/example-1`);
        const destination = await fetch(new URL(response.headers.get('location'), base));
        assert.equal(destination.status, 200);
        assert.ok((await destination.text()).includes('aria-current="page"'));
    }
    for (const path of ['/about', '/Home/About']) {
        assert.equal((await fetch(base + path)).status, 404);
    }
});
