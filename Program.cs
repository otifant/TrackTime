using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

// Tutorial: https://learn.microsoft.com/en-us/dotnet/standard/commandline/get-started-tutorial
Console.WriteLine("Hello, World!");
Console.WriteLine(new List<int>().GetType());


var rootCommand = new RootCommand();
var userOption = new Option<string>("--user", "");
var lightModeOption = new Option<bool>(
    description: "Background color of text displayed on the console: default is black, light mode is white.",
    name: "--light-mode");
var fileOption = new Option<FileInfo?>(
    name: "--file",
    description: "The file to read and display on the console.");

// rootCommand.AddOption(userOption);

Command readCommand = new("read", "Read and display the file"){
    userOption, lightModeOption, fileOption
};
rootCommand.AddCommand(readCommand);

readCommand.SetHandler(async (file, user, lightMode) => { await ReadFile(file, lightMode); }, fileOption, userOption, lightModeOption);
// rootCommand.SetHandler((user) =>
// {
//     Console.WriteLine($"Assigned it to user {user}.");
// }, userOption);

static async Task ReadFile(
        FileInfo file, bool lightMode)
{
    Console.BackgroundColor = lightMode ? ConsoleColor.White : ConsoleColor.Black;
    List<string> lines = File.ReadLines(file.FullName).ToList();
    foreach (string line in lines)
    {
        Console.WriteLine(line);
        await Task.Delay(2 * line.Length);
    };
}

return await rootCommand.InvokeAsync(args);