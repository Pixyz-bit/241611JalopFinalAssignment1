(() => {
    'use strict';
    const desktop = document.querySelector('[data-desktop]');
    if (!desktop) return;
    const host = document.getElementById('window-host');
    const dialog = document.querySelector('[data-search-dialog]');
    const input = document.querySelector('[data-search-input]');
    const notice = document.querySelector('.navigation-notice');
    const startMenu = document.querySelector('[data-start-menu]'); // Native details stores whether Start is expanded.
    const routeCache = new Map([['/', ''], [location.pathname, host.innerHTML]]);
    let pendingRequest;
    let navigationVersion = 0;
    let maximized = false;
    let minimized = false;
    let lastWindowUrl = null;

    function fitStartPanel() {
        if (!startMenu?.open) return; // Closed details have no measurable panel; fit when Start opens.
        const panel = startMenu.querySelector('.start-panel');
        panel.style.removeProperty('--start-panel-width'); // Reset the outer size before measuring the natural responsive layout.
        panel.style.removeProperty('--start-panel-height');
        const naturalBounds = panel.getBoundingClientRect(); // Preserve fractional pixels so rounding cannot introduce a scrollbar.
        const naturalWidth = naturalBounds.width;
        const naturalHeight = Math.max(panel.scrollHeight, naturalBounds.height); // Include content that scrolls on short screens.
        const bottom = parseFloat(getComputedStyle(panel).bottom); // The CSS offset already reserves the taskbar and its gap.
        const availableHeight = window.innerHeight - bottom - 12; // Leave a small margin at the top of the viewport.
        // The smaller ratio fits both axes; keeping at least 1 preserves readable controls on small screens.
        const scale = Math.max(1, Math.min((window.innerWidth - 24) / naturalWidth, availableHeight / naturalHeight));
        panel.style.setProperty('--start-panel-width', `${naturalWidth * scale}px`); // Enlarge only the box, preserving its width-to-height ratio.
        panel.style.setProperty('--start-panel-height', `${naturalHeight * scale}px`); // Text, icons and controls keep their natural CSS sizes.
    }

    startMenu?.addEventListener('toggle', fitStartPanel); // Measure after native details reveals its contents.
    window.addEventListener('resize', fitStartPanel); // Refit an open panel when the viewport or browser zoom changes.
    document.fonts?.ready.then(fitStartPanel); // Font loading can change the panel's natural height.

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
        document.querySelectorAll('.desktop-shortcut').forEach(shortcut => { // App shortcuts share the same category selection as the sidebar.
            if (category && shortcut.dataset.category === category) shortcut.setAttribute('aria-current', 'page');
            else shortcut.removeAttribute('aria-current');
        });
        document.querySelectorAll('.taskbar-item').forEach(item => {
            const active = category ? item.dataset.category === category : item.getAttribute('href') === (windowElement ? '/about' : '/');
            item.classList.toggle('is-active', active);
            if (active) item.setAttribute('aria-current', 'page');
            else item.removeAttribute('aria-current');
        });
        if (windowElement) lastWindowUrl = location.pathname;
        document.title = `${windowElement?.querySelector('#window-title')?.textContent || 'Desktop'} — C# Programming Showcase`;
    }

    function renderWindow(html) {
        const template = document.createElement('template');
        template.innerHTML = html;
        const nextWindow = template.content.querySelector('[data-window]');
        const currentWindow = host.querySelector('[data-window]');

        if (currentWindow && nextWindow) {
            // Keep the same window mounted: tab changes should not replay its opening animation.
            currentWindow.classList.toggle('about-window', nextWindow.classList.contains('about-window'));
            currentWindow.dataset.category = nextWindow.dataset.category;
            currentWindow.replaceChildren(...nextWindow.childNodes);
        } else {
            host.replaceChildren(template.content);
        }
    }

    async function navigate(url, { push = true } = {}) {
        pendingRequest?.abort();
        const version = ++navigationVersion;
        notice.hidden = true;
        host.removeAttribute('aria-busy');
        if (push && url === location.pathname && !minimized) {
            pendingRequest = undefined;
            return;
        }
        const controller = new AbortController();
        pendingRequest = controller;
        let noticeTimer;
        try {
            let html = routeCache.get(url);
            if (!routeCache.has(url)) {
                noticeTimer = setTimeout(() => {
                    if (version === navigationVersion) notice.hidden = false;
                }, 450);
                host.setAttribute('aria-busy', 'true');
                const response = await fetch(url, {
                    headers: { 'X-Showcase-Partial': 'true' },
                    signal: controller.signal
                });
                if (!response.ok) throw new Error(`Navigation failed: ${response.status}`);
                html = await response.text();
                if (controller.signal.aborted || version !== navigationVersion) return;
                routeCache.set(url, html);
            }
            if (controller.signal.aborted || version !== navigationVersion) return;
            renderWindow(html);
            if (push && url !== location.pathname) history.pushState({}, '', url);
            minimized = false;
            syncDesktop();
            const detail = host.querySelector('.example-detail') || host.querySelector('.about-content'); // Both app pages have a visible focus destination.
            if (detail) detail.focus({ preventScroll: true });
            else if (!host.querySelector('[data-window]')) document.querySelector('.welcome-action').focus({ preventScroll: true });
            else {
                const title = host.querySelector('#window-title');
                title.setAttribute('tabindex', '-1');
                title.focus({ preventScroll: true });
            }
        } catch (error) {
            if (error.name !== 'AbortError' && version === navigationVersion) location.assign(url);
        } finally {
            clearTimeout(noticeTimer);
            if (pendingRequest === controller) {
                notice.hidden = true;
                host.removeAttribute('aria-busy');
            }
        }
    }

    function openSearch() {
        if (startMenu) startMenu.open = false; // Keep Start and search from covering one another.
        startMenu?.querySelector('summary')?.focus(); // Return focus to Start when search closes; the taskbar search bar was removed.
        if (!dialog.open) dialog.showModal();
        input.focus();
    }

    document.addEventListener('click', event => {
        if (!(event.target instanceof Element)) return;
        if (startMenu?.open && !event.target.closest('[data-start-menu]')) startMenu.open = false; // Clicking outside dismisses Start.
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
            const activeApp = document.querySelector('.taskbar-item.is-active'); // The active taskbar shortcut restores the minimized example.
            activeApp?.focus({ preventScroll: true });
            return;
        }
        if (event.target.closest('[data-maximize]')) { maximized = !maximized; syncDesktop(); return; }
        const link = event.target.closest('a[data-navigate]');
        if (!link || event.button !== 0 || event.metaKey || event.ctrlKey || event.shiftKey || event.altKey || link.target === '_blank') return;
        const destination = new URL(link.href, location.href);
        if (destination.origin !== location.origin) return;
        event.preventDefault();
        if (startMenu) startMenu.open = false; // Selecting any canonical link dismisses the menu.
        if (dialog.open) dialog.close();
        const selectedCategory = host.querySelector('[data-window]')?.dataset.category; // About has no category; its shortcut must not restore a different minimized window.
        const restoresWindow = selectedCategory ? link.dataset.category === selectedCategory : destination.pathname === '/about';
        if (minimized && link.closest('.taskbar') && lastWindowUrl && restoresWindow) {
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
        if (event.key === 'Escape' && startMenu?.open) {
            startMenu.open = false; // Escape closes Start before it can close the Explorer window behind it.
            startMenu.querySelector('summary')?.focus();
            return;
        }
        if (event.key === 'Escape' && !dialog.open && host.querySelector('[data-window]') && !minimized) navigate('/');
    });
    window.addEventListener('popstate', () => navigate(location.pathname, { push: false }));
    const clock = document.querySelector('[data-clock]');
    function updateClock() {
        const now = new Date();
        clock.dateTime = now.toISOString();
        const date = new Intl.DateTimeFormat('en-US', { month: 'numeric', day: 'numeric', year: 'numeric' }).format(now); // Use the visitor's local time zone.
        const time = new Intl.DateTimeFormat('en-US', { hour: 'numeric', minute: '2-digit', hour12: false }).format(now);
        clock.textContent = `${time}\n${date}`; // A line break gives the Windows taskbar its two-line clock.
    }
    updateClock();
    setInterval(updateClock, 30000);
    syncDesktop();

    // Delegated listeners also handle converters inserted by cached navigation.

    document.documentElement?.classList?.add('temperature-enhanced');
    function clearTemperatureResult(form, message = 'Inputs changed. Choose Confirm to update the result.') {
        form.querySelector('#temperature-result').value = '';
        form.querySelector('[data-temperature-result-unit]').textContent = '';
        form.dataset.confirmedResult = ''; // Never reuse an output belonging to earlier inputs.
        form.querySelector('#temperature-help').textContent = message;
        form.closest('.temperature-example').querySelector('[data-temperature-formula]').textContent = 'Your calculation will appear here after confirmation.';
        form.querySelectorAll('.temperature-errors').forEach(error => { error.textContent = ''; });
        form.querySelectorAll('[aria-invalid]').forEach(field => field.removeAttribute('aria-invalid'));
    }
    function temperatureInputChanged(event) {
        if (!(event.target instanceof Element)) return;
        const form = event.target.closest('.temperature-form');
        if (form && event.target.matches('[name="Value"], [name="FromUnit"], [name="ToUnit"]')) clearTemperatureResult(form);
    }
    document.addEventListener('input', temperatureInputChanged);
    document.addEventListener('change', temperatureInputChanged);
    document.addEventListener('click', event => {
        if (!(event.target instanceof Element)) return;
        const control = event.target.closest('[data-temperature-swap], [data-temperature-clear]');
        if (!control) return;
        const form = control.closest('.temperature-form');
        event.preventDefault();
        const value = form.querySelector('[name="Value"]');
        const from = form.querySelector('[name="FromUnit"]');
        const to = form.querySelector('[name="ToUnit"]');
        if (control.matches('[data-temperature-swap]')) {
            [from.value, to.value] = [to.value, from.value]; // Exchange the units without calculating in JavaScript.
            if (form.dataset.confirmedResult) value.value = form.dataset.confirmedResult;
            clearTemperatureResult(form, 'Units swapped. Choose Confirm to convert.');
        } else {
            value.value = '';
            from.value = 'Celsius';
            to.value = 'Fahrenheit';
            clearTemperatureResult(form, 'Choose Confirm to update the result.');
        }
        value.focus(); // Return keyboard users to the next editable field.
    });

})();
