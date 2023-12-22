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
      if (!lookup.Any())
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
    /// Compiled regex to match one or more non-alphanumeric characters.
    /// </summary>
    /// 
    /// <returns>The compiled <see cref="Regex"/></returns>
    [GeneratedRegex("[^\\da-z]+")]
    private static partial Regex NonAlphaNum();

    /// <summary>
    /// Generates a CSS safe slug for a given string.
    /// </summary>
    /// 
    /// <param name="input">The string to slug.</param>
    /// 
    /// <returns>A CSS safe slug.</returns>
    public static string Slug(this string? input) => input is not null ? NonAlphaNum().Replace(string.Concat(input.Replace("\'", string.Empty).Select((x, i) => (i > 0 && char.IsUpper(x)) ? $" {x}" : x.ToString())).ToLower(), "-").Trim('-') : string.Empty;
  }
}
