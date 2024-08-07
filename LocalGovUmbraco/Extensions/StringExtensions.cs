using HtmlAgilityPack;
using Microsoft.AspNetCore.Html;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Web;
using Umbraco.Cms.Core.Strings;

namespace LocalGovUmbraco.Extensions
{
  /// <summary>
  /// Common string helper functions.
  /// </summary>
  public static partial class StringExtensions
  {
    /// <summary>
    /// Compiled regex to match multiple occurances of whitespace.
    /// </summary>
    /// 
    /// <returns>The compiled <see cref="Regex"/></returns>
    [GeneratedRegex("[\\r\\n\\s]+")]
    private static partial Regex MultipleWhitespace();

    /// <summary>
    /// Extract just the plain text content from the given string.
    /// </summary>
    /// 
    /// <param name="input">The string to sanitize.</param>
    /// 
    /// <returns>The string with all HTML tags removed</returns>
    public static string PlainText(this string input)
    {
      HtmlDocument doc = new();
      doc.LoadHtml(input.Replace("><", "> <"));
      string plainText = HttpUtility.HtmlDecode(doc.DocumentNode.InnerText).Trim();

      return MultipleWhitespace().Replace(plainText, " ");
    }

    /// <inheritdoc cref="PlainText(string)" />
    public static string PlainText(this IHtmlEncodedString input) => input.ToString()?.PlainText() ?? string.Empty;

    /// <summary>
    /// Convert <see cref="IHtmlContent"/> to a string.
    /// </summary>
    /// 
    /// <param name="input">The <see cref="IHtmlContent"/> to convert.</param>
    /// 
    /// <returns>The input, converted to a string</returns>
    public static IHtmlEncodedString ToHtmlEncodedString(this IHtmlContent input)
    {
      using StringWriter writer = new();
      input.WriteTo(writer, HtmlEncoder.Default);

      return new HtmlEncodedString(writer.ToString());
    }

    /// <inheritdoc cref="PlainText(string)" />
    public static string PlainText(this IHtmlContent input) => input.ToHtmlEncodedString()?.PlainText() ?? string.Empty;

    /// <summary>
    /// Compiled regex to match punctuation.
    /// </summary>
    /// 
    /// <returns>The compiled <see cref="Regex"/></returns>
    [GeneratedRegex("\\p{P}")]
    private static partial Regex Punctuation();

    /// <inheritdoc cref="Truncate(string, int, string?)" />
    public static string Truncate(this IHtmlEncodedString input, int length, string suffix = "…") => input.PlainText().Truncate(length, suffix);

    /// <inheritdoc cref="Truncate(string, int, string?)" />
    public static string Truncate(this IHtmlContent input, int length, string suffix = "…") => input.PlainText().Truncate(length, suffix);

    /// <inheritdoc cref="string.IsNullOrWhiteSpace(string?)"/>
    public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this IHtmlEncodedString? input) => string.IsNullOrWhiteSpace(input?.PlainText());

    /// <summary>
    /// Generate a snippet around the first occurance of given lookup string.
    /// </summary>
    /// 
    /// <param name="input">The string to truncate.</param>
    /// <param name="length">The length to truncate to.</param>
    /// <param name="lookup">The string to truncate around.</param>
    /// 
    /// <returns>The truncated string.</returns>
    public static string Snippet(this string input, int length, string[] lookup)
    {
      if (lookup.Length == 0)
      {
        return input.Truncate(length, "…");
      }

      int index = lookup.Select(x => input.IndexOf(x, StringComparison.CurrentCultureIgnoreCase)).Min();
      if (index < 0)
      {
        return input.Truncate(length, "…");
      }

      int offset = Math.Max(0, Math.Min(input.Length - length, index - (int) Math.Floor((double) length / 2)));
      string output = input[offset..Math.Min(offset + length, input.Length - 1)];

      if (offset > 0)
      {
        output = "…" + output;
      }

      if (offset + length < input.Length)
      {
        output += "…";
      }

      return output;
    }

    /// <inheritdoc cref="Snippet(string, int, string[])"/>
    public static string Snippet(this string input, int length, string lookup) => input.Snippet(length, new[] { lookup });

    /// <summary>
    /// Highlight regex matches in another string.
    /// </summary>
    /// 
    /// <param name="input">The input string to search.</param>
    /// <param name="highlight">A <see cref="Regex"/> representing the content to highlight.</param>
    /// 
    /// <returns>The input string with any matches of the regex marked up in HTML.</returns>
    public static IHtmlEncodedString Highlight(this string input, Regex highlight) => new HtmlEncodedString(highlight.Replace(input, "<strong>$1</strong>"));

    /// <summary>
    /// Highlight given strings in another string.
    /// </summary>
    /// 
    /// <param name="input">The input string to search.</param>
    /// <param name="highlight">The strings to highlight.</param>
    /// 
    /// <returns>The input string with any instances of the search strings marked up in HTML.</returns>
    public static IHtmlEncodedString Highlight(this string input, string[] highlight) => highlight.Length > 0 ? Highlight(input, new Regex($"({string.Join("|", highlight.Select(Regex.Escape))})")) : new HtmlEncodedString(input);

    /// <summary>
    /// Highlight a given string in another string.
    /// </summary>
    /// 
    /// <param name="input">The input string to search.</param>
    /// <param name="highlight">The string to highlight.</param>
    /// 
    /// <returns>The input string with any instances of the search string marked up in HTML.</returns>
    public static IHtmlEncodedString Highlight(this string input, string highlight) => !highlight.IsNullOrWhiteSpace() ? Highlight(input, new[] { highlight }) : new HtmlEncodedString(input);

    /// <summary>
    /// Compiled regex to match one or more non-alphanumeric characters.
    /// </summary>
    /// 
    /// <returns>The compiled <see cref="Regex"/></returns>
    [GeneratedRegex("[^\\da-z]+")]
    private static partial Regex NonAlphaNum();

    /// <summary>
    /// Static list of camel case identifiers.
    /// </summary>
    private static readonly string[] _camelCase = ["([a-z])([A-Z])", "([0-9])([a-zA-Z])", "([a-zA-Z])([0-9])"];

    /// <summary>
    /// Break apart a camel cased string.
    /// </summary>
    /// 
    /// <param name="input">The string to break apart.</param>
    /// 
    /// <returns>The string with spaces between each word.</returns>
    public static string BreakUpCamelCase(this string input) => _camelCase.Aggregate(input, (string current, string pattern) => Regex.Replace(current, pattern, "$1 $2", RegexOptions.IgnorePatternWhitespace));

    /// <summary>
    /// Generates a CSS safe slug for a given string.
    /// </summary>
    /// 
    /// <param name="input">The string to slug.</param>
    /// 
    /// <returns>A CSS safe slug.</returns>
    public static string Slug(this string? input) => input is not null ? NonAlphaNum().Replace(input.Replace("\'", string.Empty).BreakUpCamelCase().ToLower(), "-").Trim('-') : string.Empty;
  }
}
