using System.Collections;
using System.Xml;

namespace LocalGovUmbraco.Models.Rss
{
  /// <summary>
  /// An RSS item.
  /// </summary>
  public class Item
  {
    /// <summary>
    /// An RSS media element.
    /// </summary>
    public class Enclosure
    {
      /// <summary>
      /// The URL of the media item.
      /// </summary>
      public Uri Url { get; }

      /// <summary>
      /// The media item file size.
      /// </summary>
      public int Length { get; }

      /// <summary>
      /// The media item MIME type.
      /// </summary>
      public string? MimeType { get; }

      /// <summary>
      /// Create a new media element.
      /// </summary>
      /// 
      /// <param name="url">The URL to the media item.</param>
      /// <param name="mimetype">The MIME type of the media item.</param>
      /// <param name="length">The file size of the media item.</param>
      public Enclosure(Uri url, string? mimetype = null, int length = -1)
      {
        Url = url;
        MimeType = mimetype;
        Length = length;
      }
    }

    /// <summary>
    /// The title of the item.
    /// </summary>
    public string? Title { get; }

    /// <summary>
    /// The URL of the item.
    /// </summary>
    public Uri? Link { get; }

    /// <summary>
    /// Indicates when the item was published.
    /// </summary>
    public DateTime? Published { get; }

    /// <summary>
    /// The item synopsis.
    /// </summary>
    public string? Description { get; }

    /// <summary>
    /// The email address of the author of the item.
    /// </summary>
    public string? Author { get; }

    /// <summary>
    /// URL of a page for comments relating to the item.
    /// </summary>
    public Uri? Comments { get; }

    /// <summary>
    /// Categories the item falls under.
    /// </summary>
    public string[] Categories { get; }

    /// <summary>
    /// Media object attached to the item.
    /// </summary>
    public Enclosure[] Enclosures { get; }

    /// <summary>
    /// Create a new RSS item.
    /// </summary>
    /// 
    /// <param name="title">The title of the item.</param>
    /// <param name="link">The URL of the item.</param>
    /// <param name="published">Indicates when the item was published.</param>
    /// <param name="description">The item synopsis.</param>
    /// <param name="author">The email address of the author of the item.</param>
    /// <param name="comments">URL of a page for comments relating to the item.</param>
    /// <param name="categories"> Categories the item falls under.</param>
    /// <param name="enclosures">Media object attached to the item.</param>
    public Item(string? title = null, Uri? link = null, DateTime? published = null, string? description = null, string? author = null, Uri? comments = null, string[]? categories = null, Enclosure[]? enclosures = null)
    {
      Title = title;
      Link = link;
      Published = published;
      Description = description;
      Author = author;
      Comments = comments;
      Categories = categories ?? Array.Empty<string>();
      Enclosures = enclosures ?? Array.Empty<Enclosure>();
    }
  }

  /// <summary>
  /// A feed of RSS items.
  /// </summary>
  public class Feed : IEnumerable<Item>
  {
    /// <summary>
    /// A list of RSS items
    /// </summary>
    private readonly List<Item> _items = [];

    /// <summary>
    /// A sorted list of RSS items, with the most recently published items first.
    /// </summary>
    public IOrderedEnumerable<Item> Items => _items.OrderByDescending(x => x.Published);

    /// <summary>
    /// The name of the channel.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// The URL to the HTML website corresponding to the channel.
    /// </summary>
    public Uri Link { get; }

    /// <summary>
    /// Phrase or sentence describing the channel.
    /// </summary>
    public string? Description { get; }

    /// <summary>
    /// The language the channel is written in.
    /// </summary>
    public string? Language { get; }

    /// <summary>
    /// Copyright notice for content in the channel.
    /// </summary>
    public string? Copyright { get; }

    /// <summary>
    /// Email address for person responsible for editorial content.
    /// </summary>
    public string? Editor { get; }

    /// <summary>
    /// The publication date for the content in the channel. 
    /// </summary>
    public DateTime? Published { get; }

    /// <summary>
    /// The last time the content of the channel changed.
    /// </summary>
    public DateTime? LastBuildDate { get; }

    /// <summary>
    /// An image that can be displayed with the channel.
    /// </summary>
    public Uri? Image { get; }

    /// <summary>
    /// One or more categories that the channel belongs to.
    /// </summary>
    public string[] Categories { get; } = Array.Empty<string>();

    /// <summary>
    /// Time to live. The number of minutes the channel can be cached before refreshing from the source.
    /// </summary>
    public int Ttl { get; } = 0;

    /// <summary>
    /// The length of the RSS feed item list.
    /// </summary>
    public int Length => _items.Count;

