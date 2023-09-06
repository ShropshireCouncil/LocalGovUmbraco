'use strict';

class EventInstance {
  /** @type {?DateImmutable} */
  #lastStartDate;

  /** @type {?DateImmutable} */
  #startDate;

  /** @type {?DateImmutable} */
  #lastEndDate;

  /** @type {?DateImmutable} */
  #endDate;

  /** @returns {?DateImmutable} */
  get startDate() {
    return this.#startDate;
  }

  /** @param {DateImmutable|Date|string} value */
  set startDate(value) {
    this.#lastStartDate = this.#startDate;
    this.#startDate = this.#dateValue(value);
  }

  /** @returns {?DateImmutable} */
  get endDate() {
    return this.#endDate;
  }

  /** @param {DateImmutable|Date|string} value */
  set endDate(value) {
    this.#lastEndDate = this.#endDate;
    this.#endDate = this.#dateValue(value);
  }

  /** @returns {int} */
  get startTime() {
    return (this.startDate.getHours() * 3600000) + (this.startDate.getMinutes() * 60000) + (this.startDate.getSeconds() * 1000) + this.startDate.getMilliseconds();
  }

  /** @returns {int} */
  get endTime() {
    return (this.endDate.getHours() * 3600000) + (this.endDate.getMinutes() * 60000) + (this.endDate.getSeconds() * 1000) + this.endDate.getMilliseconds();
  }

  /** @param {boolean} value */
  set allDay(value) {
    if (value) {
      this.startDate = this.startDate.setExactHours(0);
      this.endDate = this.endDate.setHours(23).setMinutes(59).setExactSeconds(59);
    }
    else {
      this.startDate = this.startDate.setExactHours(this.startDate.toDateString() == (new DateImmutable).toDateString() ? (new DateImmutable).getHours() + 1 : 9);
      this.endDate = this.endDate.setExactHours(this.startDate.toDateString() != this.endDate.toDateString() ? this.startDate.getHours() : this.startDate.toDateString() == (new DateImmutable).toDateString() ? (new DateImmutable).getHours() + 2 : 10);
    }
  }

  /** @returns {boolean} */
  get allDay() {
    return this.startDate.getHours() == 0 && this.startDate.getMinutes() == 0 && this.endDate.getHours() == 23 && this.#endDate.getMinutes() == 59;
  }

  /** @returns {boolean} */
  get multiDay() {
    return (this.endDate - this.startDate) / 86400000 > 1
  }

  /** @returns {boolean} */
  get historical() {
    return this.endDate < new Date;
  }

  /** @param {DateImmutable|Date|string} start
   *  @param {DateImmutable|Date|string} end */
  constructor(start, end) {
    [this.startDate, this.endDate] = [start ?? (new DateImmutable).addHours(1).setExactMinutes(0), end ?? (start?.addHours(1) ?? (new DateImmutable).addHours(2)).setExactMinutes(0)].sort();
  }

  /** @param {boolean} moveEnd 
   *  @returns {void} */
  balanceDates(moveEnd) {
    if (!(this.startDate?.getTime() ?? 0)) {
      this.startDate = (new DateImmutable).setExactHours(0);
      this.endDate = (new DateImmutable).setHours(23).setMinutes(59).setExactSeconds(59);
      return;
    }

    if (!(this.endDate?.getTime() ?? 0)) {
      this.endDate = this.startTime ? this.startDate.addHours(1) : this.startDate.setHours(23).setMinutes(59).setExactSeconds(59);
      return;
    }

    if (moveEnd) {
      let offset = this.endDate.getTime() - this.#lastStartDate.getTime();
      this.endDate = this.startDate.addMilliseconds(offset);
    }
  }

  /** @returns {?DateImmutable} */
  #dateValue(datetime) {
    try {
      switch (typeof datetime) {
        case 'string':
          let ymd = datetime.match(/(\d{4})-(\d{2})-(\d{2})(?:[\sT](\d{2}):(\d{2})(?::(\d{2}))?(Z|[+-]\d{2}:\d{2})?)?/);
          if (ymd) {
            return new DateImmutable(ymd[1] + '-' + ymd[2] + '-' + ymd[3] + 'T' + (ymd[4] || 0) + ':' + (ymd[5] || 0) + ':' + (ymd[6] || 0) + (ymd[7] || 'Z'));
          }

        default:
          return new DateImmutable(datetime);
      }
    }
    catch (ex) { }

    return null;
  }

  /** @returns {Date[]} */
  toJSON() {
    return [this.startDate, this.endDate];
  }
}
