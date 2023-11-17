using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Smidge;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace LocalGovUmbraco.Controllers
{
  /// <summary>
  /// Default render controller overrides
  /// </summary>
  public class RootController : RenderController
  {
    /// <summary>
    /// The <see cref="SmidgeHelper"/> singleton.
    /// </summary>
    private readonly SmidgeHelper _smidge;

    /// <summary>
    /// An instance of <see cref="IWebHostEnvironment"/>.
    /// </summary>
    private readonly IWebHostEnvironment _env;

    /// <summary>
    /// Dependency injector.
    /// </summary>
    public RootController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor, IWebHostEnvironment env, SmidgeHelper smidge) : base(logger, compositeViewEngine, umbracoContextAccessor)
    {
      _env = env;
      _smidge = smidge;
    }

    /// <inheritdoc/>
    public override IActionResult Index()
    {
      _smidge.CreateJsBundle("core-scripts").RequiresJs(
        "~/core/scripts/cookie-control.js", // Cookie pop-up
        "~/core/scripts/newsflash.js", // Makes 'newsflashes' dismissable
        "~/core/scripts/menu.js" // Mobile menu
      );

      ISmidgeRequire cssBundle = _smidge.CreateCssBundle("core-styles").RequiresCss(
        "~/core/css/grid.css", // Layout for the block grid editor
        "~/core/css/gds.css", // High-level GDS Design patterns
        "~/core/css/cookie-control.css", // Cookie pop-up
        "~/core/css/typography.css", // Custom fonts
        "~/core/css/defaults.css" // Set element defaults
      );

      DirectoryInfo coreBlocksDir = new(Path.Combine(_env.WebRootPath, "core", "css", "blocks"));
      if (coreBlocksDir.Exists)
      {
        foreach (FileInfo coreBlockCss in coreBlocksDir.GetFiles("*.css"))
        {
          cssBundle.RequiresCss("~/core/css/blocks/" + coreBlockCss.Name);
        }
      }

      ISmidgeRequire blocksBundle = _smidge.CreateCssBundle("blocks");
      DirectoryInfo customBlocksDir = new(Path.Combine(_env.WebRootPath, "css", "blocks"));
      if (customBlocksDir.Exists)
      {
        foreach (FileInfo customBlockCss in customBlocksDir.GetFiles("*.css"))
        {
          blocksBundle.RequiresCss("~/css/blocks/" + customBlockCss.Name);
        }
      }

      return base.Index();
    }
  }
}
