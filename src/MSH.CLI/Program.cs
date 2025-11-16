using Cocona;

using MSH.CLI.Commands;
using MSH.CLI.Terminals;

using Msh.Interpreter.Abstractions;

using Spectre.Console;

var builder = CoconaApp.CreateBuilder(args);

builder.Services.AddSingleton<IAnsiConsole>(_ => AnsiConsole.Console);
builder.Services.AddSingleton<ITerminal, ConsoleTerminal>(provider => new ConsoleTerminal(provider.GetRequiredService<IAnsiConsole>()));
builder.Services.AddSingleton<ConsoleTerminal>(provider => new ConsoleTerminal(provider.GetRequiredService<IAnsiConsole>()));

var app = builder.Build();

app.AddREPLCommand();

await app.RunAsync();