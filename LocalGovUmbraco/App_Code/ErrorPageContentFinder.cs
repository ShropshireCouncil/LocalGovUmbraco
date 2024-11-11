using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace LocalGovUmbraco.Extensions
{
  public class ErrorPageContentFinder(IUmbracoContextAccessor contextAccessor) : IContentLastChanceFinder
  {
    private readonly IUmbracoContextAccessor _contextAccessor = contextAccessor;

    public Task<bool> TryFindContent(IPublishedRequestBuilder request)
    {
      if (_contextAccessor.TryGetUmbracoContext(out IUmbracoContext? umbracoContext) && umbracoContext.Content?.GetAtRoot().FirstOfType<ErrorPage>() is ErrorPage errorPage)
      {
        request.SetPublishedContent(errorPage);
        request.SetResponseStatus(404);
        return Task.FromResult(true);
      }

      return Task.FromResult(false);
    }
  }

  public class ErrorPagerComposer : IComposer
  {
    public void Compose(IUmbracoBuilder builder) => builder.SetContentLastChanceFinder<ErrorPageContentFinder>();
  }
}
