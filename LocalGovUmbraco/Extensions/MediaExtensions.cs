using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ShropshireGov.Extensions
{
  public enum FileType
  {
    Folder,
    Archive,
    Image,
    Audio,
    Video,
    Csv,
    Pdf,
    Word,
    Excel,
    Powerpoint,
    Publisher,
  }

  /// <summary>
  /// Common media helper functions.
  /// </summary>
  public static partial class MediaExtensions
  {
    private static readonly string[] _csv = ["csv"];
    private static readonly string[] _pdf = ["pdf"];
    private static readonly string[] _word = ["rtf", "doc", "docx", "docm", "dot", "dotx", "dotm"];
    private static readonly string[] _excel = ["xls", "xlt", "xlm", "xlsx", "xlsm", "xltx", "xltm"];
    private static readonly string[] _powerpoint = ["ppt", "pot", "pps", "ppa", "pptx", "pptm", "potx", "potm", "ppam", "ppsx", "ppsm", "sldx", "sldm", "ppam"];
    private static readonly string[] _publisher = ["pub"];
    private static readonly string[] _archive = ["zip", "tar", "rar", "7z", "gz"];
    private static readonly string[] _video = ["m4v", "h264", "avi", "mkv", "mpg", "mpeg", "mov", "flv", "3gp", "wmv", "webm"];
    private static readonly string[] _audio = ["wav", "mp3", "aac", "ac3", "ogg", "wma", "m4a", "webv", "flac"];
    private static readonly string[] _image = ["jpg", "jpeg", "gif", "bmp", "tiff", "svg", "png"];

    /// <summary>
    /// Attempt to classify file type by extension
    /// </summary>
    /// 
    /// <param name="media">The media file to classify.</param>
    ///
    /// <returns>The classification of the file.</returns>
    public static FileType? Classification(this IPublishedContent content) => content.Value<string?>("umbracoExtension") switch
    {
      string x when _csv.Contains(x) => FileType.Csv,
      string x when _pdf.Contains(x) => FileType.Pdf,
      string x when _word.Contains(x) => FileType.Word,
      string x when _excel.Contains(x) => FileType.Excel,
      string x when _powerpoint.Contains(x) => FileType.Powerpoint,
      string x when _publisher.Contains(x) => FileType.Publisher,
      string x when _archive.Contains(x) => FileType.Archive,
      string x when _video.Contains(x) => FileType.Video,
      string x when _audio.Contains(x) => FileType.Audio,
      string x when _image.Contains(x) => FileType.Image,
      _ => null,
    };

    public static FileType? Classification(this MediaWithCrops media) => media.Content is Umbraco.Cms.Web.Common.PublishedModels.Folder ? FileType.Folder : Classification(media.Content);
  }
}
