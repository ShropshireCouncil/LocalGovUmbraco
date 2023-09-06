namespace LocalGovUmbraco.PropertyEditors.EventInstances
{
  /// <summary>
  /// A single event occurrence.
  /// </summary>
  public class EventDate : IComparable<EventDate>
  {
    /// <summary>
    /// The event start date.
    /// </summary>
    public DateTime Start { get; }

    /// <summary>
    /// The event end date.
    /// </summary>
    public DateTime End { get; }

    /// <summary>
    /// Is the event an all-day event?
    /// </summary>
    public bool AllDay => End.TimeOfDay - Start.TimeOfDay >= new TimeSpan(23, 59, 0);

    /// <summary>
    /// Does the event span multiple days?
    /// </summary>
    public bool MultiDay => Start.Date < End.Date;

    /// <summary>
    /// Create a new <see cref="EventDate"/> for a given start and end time.
    /// </summary>
    /// 
    /// <param name="startDate">The start date.</param>
    /// <param name="endDate">The end date.</param>
    public EventDate(DateTime startDate, DateTime endDate)
    {
      Start = startDate;
      End = endDate;
    }

    /// <summary>
    /// Create a new all-day <see cref="EventDate"/> for a specific day.
    /// </summary>
    /// 
    /// <param name="startDate">The start date.</param>
    public EventDate(DateTime date) : this(date.Date, date.Date + new TimeSpan(23, 59, 0)) { }

    /// <inheritdoc/>
    public int CompareTo(EventDate? other)
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
  }
}
