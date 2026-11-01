// Delegation preserves these behaviors when the desktop replaces a cached example view.
document.addEventListener('input', event => {
    if (!(event.target instanceof Element)) return;
    const form = event.target.closest('[data-example-form]');
    if (!form) return;
    form.dataset.revision = String(Number(form.dataset.revision || 0) + 1);
    form.closest('.example-workspace').querySelectorAll('[data-example-result]').forEach(result => { result.hidden = true; });
    const hint = form.querySelector('[data-example-hint]');
    if (hint) hint.textContent = 'Inputs changed. Submit again to update the result.';
});

// The sole photo reference is an in-memory blob URL, revoked on clear, navigation or page exit.
(() => {
    let photoForm = null;
    let photoUrl = null;
    let photoVersion = 0;
    let photoReady = false;
    function releasePhoto() {
        if (photoForm) {
            const card = photoForm.closest('.id-example').querySelector('[data-id-preview]');
            card.hidden = true;
            card.querySelector('[data-id-photo-preview]').removeAttribute('src');
        }
        photoReady = false;
        if (photoUrl) URL.revokeObjectURL(photoUrl);
        photoUrl = null;
        photoForm = null;
        photoVersion++;
    }
    document.addEventListener('change', event => {
        if (!(event.target instanceof HTMLInputElement) || event.target.id !== 'id-photo') return;
        releasePhoto();
        const form = event.target.closest('[data-id-form]');
        const file = event.target.files[0];
        const status = form.querySelector('[data-id-photo-status]');
        if (!file) { status.textContent = ''; return; }
        if (!['image/jpeg', 'image/png', 'image/webp'].includes(file.type) || file.size > 5 * 1024 * 1024) {
            status.textContent = 'Choose a JPG, PNG or WebP image no larger than 5 MB.';
            event.target.value = '';
            return;
        }
        const version = photoVersion;
        photoForm = form;
        photoUrl = URL.createObjectURL(file);
        const image = new Image();
        image.onload = () => { if (version === photoVersion && form.isConnected) { photoReady = true; status.textContent = file.name + ' selected. Choose Generate ID to preview.'; } };
        image.onerror = () => { if (version === photoVersion) { releasePhoto(); event.target.value = ''; status.textContent = 'This file could not be read as an image. Choose another photo.'; } };
        image.src = photoUrl; // Decode locally; this URL never enters a request or storage API.
    });
    document.addEventListener('submit', async event => {
        const form = event.target;
        if (!(form instanceof HTMLFormElement) || !form.matches('[data-id-form]')) return;
        event.preventDefault();
        if (form.querySelector('#id-photo').files.length && !photoReady) { form.querySelector('[data-example-hint]').textContent = 'Wait for the selected photo to finish loading, or choose another image.'; return; }
        const revision = form.dataset.revision || '0';
        const button = form.querySelector('button[type="submit"]');
        const hint = form.querySelector('[data-example-hint]');
        const error = form.querySelector('[data-id-errors]');
        error.textContent = '';
        button.disabled = true;
        hint.textContent = 'Generating preview…';
        try {
            // The photo input has no name, so only the text fields and antiforgery token are sent.
            const response = await fetch(form.action, { method: 'POST', body: new FormData(form), headers: { 'X-Id-Preview': 'true' } });
            const data = await response.json();
            if (!form.isConnected || (form.dataset.revision || '0') !== revision) return;
            if (!response.ok) {
                error.textContent = data.errors?.join(' ') || 'Check your details and try again.';
                hint.textContent = 'The preview needs valid details.';
                return;
            }
            const card = form.closest('.id-example').querySelector('[data-id-preview]');
            // textContent prevents entered markup from becoming executable HTML.
            card.querySelector('[data-id-name]').textContent = data.fullName;
            card.querySelector('[data-id-number]').textContent = data.studentNumber;
            card.querySelector('[data-id-course]').textContent = data.course;
            card.querySelector('[data-id-year]').textContent = data.yearLevel;
            const image = card.querySelector('[data-id-photo-preview]');
            if (photoForm === form && photoUrl) { image.src = photoUrl; image.hidden = false; }
            else { image.removeAttribute('src'); image.hidden = true; }
            card.querySelector('[data-id-photo-placeholder]').hidden = !image.hidden;
            card.hidden = false;
            hint.textContent = 'ID preview ready. Nothing is saved.';
        } catch {
            if (form.isConnected) hint.textContent = 'Could not generate the preview. Check your connection and try again.';
        } finally { if (form.isConnected) button.disabled = false; }
    });
    document.addEventListener('click', event => {
        if (!(event.target instanceof Element)) return;
        const clear = event.target.closest('[data-id-clear]');
        if (!clear) return;
        event.preventDefault();
        const form = clear.closest('form');
        releasePhoto();
        form.reset();
        ['FullName', 'StudentNumber', 'Course', 'YearLevel'].forEach(name => { form.elements[name].value = ''; });
        form.dataset.revision = String(Number(form.dataset.revision || 0) + 1);
        const card = form.closest('.id-example').querySelector('[data-id-preview]');
        card.hidden = true;
        const image = card.querySelector('[data-id-photo-preview]');
        image.removeAttribute('src'); image.hidden = true;
        form.querySelector('[data-id-photo-status]').textContent = '';
        form.querySelector('[data-id-errors]').textContent = '';
        form.querySelector('[data-example-hint]').textContent = 'Enter your details, then choose Generate ID.';
        form.querySelector('[name="FullName"]').focus();
    });
    new MutationObserver(() => { if (photoForm && !photoForm.isConnected) releasePhoto(); }).observe(document.getElementById('window-host'), { childList: true, subtree: true });
    window.addEventListener('pagehide', () => { if (photoForm) photoForm.querySelector('#id-photo').value = ''; releasePhoto(); });
})();
