'use strict';
// Adds a data attribute to all off-domain urls.
Object.defineProperty(HTMLAnchorElement.prototype, 'isExternal', {
  get() {
    if (this.href) {
      try {
        let link = new URL(this.href);

        return link.host && link.host.replace(/^www\./, '') !== window.location.host.replace(/^www\./, '');
      }
      catch (e) { }
    }

    return false;
  },
  enumerable: false,
  configurable: false,
});

(() => document.querySelectorAll('a[href]').forEach(a => a.toggleAttribute('data-external', a.isExternal)))();
