using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.RegularExpressions;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace LocalGovUmbraco.TagHelpers
{
  /// <summary>
  /// Tag helper to build a nested menu from a list of pages.
  /// </summary>
  [HtmlTargetElement("nav-menu", Attributes = "content", TagStructure = TagStructure.NormalOrSelfClosing)]
  public partial class MenuTagHelper : TagHelper
  {
    /// <summary>
    /// An <see cref="IEnumerable" /> of <see cref="IPublishedContent"/> for the top level menu items.
    /// </summary>
    [HtmlAttributeName("content")]
    public IEnumerable<IPublishedContent>? MenuItems { get; set; } = Enumerable.Empty<IPublishedContent>();

    /// <summary>
    ///   <para>The maximum depth to recurse to.</para>
    ///   <para>Defaults to -1 (no limit).</para>
    /// </summary>
    [HtmlAttributeName("maxdepth")]
    public int MaxDepth { get; set; } = -1;

    /// <summary>
    ///  <para>A LINQ Where() callback function to filter the children for display.</para>
    ///  <para>Accepts a single <see cref="IPublishedContent"/> parameter.</para>
    /// </summary>
    [HtmlAttributeName("filter")]
    public Func<IPublishedContent, bool> Filter { get; set; } = x => x.IsVisible() && x.TemplateId > 0;

    /// <summary>
    ///  <para>A LINQ Where() callback function to enable/disable recursion for a particular link.</para>
    ///  <para>Accepts a single <see cref="IPublishedContent"/> parameter.</para>
    /// </summary>
    [HtmlAttributeName("recurseWhen")]
    public Func<IPublishedContent, bool> FilterRecurse { get; set; } = x => x.Parent is not null;

    /// <summary>
    ///  <para>A LINQ Where() callback function to filter the label for a particular link.</para>
    ///  <para>Accepts a single <see cref="IPublishedContent"/> parameter.</para>
    /// </summary>
    [HtmlAttributeName("label")]
    public Func<IPublishedContent, string> FilterLabel { get; set; } = x => x.Name;

    /// <summary>
    ///   (Optional) context for setting ancestor classes.
    /// </summary>
    [HtmlAttributeName("context")]
    public IPublishedContent? Context { get; set; }

    [GeneratedRegex("[^\\da-z]+")]
    private static partial Regex AlphaNum();

    /// <summary>
    /// Generate CSS classes for a given piece of content.
    /// </summary>
    /// 
    /// <param name="content">The <see cref="IPublishedContent"/> to generate classes for.</param>
    /// 
    /// <returns>A <see cref="List{string}"/> of CSS classes.</returns>
    private List<string> GenerateClasses(IPublishedContent content)
    {
      List<string> classes = new()
      {
          AlphaNum().Replace(string.Concat(content.Name.Replace("\'", string.Empty).Select((x, i) => i > 0 && char.IsUpper(x) ? $" {x}" : x.ToString())).ToLower(), "-"),
      };

      if (Context is not null)
      {
        if (Context == content)
        {
          classes.Add("current-page");
        }

        if (Context.Ancestors().Contains(content))
        {
          classes.Add("ancestor");
        }
      }

      return classes;
    }

    /// <summary>
    ///   <para>Renders the ul element for this tier.</para>
    ///   <para>Recurses if any children matching the <see cref="FilterCallback">FilterCallback</see> are found.</para>
    /// </summary>
    /// 
    /// <param name="items">An <see cref="IEnumerable" /> of <see cref="IPublishedContent"/> for this level.</param>
    /// <param name="depth">The current depth of recursion.</param>
    /// 
    /// <returns>A HTML string for the ul.</returns>
    private string RenderMenuTier(IEnumerable<IPublishedContent> items, int depth = 0)
    {
      string output = "<ul class=\"" + (depth == 0 ? "menu" : "submenu") + "\">";
      foreach (IPublishedContent item in items)
      {
        List<string> classes = GenerateClasses(item);
        IEnumerable<IPublishedContent> children = FilterRecurse(item) && (MaxDepth < 0 || MaxDepth > depth) && item.Children() is IEnumerable<IPublishedContent> descendants ? descendants.Where(Filter) : Enumerable.Empty<IPublishedContent>();
        if (children.Any())
        {
          classes.Add("has-children");
        }

        output += $"<li class=\"{string.Join(" ", classes)}\">";
        output += $"<a href=\"{item.Url()}\">{FilterLabel(item)}</a>";
        if (children.Any())
        {
          output += RenderMenuTier(children, depth + 1);
        }
        output += "</li>";
      }
      output += "</ul>";

      return output;
    }

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      if (!(MenuItems?.Any(Filter) ?? false))
      {
        output.TagName = null;
        output.SuppressOutput();
        return;
      }

      output.TagName = "nav";
      _ = output.Content.SetHtmlContent(RenderMenuTier(MenuItems.Where(Filter)));

      base.Process(context, output);
    }
  }
}
