using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using TrackTime.Cli;
using TrackTime.Cli.Shared;
using TrackTime.Cli.Stamps.Model;
using TrackTime.Lib.Stamps;
using TrackTime.Lib.Store;

StampService stampService = new();

var rootCommand = new RootCommand("Sample app for System.CommandLine");

var timeArgument = new Argument<CustomDateTimeArgument>(
    name: "time",
    description: "The time you want to stamp.",
    parse: input => new CustomDateTimeArgument(input.Tokens.Single().Value));

var atOption = new Option<TimeOnly?>(
    name: "--at",
    description: "Specify the time of day if not done yet. Format: H:mm.",
    parseArgument: input => input.Tokens.Count > 0 ? TimeOnly.ParseExact(input.Tokens.Single().Value, "H:mm") : null
    );

var reasonOption = new Option<string>(
    name: "--reason",
    description: "Add a description/reason for this stamp");

var inOption = new Option<bool>(
    name: "--in",
    description: "Specify that this stamp entry should be the start point."
);

var outOption = new Option<bool>(
    name: "--out",
    description: "Specify that this stamp entry should be the end point."
);

var fileOption = new Option<FileInfo?>(
    name: "--file",
    description: "The file where the stamps are stored in.",
    // isDefault: true,
    parseArgument: result =>
    {
        if (result.Tokens.Count == 0)
        {
            return new FileInfo("sampleQuotes.txt");

        }
        string? filePath = result.Tokens.Single().Value;
        if (File.Exists(filePath)) return new FileInfo(filePath);

        result.ErrorMessage = "File does not exist";
        return null;
    });

var stampCommand = new Command("stamp", "Stamps the specified time.");
stampCommand.AddArgument(timeArgument);
stampCommand.AddOption(reasonOption);
stampCommand.AddOption(inOption);
stampCommand.AddOption(outOption);
stampCommand.AddOption(atOption);

var listStampsCommand = new Command(name: "list", description: "List all stamps for a specific day");
listStampsCommand.AddAlias("ls");
var todayOption = new Option<bool>(name: "--today");
var timeForListArgument = new Argument<CustomDateTimeArgument?>(
    name: "time",
    description: "The time you want to stamp.",
    parse: input => input.Tokens.Any() ? new CustomDateTimeArgument(input.Tokens.Single().Value) : null);
listStampsCommand.AddArgument(timeForListArgument);
listStampsCommand.SetHandler(async (ListStampsModel listModel, FileInfo? file) =>
{
    var stamps = await stampService.GetStamps(listModel.Today ? DateTime.Today : listModel.Day, file);
    if (stamps == null)
    {
        Console.WriteLine("Can't read stamps.");
        return;
    }
    foreach (var stamp in stamps)
    {
        Console.WriteLine(stamp.ToString());
    }
}, new ListStampsModelBinder(todayOption, timeForListArgument), fileOption);
stampCommand.AddCommand(listStampsCommand);

rootCommand.AddCommand(stampCommand);
rootCommand.AddOption(fileOption);



stampCommand.SetHandler(async (CustomDateTimeArgument time, TimeOnly? at, string? reason, FileInfo? file, bool stampIn, bool stampOut) =>
{
    await stampService.Stamp(time, at, reason, file, stampIn, stampOut);
},
    timeArgument, atOption, reasonOption, fileOption, inOption, outOption);

var parser = new CommandLineBuilder(rootCommand).UseExceptionHandler((e, context) => Console.WriteLine(e.Message), errorExitCode: 1).Build();

return await rootCommand.InvokeAsync(args);

