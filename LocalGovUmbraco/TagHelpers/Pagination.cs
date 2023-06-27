using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Specialized;
using System.Web;

namespace LocalGovUmbraco.Extensions
{
  /// <summary>
  /// Helper extentions for paginating data.
  /// </summary>
  public static class PaginationTagHelperExtensions
  {
    /// <summary>
    /// Get the current page number from the <see cref="HttpRequest"/>.
    /// </summary>
    /// 
    /// <param name="request">The contextual request</param>
    /// <param name="maxPages">The maximum numbe of pages available.</param>
    /// 
    /// <returns>The current page number.</returns>
    public static int CurrentPage(this HttpRequest request, int maxPages = -1) => int.TryParse(request.Query["page"].FirstOrDefault(), out int pageNum) ? Math.Max(Math.Min(pageNum, maxPages > 0 ? maxPages : pageNum), 1) : 1;

    /// <summary>
    /// Extract the <typeparamref name="T"/> objects for the given <see cref="HttpRequest"/>.
    /// </summary>
    /// 
    /// <typeparam name="T">The type of objects to extract.</typeparam>
    /// 
    /// <param name="request">The contextual request</param>
    /// <param name="items">The items to filter.</param>
    /// <param name="perPage">The number of items per page.</param>
    /// 
    /// <returns>The subset of <typeparamref name="T"/> objects.</returns>
    public static IEnumerable<T> Paginate<T>(this HttpRequest request, IEnumerable<T> items, int perPage) => perPage > 0 ? items.Skip((request.CurrentPage((int) Math.Ceiling(items.Count() / (double) perPage)) - 1) * perPage).Take(perPage) : items;
  }
}

namespace LocalGovUmbraco.TagHelpers
{
  using LocalGovUmbraco.Extensions;

  /// <summary>
  /// Tag helper for generating pagination links
  /// </summary>
  [HtmlTargetElement("pagination", Attributes = "context, count", TagStructure = TagStructure.NormalOrSelfClosing)]
  public class PaginationTagHelper : TagHelper
  {
    /// <summary>
    /// The total number of items to paginate.
    /// </summary>
    [HtmlAttributeName("count")]
    public int Count { get; set; }

    /// <summary>
    /// The curent HttpRequest context for the pagination.
    /// </summary>
    [HtmlAttributeName("context"),]
    public HttpRequest Context { get; set; } = default!;

    /// <summary>
    /// Backing field for <see cref="PerPage"/>
    /// </summary>
    private int _perPage = 12;

    /// <summary>
    /// The maximum number of items to display per page.
    /// </summary>
    [HtmlAttributeName("perPage")]
    public int PerPage
    {
      get => _perPage;
      set => _perPage = value > 0 ? value : 12;
    }

    /// <summary>
    /// Show previous/next links (always shown if ShowPages is false).
    /// </summary>
    [HtmlAttributeName("showPrevNext")]
    public bool ShowPrevNext { get; set; } = true;

    /// <summary>
    /// Maximum number of page links to display
    /// </summary>
    [HtmlAttributeName("maxDisplay")]
    public int MaxDisplay { get; set; } = 5;


    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      int pageCount = (int) Math.Ceiling((double) Count / PerPage);
      int currentPage = Context.CurrentPage(pageCount);

      if (pageCount <= 1)
      {
        output.TagName = null;
        output.SuppressOutput();
        return;
      }

      output.TagName = "ul";
      TagHelperAttribute? classAttribute = output.Attributes.TryGetAttribute("class", out classAttribute) ? classAttribute : null;
      output.Attributes.SetAttribute("class", string.Join(' ', ((classAttribute?.Value.ToString())?.Split(' ') ?? Array.Empty<string>()).Prepend("pagination").Where(x => !string.IsNullOrWhiteSpace(x)).Distinct()));
      output.Attributes.SetAttribute("data-prev-next", ShowPrevNext || MaxDisplay == 0 ? "true" : "false");
      output.Attributes.SetAttribute("data-page-links", MaxDisplay != 0 ? "true" : "false");

      NameValueCollection rawQuery = HttpUtility.ParseQueryString(Context.QueryString.ToString());
      rawQuery.Remove("page");
      IEnumerable<string> paginationQuery = rawQuery.AllKeys.SelectMany(k => rawQuery.GetValues(k)?.Select(v => string.Format("{0}={1}", HttpUtility.UrlEncode(k), HttpUtility.UrlEncode(v))) ?? Enumerable.Empty<string>()).WhereNotNull();

      if ((ShowPrevNext || MaxDisplay == 0) && currentPage > 1)
      {
        output.Content.AppendHtml($"<li class=\"prev\"><a href=\"{Context.Path}?{string.Join("&", paginationQuery.Concat(new[] { "page=" + (currentPage - 1) }))}\" title=\"Page {currentPage - 1}\">Previous<span class=\"screen-reader-text\"> page</span><span class=\"screen-reader-text pagination-page-count\"> ({currentPage - 1} of {pageCount})</span></a></li>");
      }

      if (MaxDisplay != 0)
      {
        int balance = (MaxDisplay + (MaxDisplay % 2 - 1)) / 2;
        for (int i = 1; i <= pageCount; ++i)
        {
          if (MaxDisplay < 0 || pageCount <= MaxDisplay || i == 1 || i == pageCount || (i >= currentPage && i <= MaxDisplay - currentPage) || (i <= currentPage && i >= pageCount - MaxDisplay) || (i >= currentPage - balance && i <= currentPage + balance))
          {
            if (currentPage != i)
            {
              output.Content.AppendHtml($"<li><a href=\"{Context.Path}?{string.Join("&", paginationQuery.Concat(new[] { "page=" + i }))}\" title=\"Page {i}\"><span class=\"screen-reader-text\">Navigate to page </span>{i}</a></li>");
            }
            else
            {
              output.Content.AppendHtml($"<li class=\"current\"><span title=\"Page {i}\"><span class=\"screen-reader-text\">Current page: </span>{i}</span></li>");
            }
          }
          else
          {
            if (i == 2 || i == pageCount - 1)
            {
              output.Content.AppendHtml("<li>&hellip;</li>");
            }
          }
        }
      }

      if ((ShowPrevNext || MaxDisplay == 0) && currentPage < pageCount)
      {
        output.Content.AppendHtml($"<li class=\"next\"><a href=\"{Context.Path}?{string.Join("&", paginationQuery.Concat(new[] { "page=" + (currentPage + 1) }))}\" title=\"Page {currentPage + 1}\">Next<span class=\"screen-reader-text\"> page</span><span class=\"screen-reader-text pagination-page-count\"> ({currentPage + 1} of {pageCount})</span></a></li>");
      }
    }
  }
}
