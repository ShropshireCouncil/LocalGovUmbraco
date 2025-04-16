using Microsoft.AspNetCore.Razor.TagHelpers;

namespace LocalGovUmbraco.TagHelpers
{
  /// <summary>
  /// Tag helper for generating time tags.
  /// </summary>
  [HtmlTargetElement("datetime", Attributes = "value", TagStructure = TagStructure.NormalOrSelfClosing)]
  public class DateTimeTagHelper : TagHelper
  {
    /// <summary>
    /// The <see cref="DateTime"/> object to generate a tag for.
    /// </summary>
    [HtmlAttributeName("value")]
    public DateTime Value { get; set; }

    /// <summary>
    /// List of ordinal suffixes.
    /// </summary>
    private static readonly string[] _ordinalSuffixes = ["th", "st", "nd", "rd"];

    /// <summary>
    /// Display ordinal suffixes on dates?
    /// </summary>
    [HtmlAttributeName("ordinal")]
    public bool OrdinalSuffix { get; set; } = true;

    /// <summary>
    /// Display the date portion of the <see cref="DateTime"/> object to the user?
    /// </summary>
    [HtmlAttributeName("date")]
    public bool ShowDate { get; set; } = true;

    /// <summary>
    /// Display the year portion of the <see cref="DateTime"/> object to the user?
    /// </summary>
    [HtmlAttributeName("year")]
    public bool? ShowYear { get; set; }

    /// <summary>
    /// Display the time portion of the <see cref="DateTime"/> object to the user?
    /// </summary>
    [HtmlAttributeName("time")]
    public bool ShowTime { get; set; } = false;

    /// <summary>
    /// A separator for the date and time portions.
    /// </summary>
    [HtmlAttributeName("separator")]
    public string? Separator { get; set; }

    /// <summary>
    /// The format for the time portion.
    /// </summary>
    [HtmlAttributeName("twelveHour")]
    public bool TwelveHourDisplay { get; set; } = true;

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      output.TagName = "time";
      output.Attributes.SetAttribute("datetime", Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"));
      output.Attributes.SetAttribute("title", $"{Value:d MMMM} at {Value:h:mmtt}");

      if (ShowDate || (ShowYear ?? false))
      {
        output.Content.Append(Value.Day.ToString());
        if (OrdinalSuffix)
        {
          output.Content.Append(_ordinalSuffixes[Math.Min(Value.Day % (Value.Day < 30 ? 20 : 30), 4) % 4]);
        }
        output.Content.Append(Value.ToString(" MMMM"));
        if (ShowYear ?? Value.Year != DateTime.Now.Year)
        {
          output.Content.Append(Value.ToString(" yyyy"));
        }
      }

      if (ShowTime)
      {
        output.Content.Append($"{Separator ?? (ShowDate ? " at " : null)}{(TwelveHourDisplay ? Value.ToString("h:mmtt").ToLowerInvariant() : Value.ToString("HH:mm"))}");
      }
    }
  }
}
