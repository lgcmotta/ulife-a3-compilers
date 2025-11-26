using Cocona;

using Msh.CommandLineInterface.Commands;
using Msh.CommandLineInterface.Extensions;

var builder = CoconaApp.CreateBuilder(args);

builder.Services.AddAnsiConsole();
builder.Services.AddConsoleTerminal();

var app = builder.Build();

app.AddRunCommand();
app.AddREPLCommand();

await app.RunAsync();