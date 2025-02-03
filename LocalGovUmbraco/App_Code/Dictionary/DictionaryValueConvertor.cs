using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace LocalGovUmbraco.Dictionary
{
  /// <inheritdoc cref="IPropertyValueConverter"/>
  public class DictionaryValueConvertor : IPropertyValueConverter
  {
    /// <inheritdoc/>
    public Type GetPropertyValueType(IPublishedPropertyType propertyType) => typeof(Dictionary<string, string>);

    /// <inheritdoc/>
    public PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType) => PropertyCacheLevel.None;

    /// <inheritdoc/>
    public bool IsConverter(IPublishedPropertyType propertyType) => propertyType.EditorAlias.Equals("LocalGovUmbraco.Dictionary");

    /// <inheritdoc/>
    public bool? IsValue(object? value, PropertyValueLevel level) => level switch
    {
      PropertyValueLevel.Source => null,
      PropertyValueLevel.Inter => null,
      PropertyValueLevel.Object => value is Dictionary<string, string>,
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
    public object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview) => inter is JObject jObject ? jObject.ToObject<Dictionary<string, string>>() : (object?) null;
  }
}
