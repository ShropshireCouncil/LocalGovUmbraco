class DateImmutable extends Date { }

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.setTime = function (number) {
  let date = new Date(this);
  date.setTime(number);

  return new DateImmutable(date);
}

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.setMilliseconds = function (number) {
  let date = new Date(this);
  date.setMilliseconds(number);

  return new DateImmutable(date);
}

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.setUTCMilliseconds = function (number) {
  let date = new Date(this);
  date.setUTCMilliseconds(number);

  return new DateImmutable(date);
}

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.setSeconds = function (number) {
  let date = new Date(this);
  date.setSeconds(number);

  return new DateImmutable(date);
}

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.setExactSeconds = function (number) {
  return this.setSeconds(number).setMilliseconds(0);
}

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.setUTCSeconds = function (number) {
  let date = new Date(this);
  date.setUTCSeconds(number);

  return new DateImmutable(date);
}

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.setExactUTCSeconds = function (number) {
  return this.setUTCSeconds(number).setUTCMilliseconds(0);
}

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.setMinutes = function (number) {
  let date = new Date(this);
  date.setMinutes(number);

  return new DateImmutable(date);
}

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.setExactMinutes = function (number) {
  return this.setMinutes(number).setExactSeconds(0);
}

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.setUTCMinutes = function (number) {
  let date = new Date(this);
  date.setUTCMinutes(number);

  return new DateImmutable(date);
}

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.setExactUTCMinutes = function (number) {
  return this.setUTCMinutes(number).setExactUTCSeconds(0);
}

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.setHours = function (number) {
  let date = new Date(this);
  date.setHours(number);

  return new DateImmutable(date);
}

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.setExactHours = function (number) {
  return this.setHours(number).setExactMinutes(0);
}

/** @returns {DateImmutable} */
DateImmutable.prototype.setUTCHours = function (number) {
  let date = new Date(this);
  date.setUTCHours(number);

  return new DateImmutable(date);
}

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.setExactUTCHours = function (number) {
  return this.setUTCHours(number).setExactUTCMinutes(0);
}

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.setDate = function (number) {
  let date = new Date(this);
  date.setDate(number);

  return new DateImmutable(date);
}

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.setUTCDate = function (number) {
  let date = new Date(this);
  date.setUTCDate(number);

  return new DateImmutable(date);
}

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.setMonth = function (number) {
  let date = new Date(this);
  date.setMonth(number);

  return new DateImmutable(date);
}

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.setUTCMonth = function (number) {
  let date = new Date(this);
  date.setUTCMonth(number);

  return new DateImmutable(date);
}

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.setFullYear = function (number) {
  let date = new Date(this);
  date.setFullYear(number);

  return new DateImmutable(date);
}

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.setUTCFullYear = function (number) {
  let date = new Date(this);
  date.setUTCFullYear(number);

  return new DateImmutable(date);
}

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.addMilliseconds = function (number) {
  return this.setMilliseconds(this.getMilliseconds() + number);
};

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.addSeconds = function (number) {
  return this.setSeconds(this.setSeconds() + number);
};

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.addMinutes = function (number) {
  return this.setMinutes(this.getMinutes() + number);
};

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.addHours = function (number) {
  return this.setHours(this.getHours() + number);
};

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.addDays = function (number) {
  return this.setDate(this.getDate() + number);
}

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.addMonths = function (number) {
  return this.setMonth(this.getMonth() + number);
}

/** @param {number} number
 *  @returns {DateImmutable} */
DateImmutable.prototype.addYears = function (number) {
  return this.setFullYear(this.getFullYear() + number);
}
