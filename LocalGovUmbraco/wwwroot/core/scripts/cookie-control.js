'use strict';
// Adds an opt-in cookie control widget for GDPR compliance.
// Usage: Automatic. Widget will appear if no exisiting settings are found.

class CookieControl {
  #$$ = 'cookieControl';
  #widget;

  get id() { return this.#$$ };
  get allowEssentialCookies() { return true; }
  allowPersonalisationCookies = false;
  allowAnalyticsCookies = false;

  get cookiePermissions() {
    return Object.assign({}, {
      essential: !!this.allowEssentialCookies,
      personalisation: !!this.allowPersonalisationCookies,
      analytics: !!this.allowAnalyticsCookies,
    });
  }

  set cookiePermissions(v) {
    Object.entries(Object.keys(this.cookiePermissions).reduce((o, k) => ({ ...o, [k]: (v ?? {})[k] ?? this.cookiePermissions[k] }), {})).forEach(([k, v]) => {
      let property = 'allow' + k.charAt(0).toUpperCase() + k.slice(1) + 'Cookies';
      if (Object.getOwnPropertyDescriptor(this, property)?.writable) {
        this[property] = !!v;
      }
    });
  }

  get localSettings() {
    let userSettings = getCookie(this.id) ?? {};

    return Object.keys(this.cookiePermissions).reduce((o, k) => ({ ...o, [k]: userSettings[k] ?? null }), {});
  }

  saveToLocalSettings() {
    setCookie(this.id, this.cookiePermissions)
  }

  clearLocalSettings() {
    deleteCookie(this.id);
  }

  showModal() {
    if (document.body.contains(this.#widget)) {
      return;
    }

    document.body.insertBefore(this.#widget, document.body.firstChild);
  }

  hideModal() {
    if (!document.body.contains(this.#widget)) {
      return;
    }

    document.body.removeChild(this.#widget);
  }

  savePermissions(x) {
    this.cookiePermissions = x;
    this.saveToLocalSettings();
    this.hideModal();
  }

  constructor(id) {
    this.#$$ = id ?? 'cookieControl';
    window.getCookie ||= k => JSON.parse(Object.fromEntries(new URLSearchParams(document.cookie.replace(/; ?/g, '&')))?.[k] || null);
    window.setCookie ||= (k, v, e) => document.cookie = `${k}=${JSON.stringify(v)}; path=/; max-age=${e || 2592000}; samesite=strict`;
    window.deleteCookie ||= (k, v, e) => document.cookie = `${k}=; path=/; max-age=-1; samesite=strict`;

    this.cookiePermissions = this.localSettings;
    this.#widget = new CookieControlWidget(this).widget;
    if (!getCookie(this.id)) {
      this.showModal();
    }

    window['$' + this.id] = this;
  }
}

class CookieControlWidget {
  #host;
  #values;
  #widgetWrapper;

  get widget() {
    return this.#widgetWrapper;
  }

  constructor(host) {
    this.#host = host;
    this.#widgetWrapper = this.#wrapper([
      this.#wrapper([
        this.#header('Cookies'),
        this.#text('This site stores certain information as "cookies" on your device in order to improve your website experience. We also share information about your use of the site with analytics partners who may combine it with other information that you\'ve provided to them or that they\'ve collected from your use of their services.'),
        this.#fieldGroup('Settings', [
          this.#toggle('Customise cookies', [
            this.#fieldGroup('Cookie types', [
              this.#checkbox('Essential', 'These cookies are deemed essential to the proper functioning of this website and by continuing to use this site you accept that they may be stored on your computer to help us deliver services. No personal information is contained in these cookies and they are used for the sole purpose of enabling processes used to provide content functions on this site.', { disabled: true, checked: true }),
              this.#checkbox('Personalisation', 'These cookies are deemed essential to the proper functioning of this website and by continuing to use this site you accept that they may be stored on your computer to help us deliver services. No personal information is contained in these cookies and they are used for the sole purpose of enabling processes used to provide content functions on this site.'),
              this.#checkbox('Analytics', 'These cookies are deemed essential to the proper functioning of this website and by continuing to use this site you accept that they may be stored on your computer to help us deliver services. No personal information is contained in these cookies and they are used for the sole purpose of enabling processes used to provide content functions on this site.'),
            ]),
            this.#button('Save cookie preferences', () => this.#host.savePermissions(this.values), { className: 'accept', id: this.#slug(this.#host.id + '-save') }),
          ]),
          this.#button('Essential cookies only', () => this.#host.savePermissions(Object.keys(this.values).reduce((o, x) => { o[x] = false; return o }, {})), { className: 'reject', id: this.#slug(this.#host.id + '-reject') }),
          this.#button('Accept all cookies', () => this.#host.savePermissions(Object.keys(this.values).reduce((o, x) => { o[x] = true; return o }, {})), { className: 'accept', id: this.#slug(this.#host.id + '-accept') }),
        ], { className: 'settings' }, false)
      ], { className: 'max-width' }),
    ], { id: this.#host.id, className: 'cookie-control-widget' });
  }

  get values() {
    return Array.from(this.#widgetWrapper.querySelectorAll('input[type="checkbox"]')).reduce((o, x) => ({ ...o, [x.name]: !!x.checked }), {})
  }

  #slug(x) {
    return x.match(/[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+/g).map(x => x.toLowerCase()).join('-');
  }

  #wrapper(children, opts) {
    let el = this.#createElement('div', opts);

    if (children?.constructor === Array) {
      children.forEach(x => el.appendChild(x));
    }

    return el;
  }

  #header(value, depth, opts) {
    return this.#createElement('h' + Math.min(Math.max(depth ?? 0, 2), 6), Object.assign({ innerText: value }, opts));
  }

  #text(value, opts) {
    return this.#createElement('p', Object.assign({ innerHTML: value }, opts));
  }

  #toggle(label, children, opts) {
    let el = this.#createElement('details', opts);
    el.appendChild(this.#createElement('summary', { innerText: label, className: 'button' }));
    el.appendChild(this.#wrapper(children, { className: 'inner' }));

    return el;
  }

  #fieldGroup(label, children, opts, showLegend) {
    let el = this.#createElement('fieldset', opts);
    el.appendChild(this.#createElement('legend', { innerText: label, className: showLegend ? '' : 'screen-reader-text' }));

    if (children?.constructor === Array) {
      children.forEach(x => el.appendChild(x));
    }

    return el;
  }

  #checkbox(label, desc, opts) {
    let id = this.#slug(this.#host.id + '-' + label);
    let el = this.#wrapper([
      this.#createElement('input', Object.assign({ type: 'checkbox', name: this.#slug(label), id: id, checked: this.#host.cookiePermissions[this.#slug(label)] ?? false }, opts)),
      this.#createElement('label', { innerText: label, htmlFor: id }),
    ], { className: 'form-element checkbox' });

    if (desc) {
      el.appendChild(this.#text(desc));
    }

    return el;
  }

  #button(label, fn, opts) {
    let el = this.#createElement('button', Object.assign({ innerText: label }, opts));
    if (typeof fn == 'function') {
      el.addEventListener('click', e => fn.apply(this, [e]));
    }

    return el;
  }

  #createElement(tag, attr) {
    let el = document.createElement(tag);
    Object.assign(el, attr ?? {});
    Object.entries(attr ?? {}).forEach(([k, v]) => k.startsWith('aria') ? el.setAttribute(k.replace(/[A-Z]/g, m => "-" + m.toLowerCase()), v) : null);

    return el;
  }
}

(() => new CookieControl)();
