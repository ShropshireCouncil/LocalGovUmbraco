angular.module('umbraco').filter('pluralise', function () {
  const plurals = [
    [/^(child)$/i, '$1ren'],
    [/^(m)an/i, '$1en'],
    [/^(pe)rson$/i, '$1ople'],
    [/(quiz)$/i, '$1zes'],
    [/^(ox)$/i, '$1en'],
    [/([m|l])ouse$/i, '$1ice'],
    [/(matr|vert|ind)ix|ex$/i, '$1ices'],
    [/(x|ch|ss|sh)$/i, '$1es'],
    [/([^aeiouy]|qu)y$/i, '$1ies'],
    [/([^aeiouy]|qu)ies$/i, '$1y'],
    [/(hive)$/i, '$1s'],
    [/(?:([^f])fe|([lr])f)$/i, '$1$2ves'],
    [/([ti])um$/i, '$1a'],
    [/(buffal|tomat)o$/i, '$1oes'],
    [/(bu)s$/i, '$1ses'],
    [/(alias|status)$/i, '$1es'],
    [/(octop|vir)us$/i, '$1i'],
    [/(ax|test)is$/i, '$1es'],
    [/s$/i, 's'],
    [/$/, 's'],
  ];

  const uncountable = ['sheep','fish','series','species','money','rice','information','equipment'];

  return (string, value) => {
    if ((value ?? 1) != 1 && uncountable.indexOf(string.toLowerCase())) {
      for (let i = 0; i < plurals.length; i++) {
        if (RegExp(plurals[i][0]).test(string)) {
          return string.replace(RegExp(plurals[i][0]), plurals[i][1]);
        }
      }
    }

    return string;
  }
});
