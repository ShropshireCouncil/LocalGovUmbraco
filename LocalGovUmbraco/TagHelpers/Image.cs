using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Specialized;
using System.Web;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace LocalGovUmbraco.TagHelpers
{
  /// <summary>
  /// Tag helper for auto-generating srcset information for a given image.
  /// </summary>
  [HtmlTargetElement("media", Attributes = "src", TagStructure = TagStructure.NormalOrSelfClosing)]
  public class MediaTagHelper : TagHelper
  {
    /// <summary>
    /// The <see cref="MediaWithCrops"/> to generate tags for.
    /// </summary>
    [HtmlAttributeName("src")]
    public MediaWithCrops? Media { get; set; }

    /// <summary>
    /// <para>Alt text override.</para>
    /// <para>A <see cref="null"/> value will use the image name as the alt text.</para>
    /// </summary>
    [HtmlAttributeName("alt")]
    public string? AltText { get; set; }

    /// <summary>
    /// An optional image caption.
    /// </summary>
    [HtmlAttributeName("caption")]
    public string? Caption { get; set; }

    /// <summary>
    /// The maximum width for the image.
    /// </summary>
    [HtmlAttributeName("width")]
    public int? Width { get; set; }

    /// <summary>
    /// The maximum height for the image.
    /// </summary>
    [HtmlAttributeName("height")]
    public int? Height { get; set; }

    /// <summary>
    /// Lazy load the image.
    /// </summary>
    [HtmlAttributeName("lazyload")]
    public bool LazyLoad { get; set; } = false;

    /// <summary>
    /// A dictionary of breakpoints and image sizes.
    /// </summary>
    [HtmlAttributeName("sizes")]
    public IDictionary<string, (int?, int?)> Sizes { get; set; } = new Dictionary<string, (int?, int?)>
    {
      { "(max-width: 320px)",   (320, null) },
      { "(max-width: 480px)",   (480, null) },
      { "(max-width: 720px)",   (720, null) },
      { "(max-width: 1024px)", (1024, null) },
      { "(max-width: 1200px)", (1200, null) },
      { "(max-width: 1920px)", (1920, null) },
    };

    /// <summary>
    /// Whether to force the image output to high-compression webp or not.
    /// </summary>
    [HtmlAttributeName("webp")]
    public bool Webp { get; set; } = true;

    /// <summary>
    /// The image quality percentage.
    /// </summary>
    [HtmlAttributeName("quality")]
    public int Quality { get; set; } = 85;

    /// <summary>
    /// The image crop mode.
    /// </summary>
    [HtmlAttributeName("cropmode")]
    public ImageCropMode ImageCropMode { get; set; } = ImageCropMode.Crop;

    /// <summary>
    /// Use the focal point, if there is one.
    /// </summary>
    [HtmlAttributeName("preferfocalpoint")]
    public bool PreferFocalPoint { get; set; } = true;

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
      if (Media is null)
      {
        output.TagName = null;
        output.SuppressOutput();
        return;
      }
      
      Height = Height is int && Height != 0 ? Math.Abs(Height.Value) : null;
      Width = Width is int && Width != 0 ? Math.Abs(Width.Value) : null;

      output.TagName = "picture";
      if (!Caption.IsNullOrWhiteSpace())
      {
        output.TagName = "figure";
        output.PreContent.AppendHtml("<picture>");
        output.PostContent.AppendHtml("</picture>");
        output.PostContent.AppendHtml($"<figcaption>{Caption}</figcaption>");
      }

      if (Media.Content is UmbracoMediaVectorGraphics)
      {
        output.Content.AppendHtml($"<img src=\"{Media.MediaUrl()}\" alt=\"{AltText ?? Media.Name}{(Height is int ? " height=\"" + Height + "\"" : null)}{(Width is int ? " width=\"" + Width + "\"" : null)}{(LazyLoad ? " loading=\"lazy\"" : null)}>");
        return;
      }

      NameValueCollection options = new()
      {
        { "quality", Math.Clamp(Math.Abs(Quality), 0, 100).ToString() }
      };

      if (Webp)
      {
        options.Add("format", "webp");
      }

      string? furtherOptions = string.Join("&", options.AllKeys.SelectMany(k => options.GetValues(k)?.Select(v => string.Format("{0}={1}", HttpUtility.UrlEncode(k), HttpUtility.UrlEncode(v))) ?? Enumerable.Empty<string>()).WhereNotNull()).IfNullOrWhiteSpace(null);

      foreach (KeyValuePair<string, (int?, int?)> size in Sizes)
      {
        int? w = size.Value.Item1;
        int? h = size.Value.Item2;

        if (!w.HasValue && !h.HasValue)
        {
          continue;
        }

        if (Width.HasValue && Height.HasValue && (!w.HasValue | !h.HasValue))
        {
          if (w.HasValue)
          {
            h = (int) ((double) Height / Width * w);
          }
          else if (h.HasValue)
          {
            w = (int) ((double) Width / Height * h);
          }
        }

        if (w < Width || h < Height)
        {
          output.Content.AppendHtml($"<source media=\"{size.Key}\" srcset=\"{Media.GetCropUrl(w, h, imageCropMode: ImageCropMode.Crop, preferFocalPoint: true, furtherOptions: furtherOptions)}\">");
        }
      }

      output.Content.AppendHtml($"<img src=\"{Media.GetCropUrl(Width, Height, imageCropMode: ImageCropMode.Crop, preferFocalPoint: true, furtherOptions: furtherOptions)}\" alt=\"{AltText ?? Media.Name}\"{(LazyLoad ? " loading=\"lazy\"" : null)}>");
    }
  }
}
