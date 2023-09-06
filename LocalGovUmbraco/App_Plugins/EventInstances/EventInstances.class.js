'use strict';

class EventInstances {
  /** @type {EventInstance[]} */
  #instances = [];

  /** @returns {EventInstance[]} */
  get instances() {
    return this.#instances;
  }

  /** @returns {EventInstance[]} */
  get historical() {
    return this.#instances.filter(x => x.historical);
  }

  /** @returns {void} */
  removeInvalid() {
    this.#instances = this.instances.filter(x => x.startDate && x.endDate);
  }

  /** @returns {void} */
  removeDuplicates() {
    this.#instances = [...new Set(this.#instances.map(JSON.stringify))].map(x => new EventInstance(...JSON.parse(x).map(Date.parse)));
  }

  /** @returns {void} */
  sort() {
    this.removeDuplicates();
    this.#instances.sort((a, b) => a.startDate - b.startDate || a.endDate - b.endDate);
  }

  /** @param {EventInstance|(Date|string)[]} dates */
  addInstance(dates) {
    let instance = dates instanceof EventInstance ? dates : new EventInstance(dates?.shift(), dates?.shift());
    this.#instances.push(instance);
  }

  /** @param {number} i */
  removeInstance(i) {
    this.#instances.splice(i, 1);
  }

  /** @param {(Date[]|string[])[]|string} x */
  constructor(x) {
    switch (true) {
      case x instanceof Array: this.#fromArray(x); break;
      case typeof x == 'string': this.#fromJson(x); break;
    }

    this.removeInvalid();
    this.sort();
    return;
  }

  next() {
    return this.instances.filter(x => x.startDate >= DateImmutable.now()).shift();
  }

  /** @returns {Date[][]} */
  toJSON() {
    this.removeInvalid();
    this.sort();

    return this.instances.map(x => x.toJSON());
  }

  /** @param {string} array */
  #fromJson(string) {
    try {
      return this.#fromArray(EventInstances.$deserialise(string).toJSON());
    }
    catch (e) { }
  }

  /** @param {(Date[]|string[])[]} array */
  #fromArray(array) {
    array.forEach(x => this.addInstance(x));
  }

  /** @param {string} string */
  static $deserialise(string) {
    string = string.trim();
    try {
      var x = JSON.parse(string);

      return new this(x);
    }
    catch (e) {
      throw new SyntaxError('Invalid JSON string: ' + e.message);
    }
  }
}
