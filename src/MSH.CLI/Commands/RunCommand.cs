using Antlr4.Runtime;

using Antlr4BuildTasks;

using Cocona;

using JetBrains.Annotations;

using Msh.CommandLineInterface.Extensions;
using Msh.CommandLineInterface.Terminals;
using Msh.Interpreter.Visitors;

using Spectre.Console;

namespace Msh.CommandLineInterface.Commands;

[UsedImplicitly]
internal record RunCommandOptions([Option("file", shortNames: ['f'])] string File) : ICommandParameterSet;

internal static class RunCommand
{
    extension(CoconaApp app)
    {
        internal CoconaApp AddRunCommand()
        {
            app.AddCommand(name: "run", async Task<long> (RunCommandOptions options, ConsoleTerminal terminal, CoconaAppContext ctx) =>
            {
                try
                {
                    var extension = Path.GetExtension(options.File);

                    if (extension is not ".ms")
                    {
                        terminal.Console.MarkupInterpolated($"[red]Received file with extension '{extension}', but msh requires a '.ms' file extension[/]");

                        return 1L;
                    }

                    var file = Path.GetFullPath(options.File);

                    if (!File.Exists(file))
                    {
                        terminal.Console.MarkupInterpolated($"[red]Input file does not exists at '{file}'[/]");

                        return 1L;
                    }

                    var content = await File.ReadAllTextAsync(file, ctx.CancellationToken);

                    terminal.WriteLine($"[yellow]Parsing program '{file}'...[/]");

                    var visitor = new MShellVisitor(terminal);

                    var input = new AntlrInputStream(content.ReplaceLineEndings(string.Empty));

                    var lexer = new MShellLexer(input) { ErrorListeners = { ConsoleErrorListener<int>.Instance } };

                    var tokens = new CommonTokenStream(lexer);

                    var parser = new MShellParser(tokens) { ErrorListeners = { ConsoleErrorListener<IToken>.Instance } };

                    var tree = parser.prog();

                    var result = visitor.Visit(tree);

                    terminal.WriteVariant(result);

                    return 0L;
                }
                catch (Exception exception) when (exception is not OperationCanceledException)
                {
                    terminal.Console.WriteException(exception, ExceptionFormats.ShortenEverything);

                    return 1L;
                }
                catch (OperationCanceledException)
                {
                    return 0L;
                }
            });

            return app;
        }
    }
}