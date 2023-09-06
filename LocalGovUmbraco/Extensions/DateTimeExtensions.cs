namespace LocalGovUmbraco.Extensions
{
  /// <summary>
  /// DateTime extension functions
  /// </summary>
  public static class DateTimeExtensions
  {
    /// <summary>
    /// Round a <see cref="DateTime"/> value down to a given <see cref="TimeSpan"/> interval.
    /// </summary>
    /// 
    /// <param name="dateTime">The <see cref="DateTime"/> value to round.</param>
    /// <param name="interval">The <see cref="TimeSpan"/> interval to round to.</param>
    /// 
    /// <returns>The new <see cref="DateTime"/> value.</returns>
    public static DateTime Floor(this DateTime dateTime, TimeSpan interval) => dateTime.AddTicks(-(dateTime.Ticks % interval.Ticks));

    /// <summary>
    /// Round a <see cref="DateTime"/> value up to a given <see cref="TimeSpan"/> interval.
    /// </summary>
    /// 
    /// <param name="dateTime">The <see cref="DateTime"/> value to round.</param>
    /// <param name="interval">The <see cref="TimeSpan"/> interval to round to.</param>
    /// 
    /// <returns>The new <see cref="DateTime"/> value.</returns>
    public static DateTime Ceiling(this DateTime dateTime, TimeSpan interval) => dateTime.Floor(interval).AddTicks(interval.Ticks);

    /// <summary>
    /// Round a <see cref="DateTime"/> value to a given <see cref="TimeSpan"/> interval.
    /// </summary>
    /// 
    /// <param name="dateTime">The <see cref="DateTime"/> value to round.</param>
    /// <param name="interval">The <see cref="TimeSpan"/> interval to round to.</param>
    /// 
    /// <returns>The new <see cref="DateTime"/> value.</returns>
    public static DateTime Round(this DateTime dateTime, TimeSpan interval)
    {
      long halfIntervalTicks = (interval.Ticks + 1) >> 1;

      return dateTime.AddTicks(halfIntervalTicks - ((dateTime.Ticks + halfIntervalTicks) % interval.Ticks));
    }

    /// <summary>
    /// Gets the week start for a given date.
    /// </summary>
    /// 
    /// <param name="refDate">The reference <see cref="DateTime"/> to use.</param>
    /// <param name="startDay">The <see cref="DayOfWeek"/> that the week starts on (default: <see cref="DayOfWeek.Monday"/>).</param>
    /// 
    /// <returns>The <see cref="DateTime"/> of the week start.</returns>
    public static DateTime WeekStart(this DateTime refDate, DayOfWeek startDay = DayOfWeek.Monday)
    {
      int delta = startDay - refDate.DayOfWeek;

      return refDate.AddDays(delta <= 0 ? delta : delta - 7);
    }
  }
}
