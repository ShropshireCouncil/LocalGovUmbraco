using System.Collections;

namespace LocalGovUmbraco.Extensions
{
  /// <summary>
  /// Common enumerable helper functions.
  /// </summary>
  public static partial class EnumerableExtensions
  {
    /// <summary>
    /// Return the first item in the enumerable that matches type <typeparamref name="T"/>.
    /// </summary>
    /// 
    /// <typeparam name="T">The type to match.</typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// 
    /// <returns>The first occurance of <typeparamref name="T"/>, or null if no matching item found.</returns>
    public static T? FirstOfType<T>(this IEnumerable enumerable)
    {
      foreach (object o in enumerable)
      {
        if (o is T i)
        {
          return i;
        }
      }

      return default;
    }

    /// <summary>
    /// Convert a <see cref="Dictionary{TKey, IEnumerable{TValue}}"/> to an <see cref="IEnumerable{Dictionary{TKey, TValue}}"/>, preserving key / value relationships.
    /// </summary>
    /// 
    /// <typeparam name="TKey">The key <see cref="Type"/>.</typeparam>
    /// <typeparam name="TValue">The value <see cref="Type"/>.</typeparam>
    /// 
    /// <param name="source">The <see cref="Dictionary{TKey, IEnumerable{TValue}}"/> to invert.</param>
    /// 
    /// <returns>The inverted <see cref="IEnumerable{Dictionary{TKey, TValue}}"/>.</returns>
    public static Dictionary<TKey, IEnumerable<TValue?>> InvertKeyValues<TKey, TValue>(this IEnumerable<Dictionary<TKey, TValue?>> source) where TKey : IEquatable<TKey>
    {
      Dictionary<TKey, IEnumerable<TValue?>> data = [];
      foreach (Dictionary<TKey, TValue?> dict in source)
      {
        foreach (KeyValuePair<TKey, TValue?> item in dict)
        {
          if (!data.ContainsKey(item.Key))
          {
            data.Add(item.Key, Enumerable.Repeat(default(TValue), data.Values.MaxBy(x => x.Count())?.Count() ?? 0));
          }

          data[item.Key] = data[item.Key].Append(item.Value);
        }
      }

      return data;
    }

    /// <summary>
    /// Convert an  <see cref="IEnumerable{Dictionary{TKey, TValue}}"/> to a <see cref="Dictionary{TKey, IEnumerable{TValue}}"/>, preserving key / value relationships.
    /// </summary>
    /// 
    /// <typeparam name="TKey">The key <see cref="Type"/>.</typeparam>
    /// <typeparam name="TValue">The value <see cref="Type"/>.</typeparam>
    /// 
    /// <param name="source">The <see cref="IEnumerable{Dictionary{TKey, TValue}}"/> to invert.</param>
    /// 
    /// <returns>The inverted <see cref="Dictionary{TKey, IEnumerable{TValue}}"/>.</returns>
    public static IEnumerable<Dictionary<TKey, TValue?>> InvertKeyValues<TKey, TValue>(this Dictionary<TKey, IEnumerable<TValue?>> source) where TKey : IEquatable<TKey>
    {
      List<Dictionary<TKey, TValue?>> data = Enumerable.Range(1, source.Values.MaxBy(x => x.Count())?.Count() ?? 0).Select(i => new Dictionary<TKey, TValue?>()).ToList();
      for (int i = 0; i < data.Count; ++i)
      {
        foreach (KeyValuePair<TKey, IEnumerable<TValue?>> item in source)
        {
          data[i].Add(item.Key, item.Value.ElementAtOrDefault(i));
        }
      }

      return data;
    }

    /// <summary>
    /// Remove empty values from a <see cref="Dictionary{TKey, IEnumerable{TValue}}"/>, also removing redundant keys.
    /// </summary>
    /// 
    /// <typeparam name="TKey">The key <see cref="Type"/>.</typeparam>
    /// <typeparam name="TValue">The value <see cref="Type"/>.</typeparam>
    /// 
    /// <param name="source">The <see cref="Dictionary{TKey, IEnumerable{TValue}}"/> to filter.</param>
    /// 
    /// <returns>The filtered <see cref="Dictionary{TKey, IEnumerable{TValue}}"/>.</returns>
    public static Dictionary<TKey, IEnumerable<TValue>> NotEmpty<TKey, TValue>(this Dictionary<TKey, IEnumerable<TValue?>> source) where TKey : notnull
    {
      Dictionary<TKey, IEnumerable<TValue>> data = [];
      foreach (TKey key in source.Keys)
      {
        IEnumerable<TValue> values = source[key].Where(x => x is not null && (x is not string s || !s.IsNullOrWhiteSpace())).Cast<TValue>();
        if (values.Any())
        {
          data.Add(key, values);
        }
      }

      return data;
    }

