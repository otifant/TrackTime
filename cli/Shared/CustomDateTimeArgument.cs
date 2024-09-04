using System.Globalization;

namespace TrackTime.Cli.Shared;

public class CustomDateTimeArgument(string input)
{
    private static readonly CultureInfo _invariantCulture = CultureInfo.InvariantCulture;

    public DateTime Value { get; } = input switch
    {
        string d when d.Equals("now", StringComparison.CurrentCultureIgnoreCase) => DateTime.Now, // TODO: testable interface
        "today" => DateTime.Today,
        string d when d.Equals("tomorrow", StringComparison.CurrentCultureIgnoreCase) => DateTime.Today + TimeSpan.FromDays(1),
        string d when d.Equals("yesterday", StringComparison.CurrentCultureIgnoreCase) => DateTime.Today - TimeSpan.FromDays(1),
        string d when DateTime.TryParseExact(d, "H:mm", _invariantCulture, DateTimeStyles.None, out var time) => DateTime.Today.Add(time.TimeOfDay),
        string d when DateTime.TryParseExact(d, "yyyy-M-d", _invariantCulture, DateTimeStyles.None, out var time) => time,
        _ => throw new ArgumentException($"Invalid timestamp: {input}. Expected 'now', or a time in 'H:mm' format.")
    };
}

