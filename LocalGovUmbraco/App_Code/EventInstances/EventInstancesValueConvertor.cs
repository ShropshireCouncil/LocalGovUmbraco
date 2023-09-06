using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PublishedCache;

namespace LocalGovUmbraco.PropertyEditors.EventInstances
{
  /// <inheritdoc cref="IPropertyValueConverter"/>
  public class EventInstancesValueConvertor : IPropertyValueConverter
  {
    /// <summary>
    /// An instance of <see cref="IPublishedSnapshotAccessor"/>.
    /// </summary>
    private readonly IPublishedSnapshotAccessor PublishedSnapshotAccessor;

    /// <summary>
    /// Initialise the convertor, injecting <see cref="IPublishedSnapshotAccessor"/>.
    /// </summary>
    /// 
    /// <param name="publishedSnapshotAccessor">An instance of <see cref="IPublishedSnapshotAccessor"/>.</param>
    public EventInstancesValueConvertor(IPublishedSnapshotAccessor publishedSnapshotAccessor) => PublishedSnapshotAccessor = publishedSnapshotAccessor;

    /// <inheritdoc/>
    public Type GetPropertyValueType(IPublishedPropertyType propertyType) => typeof(EventDates);

    /// <inheritdoc/>
    public PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) => PropertyCacheLevel.None;

    /// <inheritdoc/>
    public bool IsConverter(IPublishedPropertyType propertyType) => propertyType.EditorAlias.Equals("LocalGovUmbraco.PropertyEditors.EventInstances");

    /// <inheritdoc/>
    public bool? IsValue(object? value, PropertyValueLevel level) => level switch
    {
      PropertyValueLevel.Source => null,
      PropertyValueLevel.Inter => null,
      PropertyValueLevel.Object => value is EventDates,
      _ => throw new NotSupportedException($"Invalid level: {level}."),
    };

    /// <inheritdoc/>
    public object? ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object? source, bool preview)
    {
      string? sourceString = source?.ToString();
      if (sourceString?.DetectIsJson() ?? false)
      {
        try
        {
          return JsonConvert.DeserializeObject(sourceString);
        }
        catch (Exception) { };
      }

      return sourceString;
    }

    /// <inheritdoc/>
    public object? ConvertIntermediateToXPath(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview) => inter?.ToString() ?? string.Empty;

    /// <inheritdoc/>
    public object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview) => new EventDates((inter as JArray)?.Select(x => (x as JArray)?.Select<JToken, DateTime?>(x => DateTime.TryParse(x.ToString(), out DateTime date) ? date.ToLocalTime() : null).Where(x => x is DateTime).Cast<DateTime>()).Where(x => x?.Any() ?? false).WhereNotNull() ?? Enumerable.Empty<IEnumerable<DateTime>>());
  }
}
