namespace TrackTime.Lib.Stamps;

public class Stamp
{
    public required DateTime Time { get; set; }
    /// You can specify the time independent from the date.
    public TimeOnly? At
    {
        set => this.Time = (value != null) ?
                this.Time = this.Time.Date + value.Value.ToTimeSpan()
                : this.Time;
    }
    public string? Reason { get; set; }

    public bool StampIn { get; set; } = false;
    public bool StampOut { get; set; } = false;

    public override string ToString()
    {
        return $"Stamp at {Time:yyyy-MM-dd HH-mm} in: {StampIn} out: {StampOut} with Reason {Reason}";
    }
}