angular.module("umbraco").controller("LocalGovUmbraco.Dictionary", function ($scope, editorState, contentResource) {
  $scope.existingKeys = [];
  $scope.dictionaryKeys = [];
  $scope.updateKeys = () => $scope.dictionaryKeys = $scope.existingKeys.filter(x => !$scope.data.map(x => x[0]?.toLowerCase()).filter(x => !!x).includes(x.toLowerCase()));

  if ($scope.model.config.autocomplete) {
    contentResource.getChildren(editorState.current.parentId).then(children => $scope.existingKeys = [...new Set(children.items?.filter(x => x.contentTypeAlias == editorState.current.contentTypeAlias).map(x => x.properties.filter(x => x.editor == 'LocalGovUmbraco.Dictionary' && x.alias == $scope.model.alias).map(x => Object.keys(x.value))).flat(2))].sort((a, b) => a.localeCompare(b, navigator.languages[0] || navigator.language, { numeric: true, ignorePunctuation: true })) ?? []);
    $scope.$watch('existingKeys', () => $scope.updateKeys());
  }

  $scope.data = [];
  $scope.delete = i => $scope.data.splice(i, 1);
  $scope.move = (i, n) => $scope.data.splice(Math.max(i + n, 0), 0, $scope.delete(i)[0]);
  $scope.hasEmpty = () => $scope.data.some(x => !(x[0]?.length ?? false));

  $scope.$on('formSubmitting', () => $scope.model.value = Object.fromEntries($scope.data));
  $scope.$watch('model.value', () => $scope.data = Object.entries($scope.model.value ?? []));
});
