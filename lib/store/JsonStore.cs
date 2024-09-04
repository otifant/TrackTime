using System.Text.Json;
using TrackTime.Lib.Stamps;

namespace TrackTime.Lib.Store;

public class JsonStore
{
    public static readonly FileInfo DefaultPath = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tracktime.store.json"));
    private readonly JsonSerializerOptions _options = new(JsonSerializerDefaults.Web);

    /// Get the file path from the config file
    public FileInfo FilePath { get; init; } = DefaultPath;
    public async Task AppendToStore(Stamp stamp)
    {
        JsonStampStore stampStore =
            FilePath.Exists ?
            (await GetAllStamps())! :
            new JsonStampStore { Description = "new stamp store", Stamps = [] };

        stampStore.Stamps.Add(stamp);

        await using FileStream writeStream = FilePath.Open(FileMode.Create, FileAccess.Write);
        await JsonSerializer.SerializeAsync(writeStream, stampStore, _options);
    }

    public async Task<List<Stamp>?> GetStamps(DateTime day)
    {
        var store = await GetAllStamps();
        if (store == null) return null;

        var x = from stamp in store.Stamps where stamp.Time.Date == day.Date select stamp;
        return x.ToList();
    }

    /// <summary></summary>
    /// <returns>{null} if the file doesn't exist.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task<JsonStampStore?> GetAllStamps()
    {
        if (FilePath.Exists)
        {
            await using FileStream readStream = FilePath.OpenRead();
            return await JsonSerializer.DeserializeAsync<JsonStampStore>(readStream, _options) ?? throw new InvalidOperationException("Can't get JsonStampStore.");
        }
        else return null;
    }
}

public class JsonStampStore
{
    public string? Description { get; set; }
    public required List<Stamp> Stamps { get; set; }
}