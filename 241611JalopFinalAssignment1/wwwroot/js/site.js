(() => {
    'use strict';
    const desktop = document.querySelector('[data-desktop]');
    if (!desktop) return;
    const host = document.getElementById('window-host');
    const dialog = document.querySelector('[data-search-dialog]');
    const input = document.querySelector('[data-search-input]');
    const notice = document.querySelector('.navigation-notice');
    let pendingRequest;
    let maximized = false;
    let minimized = false;
    let lastWindowUrl = null;

    function syncDesktop() {
        const windowElement = host.querySelector('[data-window]');
        const category = windowElement?.dataset.category;
        desktop.classList.toggle('has-window', Boolean(windowElement) && !minimized);
        document.querySelector('.welcome').inert = Boolean(windowElement) && !minimized;
        windowElement?.classList.toggle('is-minimized', minimized);
        windowElement?.classList.toggle('is-maximized', maximized);
        const maximize = windowElement?.querySelector('[data-maximize]');
        maximize?.setAttribute('aria-pressed', String(maximized));
        maximize?.setAttribute('aria-label', maximized ? 'Restore window size' : 'Maximize window');
        document.querySelectorAll('.desktop-folder').forEach(folder => {
            if (category && folder.dataset.category === category) folder.setAttribute('aria-current', 'page');
            else folder.removeAttribute('aria-current');
        });
        document.querySelectorAll('.dock-item').forEach(item => {
            const active = category ? item.dataset.category === category : item.getAttribute('href') === (windowElement ? '/about' : '/');
            item.classList.toggle('is-active', active);
            if (active) item.setAttribute('aria-current', 'page');
            else item.removeAttribute('aria-current');
        });
        if (windowElement) lastWindowUrl = location.pathname;
        document.title = `${windowElement?.querySelector('#window-title')?.textContent || 'Desktop'} — C# Programming Showcase`;
    }

    async function navigate(url, { push = true } = {}) {
        pendingRequest?.abort();
        const controller = new AbortController();
        pendingRequest = controller;
        const noticeTimer = setTimeout(() => { notice.hidden = false; }, 450);
        host.setAttribute('aria-busy', 'true');
        desktop.classList.add('is-navigating');
        try {
            const response = await fetch(url, {
                headers: { 'X-Showcase-Partial': 'true' },
                signal: controller.signal
            });
            if (!response.ok) throw new Error(`Navigation failed: ${response.status}`);
            const html = await response.text();
            if (controller.signal.aborted) return;
            host.innerHTML = html;
            if (push) history.pushState({}, '', url);
            minimized = false;
            syncDesktop();
            const detail = host.querySelector('.example-detail');
            if (detail) detail.focus({ preventScroll: true });
            else if (!host.querySelector('[data-window]')) document.querySelector('.welcome-action').focus({ preventScroll: true });
            else {
                const title = host.querySelector('#window-title');
                title.setAttribute('tabindex', '-1');
                title.focus({ preventScroll: true });
            }
        } catch (error) {
            if (error.name !== 'AbortError') location.assign(url);
        } finally {
            clearTimeout(noticeTimer);
            if (pendingRequest === controller) {
                notice.hidden = true;
                host.removeAttribute('aria-busy');
                desktop.classList.remove('is-navigating');
            }
        }
    }

    function openSearch() {
        if (!dialog.open) dialog.showModal();
        input.focus();
    }

    document.addEventListener('click', event => {
        if (!(event.target instanceof Element)) return;
        if (event.target.closest('[data-search-open]')) { openSearch(); return; }
        if (event.target.closest('[data-search-close]')) { dialog.close(); return; }
        if (event.target === dialog) {
            const bounds = dialog.getBoundingClientRect();
            if (event.clientX < bounds.left || event.clientX > bounds.right || event.clientY < bounds.top || event.clientY > bounds.bottom) dialog.close();
            return;
        }
        if (event.target.closest('[data-minimize]')) {
            minimized = true;
            syncDesktop();
            const activeDock = document.querySelector('.dock-item.is-active');
            activeDock?.focus({ preventScroll: true });
            return;
        }
        if (event.target.closest('[data-maximize]')) { maximized = !maximized; syncDesktop(); return; }
        const link = event.target.closest('a[data-navigate]');
        if (!link || event.button !== 0 || event.metaKey || event.ctrlKey || event.shiftKey || event.altKey || link.target === '_blank') return;
        const destination = new URL(link.href, location.href);
        if (destination.origin !== location.origin) return;
        event.preventDefault();
        if (dialog.open) dialog.close();
        if (minimized && link.closest('.dock') && lastWindowUrl && (link.dataset.category === host.querySelector('[data-window]')?.dataset.category || destination.pathname === '/about' && !link.dataset.category)) {
            minimized = false;
            syncDesktop();
            host.querySelector('.example-detail')?.focus({ preventScroll: true });
        } else navigate(destination.pathname);
    });

    input.addEventListener('input', () => {
        const query = input.value.toLocaleLowerCase().trim();
        let matches = 0;
        document.querySelectorAll('[data-search-text]').forEach(result => {
            result.hidden = !result.dataset.searchText.toLocaleLowerCase().includes(query);
            if (!result.hidden) matches++;
        });
        document.querySelector('.search-empty').hidden = matches > 0;
    });
    dialog.addEventListener('close', () => { input.value = ''; input.dispatchEvent(new Event('input')); });
    document.addEventListener('keydown', event => {
        if ((event.metaKey || event.ctrlKey) && event.key.toLowerCase() === 'k') { event.preventDefault(); openSearch(); }
        if (event.key === 'Escape' && !dialog.open && host.querySelector('[data-window]') && !minimized) navigate('/');
    });
    window.addEventListener('popstate', () => navigate(location.pathname, { push: false }));
    const clock = document.querySelector('[data-clock]');
    function updateClock() {
        const now = new Date();
        clock.dateTime = now.toISOString();
        const date = new Intl.DateTimeFormat('en-US', { weekday: 'short', month: 'short', day: 'numeric' }).format(now);
        const time = new Intl.DateTimeFormat('en-US', { hour: 'numeric', minute: '2-digit', hour12: false }).format(now);
        clock.textContent = `${date.replace(',', '')}  ${time}`;
    }
    updateClock();
    setInterval(updateClock, 30000);
    syncDesktop();
})();
