using Msh.CommandLineInterface.Terminals;
using Msh.StandardLibrary.Types;

using Spectre.Console;

namespace Msh.CommandLineInterface.Extensions;

internal static class ConsoleTerminalExtensions
{
    extension(ConsoleTerminal terminal)
    {
        internal void WriteWelcomeMessage()
        {
            terminal.WriteLine("Welcome to [yellow]msh[/] REPL ([green]Read, Eval, Print and Loop!)[/]");
        }

        internal void WriteInstructions()
        {
            terminal.Console.MarkupLine("Type [yellow]clear[/] to clean the terminal.");
            terminal.Console.MarkupLine("Type [red]exit[/] to exit the quit REPL mode.");
        }

        internal async Task<string?> PromptLineAsync(CancellationToken cancellationToken = default)
        {
            return await terminal.Console.PromptAsync(new TextPrompt<string>("[yellow]>[/]"), cancellationToken: cancellationToken);
        }

        internal void WriteVariant(IVariant variant)
        {
            terminal.WriteLine($"[green]{variant}[/]");
        }
    }
}