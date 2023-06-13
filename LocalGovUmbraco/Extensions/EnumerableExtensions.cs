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
      Dictionary<TKey, IEnumerable<TValue?>> data = new();
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
      List<Dictionary<TKey, TValue?>> data = Enumerable.Repeat<Dictionary<TKey, TValue?>>(new(), source.Values.MaxBy(x => x.Count())?.Count() ?? 0).ToList();
      for (int i = 0; i < data.Count; ++i)
      {
        foreach (KeyValuePair<TKey, IEnumerable<TValue?>> item in source)
        {
          data[i].Add(item.Key, item.Value.Count() < i ? item.Value.ElementAt(i) : default);
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
  }
}
