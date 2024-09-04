using System.Globalization;

namespace TrackTime.Cli.Shared;

public class CustomDateTimeArgument(string input)
{
    public DateTime Value { get; } = input switch
    {
        string d when d.Equals("now", StringComparison.CurrentCultureIgnoreCase) => DateTime.Now, // TODO: testable interface
        "today" => DateTime.Today,
        string d when d.Equals("tomorrow", StringComparison.CurrentCultureIgnoreCase) => DateTime.Today + TimeSpan.FromDays(1),
        string d when d.Equals("yesterday", StringComparison.CurrentCultureIgnoreCase) => DateTime.Today - TimeSpan.FromDays(1),
        string d when DateTime.TryParseExact(d, "H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var time) => DateTime.Today.Add(time.TimeOfDay),
        _ => throw new ArgumentException($"Invalid timestamp: {input}. Expected 'now', or a time in 'H:mm' format.")
    };
}

