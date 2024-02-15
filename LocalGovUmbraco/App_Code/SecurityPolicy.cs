using Umbraco.Cms.Core.Services;

namespace LocalGovUmbraco
{
  /// <summary>
  /// Static extension class for security policy middleware.
  /// </summary>
  public static class SecurityPolicyAppBuilderExtension
  {
    /// <summary>
    /// Security Policy middleware.
    /// </summary>
    /// 
    /// <param name="app">The app to attach the middleware to.</param>
    /// 
    /// <returns>The extended <see cref="IApplicationBuilder"/> instance</returns>
    public static IApplicationBuilder UseSecurityPolicy(this IApplicationBuilder app) => app
      .UseXContentTypeOptions()
      .UseXfo(o => o.SameOrigin())
      .UseReferrerPolicy(o => o.StrictOrigin())
      .UseXXssProtection(o => o.EnabledWithBlockMode())
      .UseRedirectValidation(o => o.AllowSameHostRedirectsToHttps())
      .UseWhen(context => !context.Request.Path.StartsWithSegments(new PathString("/umbraco")), a =>
      {
        if (a.ApplicationServices.GetRequiredService<IRuntimeState>().EnableInstaller())
        {
          // Don't apply the CSP when the installer is running.
          return;
        }

        // Security policy for the front-end.
        a.UseCsp(o => o
          .DefaultSources(s => s.Self())
          .BaseUris(s => s.Self())
          .StyleSources(s => s.Self())
          .ScriptSources(s => s.Self().CustomSources(
            "https://*.googletagmanager.com",
            "https://www.youtube.com"
          ))
          .ImageSources(s => s.Self().CustomSources(
            "data:",
            "https://*.google-analytics.com",
            "https://*.googletagmanager.com"
          ))
          .FontSources(s => s.Self())
          .FrameSources(s => s.CustomSources(
            "https://www.youtube.com"
          ))
          .ConnectSources(s => s.Self().CustomSources(
            "https://*.google-analytics.com",
            "https://*.analytics.google.com",
            "https://*.googletagmanager.com"
          ))
          .ObjectSources(s => s.None())
        );
      })
      .UseWhen(context => context.Request.Path.StartsWithSegments(new PathString("/umbraco")), a =>
      {
        // Security policy for the back-office.
        a.UseCsp(o => o
          .DefaultSources(s => s.Self())
          .ScriptSources(s => s.Self().UnsafeInline().UnsafeEval())
          .StyleSources(s => s.Self().UnsafeInline())
          .ImageSources(s => s.Self().CustomSources("data:", "dashboard.umbraco.com", "our.umbraco.com"))
          .ChildSources(s => s.Self().CustomSources("marketplace.umbraco.com"))
          .ConnectSources(s => s.Self().CustomSources("our.umbraco.com"))
          .ObjectSources(s => s.None())
        );
      });
  }
}
