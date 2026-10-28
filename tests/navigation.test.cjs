const assert = require('node:assert/strict');
const { readFileSync } = require('node:fs');
const { resolve } = require('node:path');
const { test } = require('node:test');
const vm = require('node:vm');

const source = readFileSync(resolve(__dirname, '../241611JalopFinalAssignment1/wwwroot/js/site.js'), 'utf8');
const markup = category => `<section data-window data-category="${category}"></section>`;

// Minimal DOM boundary: exercise the real navigation code with deterministic network timing.
function setup(initialPath = '/', initialHtml = '') {
    function classes() {
        const values = new Set();
        return {
            contains: value => values.has(value),
            toggle(value, enabled) { if (enabled) values.add(value); else values.delete(value); }
        };
    }
    function element() {
        const attributes = new Map();
        return {
            classList: classes(), dataset: {}, textContent: '', hidden: true,
            focus() {}, addEventListener() {},
            setAttribute: (key, value) => attributes.set(key, value),
            getAttribute: key => attributes.get(key) ?? null,
            removeAttribute: key => attributes.delete(key)
        };
    }
    function parseWindow(html) {
        const match = html.match(/data-category="([^"]*)"/);
        if (!match) return null;
        const window = element();
        window.dataset.category = match[1];
        window.childNodes = [{ category: match[1] }];
        window.replaceChildren = (...children) => { window.childNodes = children; };
        window.querySelector = selector => {
            if (selector === '#window-title') return { textContent: window.childNodes[0].category };
            if (selector === '.example-detail') return element();
            if (selector === '[data-maximize]') return element();
            return null;
        };
        return window;
    }

    const host = element();
    host.innerHTML = initialHtml;
    host.window = parseWindow(initialHtml);
    host.querySelector = () => host.window;
    host.replaceChildren = fragment => { host.window = fragment.window; };
    const nodes = new Map([
        ['[data-desktop]', element()], ['[data-search-dialog]', element()],
        ['[data-search-input]', element()], ['.navigation-notice', element()],
        ['.welcome', element()], ['.welcome-action', element()], ['[data-clock]', element()]
    ]);
    const requests = [];
    const fallbacks = [];
    const historyEntries = [];
    const queued = [];
    const location = { pathname: initialPath, assign: url => fallbacks.push(url) };
    const context = {
        AbortController, Intl, Date, Map, setTimeout, clearTimeout,
        setInterval() {}, Element: class {}, location,
        history: { pushState(state, title, url) { location.pathname = url; historyEntries.push(url); } },
        window: { addEventListener() {} },
        document: {
            querySelector: selector => nodes.get(selector) ?? null,
            querySelectorAll: () => [], getElementById: () => host,
            addEventListener() {},
            createElement() {
                const template = { content: { window: null } };
                template.content.querySelector = () => template.content.window;
                Object.defineProperty(template, 'innerHTML', { set(html) { template.content.window = parseWindow(html); } });
                return template;
            }
        },
        fetch(url, options) {
            requests.push({ url, signal: options.signal });
            const next = queued.shift();
            return next ? next() : Promise.resolve({ ok: true, text: async () => markup(url) });
        }
    };
    vm.createContext(context);
    const testSource = source.replace(/\}\)\(\);\s*$/, 'globalThis.navigationUnderTest = { navigate }; })();');
    vm.runInContext(testSource, context);
    return { ...context.navigationUnderTest, host, location, requests, queued, fallbacks, historyEntries };
}

test('a visited route is fetched once and its window stays mounted', async () => {
    const app = setup();
    await app.navigate('/concepts/input-output');
    const window = app.host.window;
    await app.navigate('/concepts/conditional');
    assert.equal(app.host.window, window);
    await app.navigate('/concepts/input-output');
    assert.equal(app.host.window, window);
    assert.equal(app.requests.length, 2);
    assert.equal(app.host.window.dataset.category, '/concepts/input-output');
});

test('the initial server view and desktop need no additional request', async () => {
    const app = setup('/concepts/input-output', markup('input-output'));
    await app.navigate('/');
    assert.equal(app.host.window, null);
    await app.navigate('/concepts/input-output');
    assert.equal(app.requests.length, 0);
    assert.equal(app.host.window.dataset.category, 'input-output');
});

test('reselecting the active route neither fetches nor adds history', async () => {
    const app = setup();
    await app.navigate('/concepts/repetition');
    const children = app.host.window.childNodes;
    await app.navigate('/concepts/repetition');
    assert.equal(app.requests.length, 1);
    assert.equal(app.historyEntries.length, 1);
    assert.equal(app.host.window.childNodes, children);
});

test('Back and Forward use cached content without creating history entries', async () => {
    const app = setup();
    await app.navigate('/concepts/input-output');
    await app.navigate('/concepts/conditional');
    for (const path of ['/concepts/input-output', '/concepts/conditional']) {
        app.location.pathname = path;
        await app.navigate(path, { push: false });
        assert.equal(app.host.window.dataset.category, path);
    }
    assert.equal(app.requests.length, 2);
    assert.equal(app.historyEntries.length, 2);
});

test('returning to the active route cancels an older pending tab change', async () => {
    const app = setup();
    await app.navigate('/concepts/input-output');
    let finish;
    app.queued.push(() => new Promise(resolve => { finish = resolve; }));
    const pending = app.navigate('/concepts/conditional');
    await app.navigate('/concepts/input-output');
    assert.equal(app.requests[1].signal.aborted, true);
    finish({ ok: true, text: async () => markup('conditional') });
    await pending;
    assert.equal(app.location.pathname, '/concepts/input-output');
    assert.equal(app.host.window.dataset.category, '/concepts/input-output');
    assert.equal(app.fallbacks.length, 0);
    await app.navigate('/concepts/conditional');
    assert.equal(app.requests.length, 3, 'aborted content must not poison the route cache');
});

test('a late failed request cannot redirect away from a newer selection', async () => {
    const app = setup();
    let fail;
    app.queued.push(() => new Promise((resolve, reject) => { fail = reject; }));
    const pending = app.navigate('/concepts/conditional');
    await app.navigate('/concepts/basic-crud');
    fail(new Error('Older request failed'));
    await pending;
    assert.equal(app.location.pathname, '/concepts/basic-crud');
    assert.equal(app.fallbacks.length, 0);
});

test('failed routes fall back to the server and remain retryable', async () => {
    const app = setup();
    app.queued.push(async () => ({ ok: false, status: 503 }));
    await app.navigate('/concepts/conditional');
    assert.deepEqual(app.fallbacks, ['/concepts/conditional']);
    await app.navigate('/concepts/conditional');
    assert.equal(app.requests.length, 2);
    assert.equal(app.host.window.dataset.category, '/concepts/conditional');
});
