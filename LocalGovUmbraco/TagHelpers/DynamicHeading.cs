using Microsoft.AspNetCore.Razor.TagHelpers;

namespace LocalGovUmbraco.TagHelpers
{
  /// <summary>
  /// Tag helper to dynamically render a html heading tag.
  /// </summary>
  [HtmlTargetElement("heading", Attributes = "level", TagStructure = TagStructure.NormalOrSelfClosing)]
  public class DynamicHeadingHelper : TagHelper
  {
    /// <summary>
    /// The level of the heading
    /// </summary>
    [HtmlAttributeName("level")]
    public int? Level { get; set; }

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      output.TagName = $"h{Math.Clamp(Level ?? 2, 1, 6)}";
      base.Process(context, output);
    }
  }
}
