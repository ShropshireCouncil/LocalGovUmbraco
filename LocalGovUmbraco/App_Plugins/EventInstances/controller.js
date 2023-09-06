angular.module('umbraco').controller('LocalGovUmbraco.PropertyEditors.EventInstances', function ($scope, $filter, $timeout, overlayService) {
  'use strict';

  const PluginPath = $scope.model.view.substring(0, $scope.model.view.lastIndexOf('/'));

  $scope.event = new EventInstances;
  $scope.$watch('$scope.model.value', () => $scope.event = new EventInstances($scope.model.value));

  $scope.showExpired = false;
  $scope.toggleExpired = () => $timeout(() => $scope.showExpired = !$scope.showExpired); // $timeout hack needed to work around weird AngularJS watch triggers.

  /** @param {EventInstance} instance */
  $scope.duration = instance => {
    if (!instance.startDate.getTime() || !instance.endDate.getTime()) {
      return null;
    }

    let duration = instance.endDate.getTime() - instance.startDate.getTime();

    if (instance.allDay || instance.multiDay) {
      duration = Math.round(duration / 86400000);

      if (!instance.allDay && duration % 86400000) {
        duration = '~' + duration;
      }

      return duration + ' day' + (duration > 1 ? 's' : '');
    }

    duration /= 60000;
    if (duration >= 60) {
      duration = Math.round((duration / 60) * 100) / 100;

      return duration + ' hour' + (duration > 1 ? 's' : '');
    }

    duration = Math.round(duration);

    return duration + ' minute' + (duration > 1 ? 's' : '');
  };

  /** @param {Date|DateImmutable} date
   *  @returns {int} */
  $scope.daysInMonth = date => new DateImmutable(date.getFullYear(), date.getMonth() + 1, 0).getDate();
  $scope.DaysOfWeek = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];
  $scope.EventIntervals = ['Day', 'Week', 'Month', 'Year'];

  /** @param {EventInstance} instance */
  $scope.repeatInstance = instance => {
    let repeatInstances;

    overlayService.open({
      title: 'Repeat Event Instance',
      view: PluginPath + '/repeat.html',
      disableBackdropClick: false,
      submitButtonLabel: 'Add',
      sumbitButtonStyle: 'success',
      closeButtonLabel: 'Cancel',
      dateInput: false,
      instance: instance,
      repeatInstances: [],
      minDate: instance.endDate.addDays(1),
      dateValue: new DateImmutable(instance.endDate.addDays(1)),
      monthOption: 'date',
      value: {
        increment: 1,
        interval: 'day',
        days: Array($scope.DaysOfWeek.length).fill(false),
        index: 0,
        count: 1,
      },
      /** @param {object} model
       *  @returns {int} */
      calculateRepeats: model => {
        /** @type {?EventInstance} */
        let nextInstance = model.instance;
        repeatInstances = [];

        if (model.dateInput) {
          while (nextInstance && new Date(nextInstance.startDate.getFullYear(), nextInstance.startDate.getMonth(), nextInstance.startDate.getDate()) <= model.dateValue)
          {
            repeatInstances.push(nextInstance);
            nextInstance = model.getNextInstance(nextInstance, model.value, model.monthOption);
          }
          model.value.count = repeatInstances.length;
        }
        else {
          for (let i = 0; i < model.value.count; ++i) {
            if (nextInstance = model.getNextInstance(nextInstance, model.value, model.monthOption)) {
              repeatInstances.push(nextInstance);
            }
          }
          model.dateValue = new DateImmutable(repeatInstances[repeatInstances.length - 1]?.startDate);
        }
      },
      /** @param {EventInstance} instance
       *  @param {object} repeat
       *  @param {?string} monthOption
       *  @returns {EventInstance} */
      getNextInstance: (instance, repeat, monthOption) => {
        switch (repeat.interval) {
          case 'day':
            return new EventInstance(instance.startDate.addDays(repeat.increment), instance.endDate.addDays(repeat.increment));

          case 'week':
            let dayOffsets = Object.keys(repeat.days).filter(n => repeat.days[n]).map(n => (n - $filter('normalizeDay')(instance.startDate.getDay()) + repeat.days.length * repeat.increment) % (repeat.days.length * repeat.increment) || (repeat.days.length * repeat.increment));
            if (!dayOffsets.length) break;

            let increment = Math.min(...dayOffsets);
            return new EventInstance(instance.startDate.addDays(increment), instance.endDate.addDays(increment));

          case 'month':
            repeat.days = Array($scope.DaysOfWeek.length).fill(false);
            switch (monthOption) {
              case 'date':
                repeat.index = instance.startDate.getDate();
                break;

              case 'nthDay':
                repeat.index = Math.ceil(instance.startDate.getDate() / 7);
                repeat.days[$filter('normalizeDay')(instance.startDate.getDay())] = true;
                break;

              case 'lastDay':
                repeat.index = -1;
                repeat.days[$filter('normalizeDay')(instance.startDate.getDay())] = true;
                break;

              case 'last':
                repeat.index = -1;
                break;
            }

            let startDate = instance.startDate.setDate(1).addMonths(repeat.increment);
            let days = Object.keys(repeat.days).filter(n => repeat.days[n]).map(Number);
            if (days.length) {
              let nthDay = new Date(startDate.getFullYear(), startDate.getMonth(), repeat.index < 0 ? $scope.daysInMonth(startDate) + (7 * repeat.index) + 1 : (7 * repeat.index) - 6);
              while (days.indexOf($filter('normalizeDay')(nthDay.getDay())) == -1) {
                nthDay.setDate(nthDay.getDate() + 1);
                if (nthDay.getDate() == 1) {
                  nthDay.setDate(repeat.index < 0 ? $scope.daysInMonth(nthDay) + (7 * repeat.index) + 1 : (7 * repeat.index) - 6);
                }
              }
              let offset = Math.round((nthDay - startDate) / 86400000);
              startDate = startDate.addDays(offset);
            }
            else {
              let day = repeat.index < 0 ? $scope.daysInMonth(startDate) : Math.min(repeat.index, 31);
              while ($scope.daysInMonth(startDate) < day) {
                startDate = startDate.addMonths(1);
              }

              startDate = startDate.setDate(day);
            }

            let diff = Math.round((startDate - instance.startDate) / 86400000);
            return new EventInstance(startDate, instance.endDate.addDays(diff));

          case 'year':
            return new EventInstance(instance.startDate.addYears(repeat.increment), instance.endDate.addYears(repeat.increment));
        }

        return null;
      },
      submit: () => {
        repeatInstances.forEach(x => $scope.event.addInstance(x));
        $scope.event.sort();
        overlayService.close();
      },
      close: () => overlayService.close(),
    });
  }

  $scope.$on('formSubmitting', () => $scope.model.value = $scope.event.toJSON());
}).filter('nextEventDate', function () {
  return dates => new EventInstances(dates).next();
}).filter('abs', function () {
  return (n) => Math.abs(parseInt(n));
}).filter('ordinal', function ($filter) {
  return (n) => isNaN(n) ? n : ["th", "st", "nd", "rd"][Math.min($filter('abs')(n) % ($filter('abs')(n) < 30 ? 20 : 30), 4) % 4];
}).filter('withOrdinal', function ($filter) {
  return (n) => isNaN(n) ? n : n.toString() + $filter('ordinal')(n);
}).filter('normalizeDay', function () {
  return i => (i % 7 + 6) % 7;
}).filter('nthDay', function () {
  return date => Math.ceil(date.getDate() / 7);
}).filter('isLastOfMonth', function () {
  return date => date.getDate() == (new Date(date.getUTCFullYear(), date.getMonth() + 1, 0)).getDate();
}).filter('isLastDayOfMonth', function () {
  return date => (new Date(date.getUTCFullYear(), date.getMonth() + 1, 0)).getDate() - date.getDate() < 7;
});
