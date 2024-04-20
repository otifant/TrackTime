using System.CommandLine;



var fileOption = new Option<FileInfo?>(
    name: "--file",
    description: "An option whose argument is parsed as a FileInfo",
    isDefault: true,
    parseArgument: result =>
    {
        if (result.Tokens.Count == 0)
        {
            return new FileInfo("sampleQuotes.txt");

        }
        string? filePath = result.Tokens.Single().Value;
        if (!File.Exists(filePath))
        {
            result.ErrorMessage = "File does not exist";
            return null;
        }
        else
        {
            return new FileInfo(filePath);
        }
    });

var quoteArgument = new Argument<string>(
    name: "quote",
    description: "Text of quote.");

var bylineArgument = new Argument<string>(
    name: "byline",
    description: "Byline of quote.");



var rootCommand = new RootCommand("Sample app for System.CommandLine");
rootCommand.AddGlobalOption(fileOption);





var addCommand = new Command("add", "Add an entry to the file.");
addCommand.AddArgument(quoteArgument);
addCommand.AddArgument(bylineArgument);
addCommand.AddAlias("insert");
rootCommand.AddCommand(addCommand);



addCommand.SetHandler((file, quote, byline) =>
    {
        AddToFile(file!, quote, byline);
    },
    fileOption, quoteArgument, bylineArgument);

return await rootCommand.InvokeAsync(args);

static void AddToFile(FileInfo file, string quote, string byline)
{
    Console.WriteLine("Adding to file");
    using StreamWriter? writer = file.AppendText();
    writer.WriteLine($"{Environment.NewLine}{Environment.NewLine}{quote}");
    writer.WriteLine($"{Environment.NewLine}-{byline}");
    writer.Flush();
}