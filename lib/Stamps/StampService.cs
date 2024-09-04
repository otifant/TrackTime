using TrackTime.Cli.Shared;
using TrackTime.Cli.Stamps.Model;
using TrackTime.Lib.Store;

namespace TrackTime.Lib.Stamps;

public class StampService
{

    public async Task<List<Stamp>?> GetStamps(DateTime day, FileInfo? file)
    {
        var store = GetStore(file);
        return await store.GetStamps(day);
    }

    public async Task Stamp(CustomDateTimeArgument time, TimeOnly? at, string? reason, FileInfo? file, bool stampIn, bool stampOut)
    {
        JsonStore jsonStore = GetStore(file);
        await jsonStore.AppendToStore(new()
        {
            Time = time.Value,
            Reason = reason,
            StampIn = stampIn,
            StampOut = stampOut,
            At = at,
        });
        Console.WriteLine($"Wrote to json store ({jsonStore.FilePath}).");
        // Console.WriteLine($"You stamp on {time.Value.ToString("yyyy-MM-dd HH-mm")} on '{reason ?? "unknown"}'.");
    }

    private static JsonStore GetStore(FileInfo? file)
    {
        return new JsonStore() { FilePath = file ?? JsonStore.DefaultPath };
    }
}