    /// <summary>
    /// Create an RSS feed object from XML data.
    /// </summary>
    /// 
    /// <param name="xmlData">The XML to parse</param>
    /// 
    /// <exception cref="XmlException">Invalid RSS 2.0 syntax</exception>
    public Feed(string xmlData)
    {
      XmlDocument xmlDoc = new();
      xmlDoc.LoadXml(xmlData);

      if (xmlDoc.SelectSingleNode("rss") is not XmlNode rss)
      {
        throw new XmlException("No RSS data found in feed");
      }

      XmlNamespaceManager xmlNs = new(xmlDoc.NameTable);
      foreach (XmlAttribute ns in rss.Attributes?.OfType<XmlAttribute>().Where(x => x.Prefix == "xmlns:") ?? Enumerable.Empty<XmlAttribute>())
      {
        xmlNs.AddNamespace(ns.LocalName, ns.Value);
      }

      if (rss.SelectSingleNode("channel") is not XmlNode channel)
      {
        throw new XmlException("No channel data found in feed");
      }

      if (channel.SelectSingleNode("title") is not XmlNode channelTitle)
      {
        throw new XmlException("Unable to parse required node: rss/channel/title");
      }
      Title = channelTitle.InnerText;

      if (channel.SelectSingleNode("link") is not XmlNode channelLink)
      {
        throw new XmlException("Unable to parse required node: rss/channel/link");
      }
      Link = new(channelLink.InnerText);

      Description = channel.SelectSingleNode("description")?.InnerText;
      Language = channel.SelectSingleNode("language")?.InnerText;
      Copyright = channel.SelectSingleNode("copyright")?.InnerText;
      Editor = channel.SelectSingleNode("managingEditor")?.InnerText;
      Published = DateTime.TryParse(channel.SelectSingleNode("pubDate")?.InnerText, out DateTime channelPubDate) ? channelPubDate : null;
      LastBuildDate = DateTime.TryParse(channel.SelectSingleNode("lastBuildDate")?.InnerText, out DateTime channelLastBuildDate) ? channelLastBuildDate : null;
      Categories = channel.SelectNodes("category")?.OfType<XmlNode>().Select(x => x.InnerText).Distinct().WhereNotNull().ToArray() ?? Array.Empty<string>();
      Image = Uri.TryCreate(channel.SelectSingleNode("image/url")?.InnerText, UriKind.Absolute, out Uri? itemImg) ? itemImg : null;
      Ttl = int.TryParse(channel.SelectSingleNode("copyright")?.InnerText, out int channelTtl) ? channelTtl : 0;

      IEnumerable<XmlNode> items = channel.SelectNodes("item")?.OfType<XmlNode>() ?? Enumerable.Empty<XmlNode>();
      foreach (XmlNode item in items)
      {
        _items.Add(new(
          title: item.SelectSingleNode("title")?.InnerText,
          link: Uri.TryCreate(item.SelectSingleNode("link")?.InnerText, UriKind.Absolute, out Uri? itemLink) ? itemLink : null,
          published: DateTime.TryParse(item.SelectSingleNode("pubDate")?.InnerText, out DateTime itemPubDate) ? itemPubDate : null,
          description: item.SelectSingleNode("description")?.InnerText,
          author: item.SelectSingleNode("author")?.InnerText,
          comments: Uri.TryCreate(item.SelectSingleNode("comments")?.InnerText, UriKind.Absolute, out Uri? itemComments) ? itemComments : null,
          categories: item.SelectNodes("category")?.OfType<XmlNode>().Select(x => x.InnerText).Distinct().WhereNotNull().ToArray() ?? Array.Empty<string>(),
          enclosures: item.SelectNodes("enclosure")?.OfType<XmlNode>().Select(x => Uri.TryCreate(x.AttributeValue<string>("url"), UriKind.Absolute, out Uri? enclosureUrl) ? new Item.Enclosure(
            url: enclosureUrl,
            mimetype: x.AttributeValue<string>("type"),
            length: x.AttributeValue<int?>("length") ?? -1
          ) : null).WhereNotNull().ToArray()
        ));
      }
    }

    /// <summary>
    /// Parse an RSS feed from a remote URL.
    /// </summary>
    /// 
    /// <param name="feedUrl">The feed URL.</param>
    /// 
    /// <returns>An <see cref="Feed"/> built from the remote XML.</returns>
    public static async Task<Feed> FromRemoteAsync(Uri feedUrl) => await FromRemoteAsync(feedUrl, new HttpClient());

    /// <summary>
    /// Parse an RSS feed from a remote URL.
    /// </summary>
    /// 
    /// <param name="feedUrl">The feed URL.</param>
    /// <param name="httpClient">The HttpClient to use.</param>
    /// 
    /// <returns>An <see cref="Feed"/> built from the remote XML.</returns>
    public static async Task<Feed> FromRemoteAsync(Uri feedUrl, HttpClient httpClient)
    {
      string rssFeed = await httpClient.GetStringAsync(feedUrl);

      return new(rssFeed);
    }

    /// <summary>
    /// Parse an RSS feed from a remote URL.
    /// </summary>
    /// 
    /// <param name="feedUrl">The feed URL.</param>
    /// <param name="httpClientHandler">The HttpClientHandler to use.</param>
    /// <param name="baseAddress">The base URL (for relative feed URLs)</param>
    /// 
    /// <returns>An <see cref="Feed"/> built from the remote XML.</returns>
    public static async Task<Feed> FromRemoteAsync(Uri feedUrl, HttpClientHandler httpClientHandler, Uri? baseAddress = null) => await FromRemoteAsync(feedUrl, new HttpClient(handler: httpClientHandler, disposeHandler: true)
    {
      BaseAddress = baseAddress
    });

    /// <summary>
    /// Parse an RSS feed from a remote URL.
    /// </summary>
    /// 
    /// <param name="feedUrl">The feed URL.</param>
    /// <param name="baseAddress">The base URL (for relative feed URLs)</param>
    /// 
    /// <returns>An <see cref="Feed"/> built from the remote XML.</returns>
    public static async Task<Feed> FromRemoteAsync(Uri feedUrl, Uri? baseAddress = null) => await FromRemoteAsync(feedUrl, new(), baseAddress);

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc/>
    public IEnumerator<Item> GetEnumerator() => Items.GetEnumerator();
  }
}
