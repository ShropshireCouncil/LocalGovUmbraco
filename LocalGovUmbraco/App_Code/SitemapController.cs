using LocalGovUmbraco.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace LocalGovUmbraco.Controllers
{
  public class SitemapController : RenderController
  {
    private const string XmlTemplate = "SitemapXml";

    public SitemapController(ILogger<SitemapController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor) : base(logger, compositeViewEngine, umbracoContextAccessor) { }

    public override IActionResult Index() => Request.QueryCollection().GetValue<string?>("format", null) switch
    {
      "xml" => EnsurePhsyicalViewExists(XmlTemplate) ? View(XmlTemplate, CurrentPage) : CurrentTemplate(CurrentPage),
      _ => CurrentTemplate(CurrentPage),
    };
  }
}
