using Examine;
using Examine.Search;
using Microsoft.Extensions.Primitives;
using System.Text.RegularExpressions;
using Umbraco.Cms.Infrastructure.Examine;

namespace LocalGovUmbraco.Extensions
{
  /// <summary>
  /// Common search helper functions.
  /// </summary>
  public static partial class SearchExtensions
  {
    /// <summary>
    /// Compiled regex to extract individual words, or multiple quoted words.
    /// </summary>
    /// 
    /// <returns>The compiled <see cref="Regex"/></returns>
    [GeneratedRegex("\\w+|\"[\\w\\s]*\"")]
    private static partial Regex Parameters();

    /// <summary>
    /// Converts an <see cref="IEnumerable{string?}"/> to an array of prepared <see cref="ExamineValue"/>s.
    /// </summary>
    /// 
    /// <param name="strings">The strings to prepare.</param>
    ///
    /// <returns>An array of prepared <see cref="ExamineValue"/>s</returns>
    public static IExamineValue[] ToExamineValues(this IEnumerable<string?> strings) => strings.WhereNotNull().SelectMany(x => Parameters().Matches(x.PlainText()).Select(x => new ExamineValue(x.Value.StartsWith("\"") ? Examineness.Explicit : Examineness.ComplexWildcard, x.Value)).Cast<IExamineValue>()).ToArray();

    /// <summary>
    /// Compiled regex to match non alphanumeric characters.
    /// </summary>
    /// 
    /// <returns>The compiled <see cref="Regex"/></returns>
    [GeneratedRegex("[^a-z0-9]+")]
    private static partial Regex NonAlphaNum();

    /// <summary>
    /// Extracts an array of prepared <see cref="ExamineValue"/>s from a given <see cref="PathString"/>.
    /// </summary>
    /// 
    /// <param name="path">The path to prepare.</param>
    ///
    /// <returns>An array of prepared <see cref="ExamineValue"/>s</returns>
    public static IExamineValue[] ToExamineValues(this PathString path) => NonAlphaNum().Split(path).ToExamineValues();

    /// <summary>
    /// Attempts to extract the search paramaters from the request query.
    /// </summary>
    /// 
    /// <param name="request">The <see cref="IQueryCollection"/> object.</param>
    /// <param name="key">The query parameter to use.</param>
    /// 
    /// <returns>An array of <see cref="IExamineValue">IExamineValues</see>.</returns>
    public static IExamineValue[] GetSearchRequest(this IQueryCollection query, string key) => (query.TryGetValue(key, out StringValues values) ? values : new()).ToExamineValues();

    /// <summary>
    /// Attempts to extract the search paramaters from the request query.
    /// </summary>
    /// 
    /// <param name="request">The <see cref="IQueryCollection"/> object.</param>
    /// <param name="key">The query parameter to use.</param>
    /// 
    /// <returns>An array of <see cref="IExamineValue">IExamineValues</see>.</returns>
    public static IExamineValue[] GetSearchRequest(this IFormCollection form, string key) => (form.TryGetValue(key, out StringValues values) ? values : new()).ToExamineValues();

    /// <summary>
    /// Converts an array of <see cref="IExamineValue"/> to a <see langword="string"/>, joined by <paramref name="glue"/>.
    /// </summary>
    /// 
    /// <param name="array">An array of items.</param>
    /// <param name="glue">The glue to use.</param>
    /// 
    /// <returns>A string of concatenated items, joined by the <paramref name="glue"/>.</returns>
    public static string ToString(this IExamineValue[] values, string glue) => string.Join(glue, values.Select(x => x.Value));

    /// <summary>
    /// Exclude the given node types from a <see cref="IBooleanOperation"/> query.
    /// </summary>
    /// 
    /// <param name="query">The <see cref="IBooleanOperation"/> to exclude from.</param>
    /// <param name="types">The content types to exclude.</param>
    /// 
    /// <returns>The modified query</returns>
    private static IBooleanOperation ExcludeTypes(this IBooleanOperation query, string[]? types)
    {
      types?.ToList().ForEach(type => query.Not().NodeTypeAlias(type));

      return query;
    }

    /// <summary>
    /// Fetches content from the <see cref="IIndex">ExternalIndex</see> for the given paremeters and fields.
    /// </summary>
    /// 
    /// <param name="examineManager">An instance of <see cref="IExamineManager"/>.</param>
    /// <param name="searchParameters">The pre-formatted search parameters.</param>
    /// <param name="searchFields">A list of indexed fields to search.</param>
    /// <param name="excludedTypes">An optional list of content types to exclude from the results.</param>
    /// <param name="indexType">The type of indexed content to return.</param>
    /// 
    /// <returns>An <see cref="ISearchResults"/> objects containing the matching <see cref="ISearchResult">ISearchResults</see></returns>
    public static ISearchResults GetSearchResults(this IExamineManager examineManager, IExamineValue[] searchParameters, string[] searchFields, string[]? excludedTypes = null, string indexType = IndexTypes.Content) =>
      examineManager.TryGetIndex("ExternalIndex", out IIndex index)
      ? index.Searcher.CreateQuery(indexType, BooleanOperation.And).GroupedOr(searchFields, searchParameters).Not().Field("umbracoSearchHide", "1").ExcludeTypes(excludedTypes).Execute()
      : EmptySearchResults.Instance;
  }
}
