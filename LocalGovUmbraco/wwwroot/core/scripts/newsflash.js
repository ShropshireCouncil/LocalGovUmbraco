(() => {
  'use strict';
  // Adds a "dismiss" button to notification panels
  // Usage: Automatic. Will add dismiss links to any aside element with the .notification class, and a valid unique ID.

  window.nfHide = JSON.parse(Object.fromEntries(new URLSearchParams(document.cookie.replace(/; ?/g, '&')))?.['nfHide'] || null) ?? [];
  document.querySelectorAll('aside.notification[id]').forEach(notification => {
    let id = notification.getAttribute('id');
    if (window.nfHide.indexOf(id) > -1) {
      notification.remove();
    }
    else {
      let btn = Object.assign(document.createElement('button'), { innerText: 'Dismiss', className: 'close' });
      btn.addEventListener('click', e => {
        window.nfHide.push(id);
        document.cookie = `nfHide=${JSON.stringify(window.nfHide)}; path=/; max-age=${e || 2592000}; samesite=strict`;
        e.target.parentElement.remove();
      });
      notification.classList.add('dismissable');
      notification.appendChild(btn);
      notification.querySelectorAll('a[href="#dismiss"]').forEach(x => x.addEventListener('click', e => {
        btn.click();
        e.preventDefault();
        return false;
      }));
    }
  });
})();
