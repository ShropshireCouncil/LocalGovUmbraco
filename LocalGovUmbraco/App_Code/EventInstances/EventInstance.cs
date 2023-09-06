using Umbraco.Cms.Core.Models.PublishedContent;

namespace LocalGovUmbraco.Events
{
  /// <summary>
  /// A wrapper around a specific instance of an event.
  /// </summary>
  public class EventInstance : IComparable<EventInstance>, IEquatable<EventInstance>
  {
    /// <summary>
    /// The start date.
    /// </summary>
    public DateTime Start { get; }

    /// <summary>
    /// The end date.
    /// </summary>
    public DateTime End { get; }

    /// <summary>
    /// The event the instance is for.
    /// </summary>
    public IPublishedContent Content { get; }

    /// <summary>
    /// Is the event an all-day event?
    /// </summary>
    public bool AllDay => End.TimeOfDay - Start.TimeOfDay >= new TimeSpan(23, 59, 0);

    /// <summary>
    /// Does the event span multiple days?
    /// </summary>
    public bool MultiDay => Start.Date < End.Date;

    /// <summary>
    /// How long does the event last?
    /// </summary>
    public TimeSpan Duration => End - Start;

    /// <summary>
    /// Initialise a new <see cref="EventInstance"/>.
    /// </summary>
    /// 
    /// <param name="content">The event the instance is for.</param>
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    public EventInstance(IPublishedContent content, DateTime startDate, DateTime endDate)
    {
      Content = content;
      Start = startDate;
      End = endDate;
    }

    /// <summary>
    /// Initialise a new all-day <see cref="EventInstance"/>.
    /// </summary>
    /// 
    /// <param name="content">The event the instance is for.</param>
    /// <param name="date">The start date.</param>
    public EventInstance(IPublishedContent content, DateTime date) : this(content, date.Date, date.Date + new TimeSpan(23, 59, 0)) { }

    /// <inheritdoc/>
    public int CompareTo(EventInstance? other)
    {
      if (other is null)
      {
        return 1;
      }

      int diff = Start.CompareTo(other.Start);
      if (diff == 0)
      {
        diff = End.CompareTo(other.End);
      }

      return diff;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as EventInstance);

    /// <inheritdoc/>
    public bool Equals(EventInstance? other) => GetHashCode() == other?.GetHashCode();

    public static bool operator ==(EventInstance? a, EventInstance? b) => a?.Equals(b) ?? b is null;

    public static bool operator !=(EventInstance? a, EventInstance? b) => !a?.Equals(b) ?? b is not null;

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(Start.Ticks, End.Ticks, Content.Id);
  }
}
