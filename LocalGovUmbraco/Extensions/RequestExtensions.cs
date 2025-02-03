using Microsoft.Extensions.Primitives;
using System.Collections.Specialized;
using System.Web;

namespace LocalGovUmbraco.Extensions
{
  /// <summary>
  /// Common HttpRequest helper functions.
  /// </summary>
  public static partial class RequestExtensions
  {
    /// <summary>
    /// Builds a <see cref="NameValueCollection"/> from the current <see cref="QueryString"/>.
    /// </summary>
    /// 
    /// <param name="request">The contextual <see cref="HttpRequest"/></param>
    /// 
    /// <returns>A <see cref="NameValueCollection"/> of the current <see cref="QueryString"/> values.</returns>
    public static NameValueCollection QueryCollection(this HttpRequest request) => HttpUtility.ParseQueryString(request.QueryString.ToString());

    /// <summary>
    /// Merges new <see cref="KeyValuePair"/> items into an existing <see cref="NameValueCollection"/>.
    /// </summary>
    /// 
    /// <param name="query">The <see cref="NameValueCollection"/> to update.</param>
    /// <param name="overrides">An <see cref="IEnumerable"/> of <see cref="KeyValuePair"/> items to add.</param>
    /// <param name="append">Wheter to append the new items to existing keys, or override them.</param>
    /// 
    /// <returns>The modified <see cref="NameValueCollection"/>.</returns>
    public static NameValueCollection Merge(this NameValueCollection query, IEnumerable<KeyValuePair<string, string?>> overrides, bool append = false)
    {
      foreach (KeyValuePair<string, string?> item in overrides)
      {
        if (append)
        {
          query.Add(item.Key, item.Value);
        }
        else
        {
          query.Set(item.Key, item.Value);
        }
      }

      return query;
    }

    /// <summary>
    /// Convert a <see cref="NameValueCollection"/> to a <see cref="Dictionary{string, string?}"/>.
    /// </summary>
    /// 
    /// <param name="collection">The <see cref="NameValueCollection"/> to convert.</param>
    /// 
    /// <returns>The converted <see cref="Dictionary{string, string?}"/></returns>
    public static Dictionary<string, string?> ToDictionary(this NameValueCollection collection)
    {
      Dictionary<string, string?> dict = [];
      foreach (KeyValuePair<string, string?> entry in collection)
      {
        dict.Add(entry.Key, entry.Value);
      }

      return dict;
    }

    /// <summary>
    /// Merges new <see cref="NameValueCollection"/> items into an existing <see cref="NameValueCollection"/>.
    /// </summary>
    /// 
    /// <param name="query">The <see cref="NameValueCollection"/> to update.</param>
    /// <param name="overrides">A <see cref="NameValueCollection"/> of items to add.</param>
    /// <param name="append">Wheter to append the new items to existing keys, or override them.</param>
    /// 
    /// <returns>The modified <see cref="NameValueCollection"/>.</returns>
    public static NameValueCollection Merge(this NameValueCollection query, NameValueCollection overrides, bool append = false) => query.Merge(overrides.ToDictionary(), append);

    /// <inheritdoc cref="NameValueCollection.Remove(string?)"/>
    public static NameValueCollection RemoveKey(this NameValueCollection query, string key)
    {
      query.Remove(key);

      return query;
    }

    /// <summary>
    /// Flattens a <see cref="NameValueCollection"/> to a URL encoded <see langword="string"/>.
    /// </summary>
    /// 
    /// <param name="query">The <see cref="NameValueCollection"/> to flatten.</param>
    /// 
    /// <returns>A <see langword="string"/> in the format "{key1}={value1}&{key2}={value2}.</returns>
    public static string ToQueryString(this NameValueCollection query) => string.Join("&", query.AllKeys.SelectMany(k => query.GetValues(k)?.Select(v => string.Format("{0}={1}", HttpUtility.UrlEncode(k), HttpUtility.UrlEncode(v))) ?? Enumerable.Empty<string>()).WhereNotNull());

    /// <summary>
    /// Creates a url encoded query string, for the given <see cref="HttpRequest"/>, with the given key appended.
    /// </summary>
    /// 
    /// <param name="request">The contextual <see cref="HttpRequest"/></param>
    /// <param name="key">The key to set</param>
    /// <param name="value">The value to set</param>
    /// 
    /// <returns>The current <see cref="QueryString"/> as a <see langword="string"/>, with the new key/value pair appended.</returns>
    public static string SetQueryValue(this HttpRequest request, string key, string? value) => request.QueryCollection().Merge(new Dictionary<string, string?>() { { key, value } }).ToQueryString();

    /// <summary>
    /// Merge a <see cref="NameValueCollection"/> of query parameters into an existing <see cref="Uri"/>.
    /// </summary>
    /// 
    /// <param name="uri">The <see cref="Uri"/> to update.</param>
    /// <param name="query">A <see cref="NameValueCollection"/> of items to update.</param>
    /// 
    /// <returns>The modified <see cref="Uri"/>.</returns>
    public static Uri MergeQuery(this Uri uri, NameValueCollection query) => uri.SetQuery(HttpUtility.ParseQueryString(uri.Query).Merge(query));

    /// <summary>
    /// Replaces the query parameters of a <see cref="Uri"/> with those in the provided <see cref="NameValueCollection"/>.
    /// </summary>
    /// 
    /// <param name="uri">The <see cref="Uri"/> to update.</param>
    /// <param name="query">A <see cref="NameValueCollection"/> of items to use.</param>
    /// 
    /// <returns>The modified <see cref="Uri"/>.</returns>
    public static Uri SetQuery(this Uri uri, NameValueCollection query) => new UriBuilder(uri)
    {
      Query = query.ToQueryString()
    }.Uri;

    /// <summary>
    /// Get query values for <paramref name="key"/>, safe cast to <typeparamref name="T"/>.
    /// </summary>
    /// 
    /// <typeparam name="T">The type to cast to.</typeparam>
    /// <param name="query">An instance of <see cref="IQueryCollection"/>.</param>
    /// <param name="key">The key to lookup.</param>
    /// 
    /// <returns>An <see cref="IEnumerable{T}"/> of items from the <see cref="IQueryCollection"/>, that successfully cast to <typeparamref name="T"/>.</returns>
    public static IEnumerable<T> GetValues<T>(this IQueryCollection query, string key) => query.TryGetValue(key, out StringValues strVal) ? strVal.Select(x => x.TryConvertTo<T>().Result).Where(x => x is not null).Cast<T>() : Enumerable.Empty<T>();
  }
}
