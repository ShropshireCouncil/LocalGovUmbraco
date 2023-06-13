'use strict';
// Adds expandable menu toggle for touch devices based on a conditional breakpoint.
// Usage: add data-breakpoint="{int}" to any <nav> element.

class MobileMenu {
  hostEntity;

  constructor(element) {
    this.hostEntity = element;
    this.#togglable(element, {
      innerText: 'Menu',
      ariaLabel: 'Open Menu',
      title: 'Open Menu',
    }, {
      innerText: 'Close',
      ariaLabel: 'Close Menu',
      title: 'Close Menu',
    });
  }

  #togglable(el, defaultArgs, activeArgs) {
    let id = el.getAttribute('id');
    if (!id) {
      do {
        id = String.fromCharCode(96 + Math.min(Math.max(performance.now() & 15, 1), 26)) + (performance.now() * 1000).toString(16);
      } while (document.getElementById(id));
      el.setAttribute('id', id);
    }

    el.parentElement.insertBefore(this.#button(e => {
      let open = !(e.target.getAttribute('aria-expanded') == 'true');
      this.#setAttributes(e.target, open ? Object.assign(activeArgs ?? {}, { ariaExpanded: 'true' }) : Object.assign(defaultArgs ?? {}, { ariaExpanded: 'false' }));
      document.getElementById(e.target.ariaControls)?.toggleAttribute('hidden', !open);
    }, Object.assign(defaultArgs ?? {}, {
      ariaExpanded: 'false',
      ariaControls: 'main-menu',
      className: 'menu-toggle',
    })), el);
    el.setAttribute('hidden', true);
  }

  #setAttributes(el, attr) {
    Object.assign(el, attr ?? {});
    Object.entries(attr).forEach(([k, v]) => k.startsWith('aria') ? el.setAttribute(k.replace(/[A-Z]/g, m => "-" + m.toLowerCase()), v) : null); //Bugfix for ARIA attributes in Firefox

    return el;
  }

  #button(fn, args) {
    if (typeof fn != "function") {
      console.error('Second paramenter must be callable function.', arguments);
      return;
    }

    let button = this.#setAttributes(document.createElement('button'), args ?? {});
    button.addEventListener('click', fn);

    return button;
  }

  static destroy(element) {
    if (!(element.$MobileMenu instanceof MobileMenu)) {
      return;
    }

    element.parentElement.querySelectorAll('button.menu-toggle').forEach(x => {
      document.getElementById(x.ariaControls)?.removeAttribute('hidden');
      x.remove();
    });
    delete element.$MobileMenu;
  }

  static $(e) {
    if (e.$MobileMenu instanceof MobileMenu) {
      return;
    }

    e.$MobileMenu = new MobileMenu(e);
  }
}

(() => {
  document.querySelectorAll('nav[data-breakpoint]').forEach(menu => {
    let breakpoint = parseInt(menu.dataset.breakpoint?.match(/\d+/g)?.join('') ?? 0);
    if (breakpoint) {
      let media = window.matchMedia('(min-width: ' + breakpoint + 'px)'), fn = m => m.matches ? MobileMenu.destroy(menu) : MobileMenu.$(menu);
      media.addEventListener('change', fn);
      fn(media);
    }
    else {
      MobileMenu.$(menu);
    }
  });
})();
