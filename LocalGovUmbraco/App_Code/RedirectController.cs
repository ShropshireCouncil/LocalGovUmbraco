using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using NWebsec.AspNetCore.Core.Exceptions;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.ActionsResults;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace My.Website;

public class RedirectController(ILogger<RedirectController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor) : RenderController(logger, compositeViewEngine, umbracoContextAccessor)
{
  public override IActionResult Index()
  {
    if ((CurrentPage as Redirect)?.Destination?.Url is string url)
    {
      try
      {
        return Redirect(url);
      }
      catch (RedirectValidationException ex)
      {
        logger.LogError("{message} is not in the redirect whitelist", ex.Message);
      }
    }

    return new PublishedContentNotFoundResult(UmbracoContext);
  }
}
