using System.Collections;

namespace LocalGovUmbraco.PropertyEditors.EventInstances
{
  /// <summary>
  /// A list of event occurrences.
  /// </summary>
  public class EventDates : IEnumerable<EventDate>
  {
    /// <summary>
    /// The internal set of distincy event dates.
    /// </summary>
    private readonly HashSet<EventDate> _events = new();

    /// <summary>
    /// Get all event dates that start before (or on) the given date.
    /// </summary>
    /// 
    /// <param name="refDate">The date to lookup</param>
    /// 
    /// <returns>An ordered list of all matching event dates.</returns>
    public IOrderedEnumerable<EventDate> StartsBefore(DateTime refDate) => _events.Where(x => x.Start <= refDate).Order();

    /// <summary>
    /// Get all event dates that end before the given date.
    /// </summary>
    /// 
    /// <param name="refDate">The date to lookup</param>
    /// 
    /// <returns>An ordered list of all matching event dates.</returns>
    public IOrderedEnumerable<EventDate> EndsBefore(DateTime refDate) => _events.Where(x => x.End < refDate).Order();

    /// <summary>
    /// Get all event dates that start after  (or on) the given date.
    /// </summary>
    /// 
    /// <param name="refDate">The date to lookup</param>
    /// 
    /// <returns>An ordered list of all matching event dates.</returns>
    public IOrderedEnumerable<EventDate> StartsAfter(DateTime refDate) => _events.Where(x => x.Start >= refDate).Order();

    /// <summary>
    /// Get all event dates that end after the given date.
    /// </summary>
    /// 
    /// <param name="refDate">The date to lookup</param>
    /// 
    /// <returns>An ordered list of all matching event dates.</returns>
    public IOrderedEnumerable<EventDate> EndsAfter(DateTime refDate) => _events.Where(x => x.End > refDate).Order();

    /// <summary>
    /// Get all event dates that occur between the given dates.
    /// </summary>
    /// 
    /// <param name="startDate">The start date for the lookup</param>
    /// <param name="endDate">The end date for the lookup</param>
    /// <param name="allowOverlap">Include events that occur within the given bounds but start/finish outside them.</param>
    /// 
    /// <returns>An ordered list of all matching event dates.</returns>
    public IOrderedEnumerable<EventDate> Between(DateTime startDate, DateTime endDate, bool allowOverlap = false) => (allowOverlap ? StartsBefore(endDate).Intersect(EndsAfter(startDate)) : StartsAfter(startDate).Intersect(EndsBefore(endDate))).Order();

    /// <summary>
    /// An ordered list of all event dates.
    /// </summary>
    public IOrderedEnumerable<EventDate> Dates => _events.Order();

    /// <summary>
    /// An ordered list of all future event dates.
    /// </summary>
    public IOrderedEnumerable<EventDate> Upcoming => StartsAfter(DateTime.Now);

    /// <summary>
    /// An ordered list of all historical event dates.
    /// </summary>
    public IOrderedEnumerable<EventDate> Historical => EndsBefore(DateTime.Now).OrderDescending();

    /// <summary>
    /// An ordered list of all event dates end at some point in the future.
    /// </summary>
    public IOrderedEnumerable<EventDate> Current => EndsAfter(DateTime.Now);

    /// <summary>
    /// The current/next occurrence of this event.
    /// </summary>
    public EventDate? Next => Current.FirstOrDefault();

    /// <summary>
    /// Get the "display" date - either the next occurrence, or the last occurrence if the run has ended.
    /// </summary>
    public EventDate? Display => Next ?? Historical.FirstOrDefault();

    /// <summary>
    /// Initialise an empty event date list.
    /// </summary>
    public EventDates() { }

    /// <summary>
    /// Initialise an event date list with an existing set of dates.
    /// </summary>
    /// 
    /// <param name="dates">A list of <see cref="EventDate"/> instances to add.</param>
    public EventDates(IEnumerable<EventDate> dates) => dates.ToList().ForEach(Add);

    /// <summary>
    /// Initialise an event date list with an existing set of dates.
    /// </summary>
    /// 
    /// <param name="dates">A list of <see cref="DateTime"/> pairs to generate instances for.</param>
    public EventDates(IEnumerable<IEnumerable<DateTime>> dates) : this(dates.Where(x => x.Any()).Select(x => new EventDate(x.First(), x.Last()))) { }

    /// <summary>
    /// Adds an event date to the list.
    /// </summary>
    /// 
    /// <param name="date">The <see cref="EventDate"/> to add.</param>
    public void Add(EventDate date) => _events.Add(date);

    /// <summary>
    /// Adds an event date to the list.
    /// </summary>
    /// 
    /// <param name="startDate">The start date for the new <see cref="EventDate"/>.</param>
    /// <param name="endDate">The end date for the new <see cref="EventDate"/>.</param>
    public void Add(DateTime startDate, DateTime endDate) => Add(new(startDate, endDate));

    /// <summary>
    /// Convert the object to a string of the current/next event occurrence.
    /// </summary>
    /// 
    /// <returns>The current/next occurrence as a <see langword="string"/>.</returns>
    public override string? ToString() => Next?.Start.ToString();

    /// <inheritdoc cref="IEnumerable{EventDate}.GetEnumerator"/>
    public IEnumerator<EventDate> GetEnumerator() => GetEnumerator();

    /// <inheritdoc/>
    IEnumerator<EventDate> IEnumerable<EventDate>.GetEnumerator() => GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => Dates.GetEnumerator();
  }
}