    /// <summary>
    /// Filter a <see cref="Dictionary{TKey, IEnumerable{TValue}}"/> to distinct values, removing redundant keys.
    /// </summary>
    /// 
    /// <typeparam name="TKey">The key <see cref="Type"/>.</typeparam>
    /// <typeparam name="TValue">The value <see cref="Type"/>.</typeparam>
    /// 
    /// <param name="source">The <see cref="Dictionary{TKey, IEnumerable{TValue}}"/> to filter.</param>
    /// 
    /// <returns>The filtered <see cref="Dictionary{TKey, IEnumerable{TValue}}"/>.</returns>
    public static Dictionary<TKey, IEnumerable<TValue>> Distinct<TKey, TValue>(this Dictionary<TKey, IEnumerable<TValue>> source) where TKey : notnull
    {
      Dictionary<TKey, IEnumerable<TValue>> data = [];
      foreach (TKey key in source.Keys)
      {
        IEnumerable<TValue> values = source[key].Distinct();
        if (values.Any())
        {
          data.Add(key, values);
        }
      }

      return data;
    }

    /// <summary>
    /// Returns true if all strings in the other collection exist in this collection, allowing for a custom <see cref="StringComparer"/>. 
    /// </summary>
    /// 
    /// <param name="source">The first <see cref="IEnumerable{string}"/>.</param>
    /// <param name="values">The second <see cref="IEnumerable{string}"/>.</param>
    /// <param name="comparer">The <see cref="StringComparer"/> to use.</param>
    /// 
    /// <returns><see langword="true"/> if all the values were found in the source <see cref="IEnumerable{string}"/>, <see langword="false"/> otherwise.</returns>
    public static bool ContainsAll(this IEnumerable<string> source, IEnumerable<string> values, StringComparer comparer) => values.Where(x => source.Contains(x, comparer)).Count() == values.Count();

    /// <summary>
    /// Psuedo-randomly sort an IEnumerable.
    /// </summary>
    /// 
    /// <typeparam name="T">The type of element in the enumerable.</typeparam>
    /// <param name="source">The enumerable to shuffle.</param>
    /// 
    /// <returns>The shuffled enumerable.</returns>
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source) => source.Shuffle(new Random());

    /// <summary>
    /// Psuedo-randomly sort an IEnumerable.
    /// </summary>
    /// 
    /// <typeparam name="T">The type of element in the enumerable.</typeparam>
    /// <param name="source">The enumerable to shuffle.</param>
    /// <param name="rng">The random number generator to use.</param>
    /// 
    /// <returns>The shuffled enumerable.</returns>
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng) => source.OrderBy(_ => rng.Next());

    /// <summary>
    /// Filters an <see cref="IEnumerable{string}"/> to remove empty values.
    /// </summary>
    /// 
    /// <param name="source">The enumerable to filter.</param>
    /// 
    /// <returns>The filtered enumerable.</returns>
    public static IEnumerable<string> WhereNotNullOrWhiteSpace(this IEnumerable<string?>? source) => (source?.Where(x => !x.IsNullOrWhiteSpace()).Cast<string>()).EmptyNull();

    /// <summary>
    /// Merges to Dictionaries, overwriting duplicate keys.
    /// </summary>
    /// 
    /// <typeparam name="T">The primary dictionary type.</typeparam>
    /// <typeparam name="K">The key type in the dictionary.</typeparam>
    /// <typeparam name="V">The value type in the dictionary.</typeparam>
    /// 
    /// <param name="source">The first dictionary.</param>
    /// <param name="others">Other dictionaries to merge in.</param>
    /// 
    /// <returns>The merged dictionaries.</returns>
    public static T Merge<T, K, V>(this T source, params IDictionary<K, V>?[] others) where T : IDictionary<K, V>, new()
    {
      others.WhereNotNull().SelectMany(x => x).ToList().ForEach(x => source[x.Key] = x.Value);

      return source;
    }

    /// <summary>
    /// Return an alternative <see cref="IEnumerable{T}"/> if the source enumerable is null or empty.
    /// </summary>
    /// 
    /// <typeparam name="TSource">The type of the enumerable.</typeparam>
    /// 
    /// <param name="source">The source enumerable.</param>
    /// <param name="fallback">The fallback value.</param>
    /// 
    /// <returns>The source enumerable, or the fallback enumerable if the source is null or empty.</returns>
    public static IEnumerable<TSource> IfNullOrEmpty<TSource>(this IEnumerable<TSource>? source, IEnumerable<TSource> fallback) => source?.Any() ?? false ? source : fallback;
  }
}
