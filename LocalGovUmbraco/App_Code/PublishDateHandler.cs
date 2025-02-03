using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Notifications;

namespace LocalGovUmbraco
{
  /// <summary>
  /// Composer to bind <see cref="PublishDateHandler"/> to the relevent <see cref="INotification"/> implementation.
  /// </summary>
  public class PublishDateComposer : IComposer
  {
    /// <inheritdoc/>
    public void Compose(IUmbracoBuilder builder)
    {
      builder.AddNotificationHandler<SendingContentNotification, PublishDateHandler>();
      builder.AddNotificationHandler<SendingMediaNotification, PublishDateHandler>();
    }
  }

  /// <summary>
  /// Handler for when Content or Media is saved.
  /// </summary>
  public class PublishDateHandler : INotificationHandler<SendingContentNotification>, INotificationHandler<SendingMediaNotification>
  {
    /// <summary>
    /// Callback to loop through a given tab list and locate the publishDate property, if available.
    /// </summary>
    /// 
    /// <param name="tabs">The tablist to enumerate.</param>
    public void ProcessTabs(IEnumerable<Tab<ContentPropertyDisplay>> tabs)
    {
      if (tabs.SelectMany(f => f.Properties ?? Enumerable.Empty<ContentPropertyDisplay>()).FirstOrDefault(f => f.Alias.InvariantEquals("publishDate")) is ContentPropertyDisplay property && string.IsNullOrWhiteSpace(property.Value as string))
      {
        property.Value = DateTime.UtcNow;
      }
    }

    /// <summary>
    /// Handle for when content is saved.
    /// </summary>
    /// 
    /// <param name="notification">The notification</param>
    public void Handle(SendingContentNotification notification) => ProcessTabs(notification.Content.Variants.Where(v => v.State == ContentSavedState.NotCreated).SelectMany(x => x.Tabs));

    /// <summary>
    /// Handle for when media is saved.
    /// </summary>
    /// 
    /// <param name="notification">The notification</param>
    public void Handle(SendingMediaNotification notification) => ProcessTabs(notification.Media.Tabs);
  }
}
