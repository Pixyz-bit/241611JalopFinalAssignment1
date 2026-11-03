const assert = require('node:assert/strict');
const test = require('node:test');
const base = process.env.SHOWCASE_BASE_URL || 'http://localhost:5196';

// Real responses verify all navigation names, canonical destinations and selected states without JavaScript.
test('all categories expose five named links in full and partial pages', async () => {
    const categories = {
        'input-output': ['ID Card Generator', 'Area and Perimeter Calculator', 'Temperature Converter', 'Time Unit Converter', 'Character and Word Counter'],
        'nested-if-else': ['Login and Access Checker', 'ATM Withdrawal Simulator', 'BMI Calculator', 'Triangle Classifier', 'Largest of Three Numbers'],
        loops: ['Pyramid Generator', 'Sum of Digits', 'Prime Factorization', 'Number Guessing Game', 'Quiz Retry Simulator'],
        arrays: ['Duplicate Finder', 'Array Sort', 'Tic-Tac-Toe', 'Number Search in a Grid', 'Group Member Organizer']
    };
    for (const [category, names] of Object.entries(categories)) {
        for (let selected = 1; selected <= 5; selected++) {
            for (const partial of [false, true]) {
                const response = await fetch(`${base}/concepts/${category}/example-${selected}`, {
                    headers: partial ? { 'X-Showcase-Partial': 'true' } : {}
                });
                assert.equal(response.status, 200);
                const html = await response.text();
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
