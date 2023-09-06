(() => {
  'use strict';
  const DEBUG = false;

  const $ = (tag, properties) => Object.assign(document.createElement(tag), properties ?? {});
  const isArray = x => x?.constructor?.name == 'Array';

  class Calendar {
    host;
    months = [];
    toolbar;
    index;
    #events = {};

    constructor(element) {
      this.host = element;
      this.months = this.host.querySelectorAll('.calendar-month');

      if (this.months.length > 1) {
        this.addToolbar();
      }
      this.today();
      this.host.$Calendar = this;
      this.host.classList.add('js');
      this.#bindHover(this.host.querySelectorAll('.calendar-event'));
    }

    addToolbar() {
      this.toolbar = new CalendarToolbar(this, [
        ['Previous', this.previous, { className: 'previous' }],
        ['Today', this.today, { className: 'today' }],
        ['Next', this.next, { className: 'next' }]
      ]);
    }

    show(i) {
      if (i == this.index) {
        return;
      }

      this.index = i;
      this.host.dataset.index = this.index;
      this.months.forEach((month, n) => month.toggleAttribute('hidden', n != i));
      this.toolbar?.buttons.forEach(x => x.toggleAttribute('disabled', (this.index == 0 && x.classList.contains('previous')) || ((this.index == this.months.length - 1) && x.classList.contains('next'))));
    }

    previous() {
      if (this.index == 0) {
        return;
      }

      this.show(this.index - 1);
    }

    next() {
      if (this.index == this.months.length - 1) {
        return;
      }

      this.show(this.index + 1);
    }

    today() {
      let month = this.host.querySelector('[data-month="' + ((new Date).getMonth() + 1) + '"][data-year="' + (new Date).getFullYear() + '"]');
      if (!month) {
        console.error('Failed to get calendar grid for current month');
        return;
      }

      this.show(Array.from(this.months).indexOf(month) ?? 0);
    }

    getByDataId(id) {
      return this.host.querySelectorAll('[data-id="' + id + '"]');
    }

    bindEvent(element, event, fn) {
      if (typeof fn != "function") {
        console.error('Third paramenter must be callable function.', x);
        return;
      }
      if (!(event in this.#events)) {
        this.#events[event] = [];
      }

      this.#events[event].push([element, fn]);
      element.addEventListener(event, fn);
    }

    unbindEvent(element, event, fn) {
      (event ? [event] : Object.keys(this.#events)).forEach(e => {
        this.#events[e].forEach((x, i) => {
          if (x[0] == element && (!fn || fn == x[1])) {
            element.removeEventListener(e, x[1]);
            delete this.#events[e][i];
          }
        });
      });
    }

    #bindHover(elements) {
      elements.forEach(x => {
        this.bindEvent(x, 'focusin', e => this.onFocus.call(this, e));
        this.bindEvent(x, 'focusout', e => this.onFocusLost.call(this, e));
        this.bindEvent(x, 'mouseover', e => this.onFocus.call(this, e));
        this.bindEvent(x, 'mouseout', e => this.onFocusLost.call(this, e));
      });
    }

    onFocus(e) {
      let id = e.target.closest('[data-id]')?.dataset.id;
      if (!id) {
        console.error('Unable to determine ID', e.target);
        return;
      }

      this.getByDataId(id).forEach(x => x.classList.add('has-focus'));
    }

    onFocusLost(e) {
      let id = e.target.closest('[data-id]')?.dataset.id;
      if (!id) {
        console.error('Unable to determine ID', e.target);
        return;
      }

      this.getByDataId(id).forEach(x => x.classList.remove('has-focus'));
    }

    static destroy(e) {
      if (!(e.$Calendar instanceof Calendar)) {
        return;
      }

      e.$Calendar.toolbar?.remove();
      e.$Calendar.months.forEach(x => x.removeAttribute('hidden'));
      Array.from(e.$Calendar.host.querySelectorAll('.calendar-event')).map(x => e.$Calendar.unbindEvent(x));
      delete e.$Calendar;
    }

    static $(e) {
      if (e.$Calendar instanceof Calendar) {
        return;
      }

      e.$Calendar = new Calendar(e);
    }
  }

  class CalendarToolbar {
    #calendar;
    #toolbar;
    buttons = [];

    constructor(host, buttons) {
      this.#calendar = host;
      this.#toolbar = $('nav', { className: 'toolbar' });

      (isArray(buttons) ? buttons : []).forEach(x => {
        if (!isArray(x) || x.length < 2) {
          console.error('Invalid button configuration', x);
          return;
        }

        if (typeof x[1] != "function") {
          console.error('Second paramenter must be callable function.', x);
          return;
        }

        this.addButton(x[0], x[1], x[2]);
      });

      this.#calendar.host.appendChild(this.#toolbar);
    }

    addButton(label, fn, attr) {
      let button = $('button', Object.assign({ innerHTML: label }, attr ?? {}));
      button.addEventListener('click', () => fn.call(this.#calendar));
      this.buttons.push(button);
      this.#toolbar.appendChild(button);

      return this;
    }

    remove() {
      this.#toolbar.remove();
    }
  }

  document.querySelectorAll('div.calendar').forEach(c => new ResizeObserver(e => window.getComputedStyle(e[0].target).display == 'grid' ? Calendar.$(c) : Calendar.destroy(c)).observe(c.querySelector('.calendar-grid')));
})();
