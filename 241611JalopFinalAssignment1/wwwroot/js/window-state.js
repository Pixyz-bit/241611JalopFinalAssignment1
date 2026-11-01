(() => {
    // Restore this tab's window size before CSS paints the newly submitted page.
    try {
        document.documentElement.classList.toggle('window-maximized', sessionStorage.getItem('showcase-window-maximized') === 'true');
    } catch {
        // Storage can be unavailable; the app still opens at its normal size.
    }
})();
