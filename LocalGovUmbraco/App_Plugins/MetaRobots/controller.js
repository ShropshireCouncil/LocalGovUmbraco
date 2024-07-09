angular.module("umbraco").controller("LocalGovUmbraco.PropertyEditors.MetaRobots", function ($scope) {
  "use strict";

  $scope.data = $scope.model.value?.split(',').map(x => x.trim()).filter(x => x.length) ?? [];

  const dateCoerce = x => !x instanceof Date ? !isNaN(Date.parse(x)) ? new Date(x) : null : x;
  const toggle = (a, v, x) => {
    let i = a.indexOf(v);
    if (x && i < 0) {
      a.push(v);
    } else if (!x && i >= 0) {
      a.splice(i, 1);
    }
  }

  $scope.index = $scope.data.indexOf('noindex') < 0;
  $scope.follow = $scope.data.indexOf('nofollow') < 0;
  $scope.image = $scope.data.indexOf('noimageindex') < 0;
  $scope.snippet = $scope.data.indexOf('nosnippet') < 0;
  $scope.archive = $scope.data.indexOf('noarchive') < 0;
  $scope.translate = $scope.data.indexOf('notranslate') < 0;
  $scope.notafter = dateCoerce($scope.data.filter(x => x.startsWith('unavailable_after'))[0]?.split(':')[1]?.trim())
  $scope.custom = null;

  $scope.advancedMode = !($scope.image && $scope.snippet && $scope.archive && $scope.translate && !$scope.notafter && !$scope.custom);

  $scope.toggleAdvanced = () => $scope.advancedMode = !$scope.advancedMode;
  $scope.cleanCustom = () => $scope.custom = [...new Set($scope.custom?.toLowerCase().split(',').map(x => x.trim()).filter(x => x.length) ?? [])].join(', ');
  $scope.rebuildMeta = () => {
    let std = ['index', 'noindex', 'follow', 'nofollow', 'noimageindex', 'nosnippet', 'noarchive', 'notranslate'];
    let custom = $scope.custom?.toLowerCase().split(',').map(x => x.trim()) ?? [];
    let overrides = custom.filter(x => std.includes(x));
    if (overrides.length) {
      if (overrides.includes('index')) {
        $scope.index = true;
      }
      if (overrides.includes('noindex')) {
        $scope.index = false;
      }
      if (overrides.includes('follow')) {
        $scope.follow = true;
      }
      if (overrides.includes('nofollow')) {
        $scope.follow = false;
      }
      if (overrides.includes('noimageindex')) {
        $scope.image = false;
      }
      if (overrides.includes('nosnippet')) {
        $scope.snippet = false;
      }
      if (overrides.includes('noarchive')) {
        $scope.translate = false;
      }

      overrides.forEach(x => $scope.custom = $scope.custom.replace(new RegExp(x + '[, ]*', 'i'), ''));
    }

    toggle($scope.data, 'noindex', !$scope.index);
    toggle($scope.data, 'nofollow', !$scope.follow);
    toggle($scope.data, 'noimageindex', !$scope.image);
    toggle($scope.data, 'nosnippet', !$scope.snippet);
    toggle($scope.data, 'noarchive', !$scope.archive);
    toggle($scope.data, 'notranslate', !$scope.translate);

    $scope.data = $scope.data.filter(x => !x.startsWith('unavailable_after'));
    if ($scope.notafter instanceof Date) {
      $scope.data.push('unavailable_after: ' + $scope.notafter.toISOString())
    }

    $scope.data = [...new Set($scope.data.filter(x => std.filter(x => x.startsWith('no')).includes(x) || x.startsWith('unavailable_after')).concat($scope.custom?.toLowerCase().split(',').map(x => x.trim()).filter(x => x.length) ?? []))].sort();
    $scope.model.value = $scope.data.join(', ');
  }
});
