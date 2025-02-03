using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.RegularExpressions;

namespace LocalGovUmbraco.TagHelpers
{
  /// <summary>
  /// Tag helper to dynamically render a html heading tag.
  /// </summary>
  [HtmlTargetElement("datatable", Attributes = "data", TagStructure = TagStructure.NormalOrSelfClosing)]
  public partial class DataTableTagHelper : TagHelper
  {
    /// <summary>
    /// Backing variable for the data.
    /// </summary>
    private IEnumerable<Dictionary<string, string?>>? _data;

    /// <summary>
    /// The level of the heading
    /// </summary>
    [HtmlAttributeName("ariaLabel")]
    public string? AriaLabel { get; set; }

    /// <summary>
    /// The data for the table.
    /// </summary>
    [HtmlAttributeName("data")]
    public IEnumerable<Dictionary<string, string?>>? Data
    {
      get => _data;
      set => _data = Normalise(value);
    }

    /// <summary>
    /// Normalise the <see cref="IEnumerable{Dictionary{string, string?}}"/> ensuring all <see cref="Dictionary<string, string?>"/> contain the same keys.
    /// </summary>
    /// 
    /// <param name="data"></param>
    /// <returns></returns>
    private static IEnumerable<Dictionary<string, string?>>? Normalise(IEnumerable<Dictionary<string, string?>>? data)
    {
      if (data is null)
      {
        return null;
      }

      IEnumerable<IEnumerable<string>> eKeys = data.Select(x => x.Keys);
      IEnumerable<string> keys = eKeys.Skip(1).Aggregate(new HashSet<string>(eKeys.First()), (h, e) => h.Union(e).ToHashSet());
      return data.Select((x, i) =>
      {
        Dictionary<string, string?> dict = new();
        foreach (string key in keys)
        {
          dict.Add(key, x.GetValue(key));
        }

        return dict;
      });
    }

    /// <summary>
    /// Regex to match non-alphanumeric characters.
    /// </summary>
    [GeneratedRegex("[^\\da-z]+")]
    private static partial Regex NonAlphaNum();

    /// <summary>
    /// Convert a string into a CSS safe slug.
    /// </summary>
    /// 
    /// <param name="input">The converted string.</param>
    /// <returns></returns>
    private static string Slug(string input) => NonAlphaNum().Replace(string.Concat(input.Replace("\'", string.Empty).Select((x, i) => i > 0 && char.IsUpper(x) ? $" {x}" : x.ToString())).ToLower(), "-");

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      if (!(Data?.Any() ?? false) || Data.Max(x => x.Values.Count) == 0)
      {
        output.TagName = null;
        output.SuppressOutput();
        return;
      }

      output.TagName = "div";
      output.Attributes.Add("data-table", null);
      output.Attributes.Add("role", "table");
      if (!AriaLabel.IsNullOrWhiteSpace())
      {
        output.Attributes.Add("arial-label", AriaLabel);
      }

      int rowIndex = 1;
      foreach (Dictionary<string, string?> dict in Data)
      {
        output.Content.AppendHtml($"<dl class=\"row\" role=\"row\" aria-rowindex=\"{rowIndex}\">");

        int cellIndex = 1;
        foreach (KeyValuePair<string, string?> item in dict)
        {
          output.Content.AppendHtml($"<dt data-key=\"{Slug(item.Key)}\" role=\"columnheader\" class=\"{(item.Value.IsNullOrWhiteSpace() ? "empty" : null)}\">");
          output.Content.Append(item.Key);
          output.Content.AppendHtml("</dt>");

          output.Content.AppendHtml($"<dd data-key=\"{Slug(item.Key)}\" role=\"cell\" aria-cellindex=\"{cellIndex++}\" class=\"{(item.Value.IsNullOrWhiteSpace() ? "empty"  : null)}\">");
          output.Content.Append(item.Value);
          output.Content.AppendHtml("</dd>");
        }

        output.Content.AppendHtml("</dl>");
        rowIndex++;
      }
    }
  }
}
