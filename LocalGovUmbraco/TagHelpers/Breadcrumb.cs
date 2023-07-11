using Microsoft.AspNetCore.Razor.TagHelpers;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace LocalGovUmbraco.TagHelpers
{
  /// <summary>
  /// Tag helper to build a breadcrumb menu for a given page.
  /// </summary>
  [HtmlTargetElement("breadcrumb", Attributes = "for", TagStructure = TagStructure.NormalOrSelfClosing)]
  public class BreadcrumbTagHelper : TagHelper
  {
    /// <summary>
    /// An <see cref="IPublishedContent"/> to build the breadcrumb for.
    /// </summary>
    [HtmlAttributeName("for")]
    public IPublishedContent? CurrentPage { get; set; }

    /// <summary>
    /// <para>Whether to include a dropdown of all child pages.</para>
    /// <para>Note: This value is overridden by DescendantsWhen() if it's set.</para>
    /// </summary>
    [HtmlAttributeName("descendants")]
    public bool Descendants { get; set; } = false;

    /// <summary>
    /// <para>A lambda to conditionally determine whether to display child pages.</para>
    /// <para>Note: Overrides the value of Descendants.</para>
    /// </summary>
    [HtmlAttributeName("descendantsWhen")]
    public Func<IPublishedContent, bool>? DescendantsWhen { get; set; }

    /// <summary>
    /// <para>Callback function to filters child pages.</para>
    /// <para>Note: Has no effect if Descendents/DescendantsWhen are false/null.</para>
    /// </summary>
    [HtmlAttributeName("descendantsWhere")]
    public Func<IPublishedContent, bool>? DescendantsWhere { get; set; }

    /// <summary>
    ///  <para>A LINQ Where() callback function to filter the label for a particular link.</para>
    ///  <para>Accepts a single <see cref="IPublishedContent"/> parameter.</para>
    /// </summary>
    [HtmlAttributeName("label")]
    public Func<IPublishedContent, string> FilterLabel { get; set; } = x => x.Name;

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      if (CurrentPage is null || CurrentPage.Level <= 1)
      {
        output.TagName = null;
        output.SuppressOutput();
        return;
      }

      output.TagName = "nav";
      TagHelperAttribute? classAttribute = output.Attributes.TryGetAttribute("class", out classAttribute) ? classAttribute : null;
      output.Attributes.SetAttribute("class", string.Join(' ', ((classAttribute?.Value.ToString())?.Split(' ') ?? Array.Empty<string>()).Prepend("breadcrumb").Distinct().Where(x => !string.IsNullOrWhiteSpace(x))));

      output.Content.AppendHtml("<ol class=\"menu\">");
      for (int i = 1; i <= CurrentPage.Level; ++i)
      {
        if (CurrentPage.AncestorOrSelf(i) is IPublishedContent item)
        {
          output.Content.AppendHtml("<li>");
          if (i < CurrentPage.Level)
          {
            output.Content.AppendHtml($"<a href=\"{@item.Url()}\" title=\"{FilterLabel(item)}\">{FilterLabel(item)}</a>");
          }
          else if ((DescendantsWhen is not null ? DescendantsWhen(CurrentPage) : Descendants) && CurrentPage.Children?.Where(x => x.TemplateId > 0).Where(DescendantsWhere ?? (x => x.IsVisible())) is IEnumerable<IPublishedContent> children && children.Any())
          {
            output.Content.AppendHtml("<details class=\"current-page\">");
            output.Content.AppendHtml($"<summary>More in {FilterLabel(item)}</summary>");
            output.Content.AppendHtml("<ul class=\"menu\">");
            foreach (IPublishedContent child in children)
            {
              output.Content.AppendHtml("<li>");
              output.Content.AppendHtml($"<a href=\"{child.Url()}\" title=\"{FilterLabel(child)}\">{FilterLabel(child)}</a>");
              output.Content.AppendHtml("</li>");
            }
            output.Content.AppendHtml("</ul>");
            output.Content.AppendHtml("</details>");
          }
          else
          {
            output.Content.AppendHtml($"<span class=\"current-page\">{FilterLabel(item)}</span>");
          }
          output.Content.AppendHtml("</li>");
        }
      }
      output.Content.AppendHtml("</ol>");

      base.Process(context, output);
    }
  }
}
