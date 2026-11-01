// Delegation preserves these behaviors when the desktop replaces a cached example view.
document.addEventListener('input', event => {
    if (!(event.target instanceof Element)) return;
    const form = event.target.closest('[data-example-form]');
    if (!form) return;
    form.querySelectorAll('[data-example-result]').forEach(result => { result.hidden = true; });
    const hint = form.querySelector('[data-example-hint]');
    if (hint) hint.textContent = 'Inputs changed. Submit again to update the result.';
});
