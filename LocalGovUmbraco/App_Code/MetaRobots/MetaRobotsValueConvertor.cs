using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace LocalGovUmbraco.PropertyEditors.MetaRobots
{
  /// <inheritdoc cref="IPropertyValueConverter"/>
  public class MetaRobotsValueConvertor : IPropertyValueConverter
  {
    /// <inheritdoc/>
    public Type GetPropertyValueType(IPublishedPropertyType propertyType) => typeof(string);

    /// <inheritdoc/>
    public PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) => PropertyCacheLevel.None;

    /// <inheritdoc/>
    public bool IsConverter(IPublishedPropertyType propertyType) => propertyType.EditorAlias.Equals("LocalGovUmbraco.PropertyEditors.MetaRobots");

    /// <inheritdoc/>
    public bool? IsValue(object? value, PropertyValueLevel level) => level switch
    {
      PropertyValueLevel.Source => value is string,
      PropertyValueLevel.Inter => value is string,
      PropertyValueLevel.Object => value is string,
      _ => throw new NotSupportedException($"Invalid level: {level}."),
    };

    /// <inheritdoc/>
    public object? ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object? source, bool preview) => source?.ToString() ?? string.Empty;

    /// <inheritdoc/>
    public object? ConvertIntermediateToXPath(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview) => inter?.ToString() ?? string.Empty;

    /// <inheritdoc/>
    public object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview) => inter?.ToString() ?? string.Empty;
  }
}
