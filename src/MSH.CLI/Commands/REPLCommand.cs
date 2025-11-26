using System.Text;

using Antlr4.Runtime;

using Antlr4BuildTasks;

using Cocona;

using Msh.CommandLineInterface.Extensions;
using Msh.CommandLineInterface.Terminals;
using Msh.Interpreter.Visitors;

using Spectre.Console;

namespace Msh.CommandLineInterface.Commands;

internal static class REPLCommand
{
    extension(CoconaApp app)
    {
        internal CoconaApp AddREPLCommand()
        {
            app.AddCommand(name: "repl", async Task<long> (ConsoleTerminal terminal, CoconaAppContext ctx) =>
            {
                terminal.WriteWelcomeMessage();

                var visitor = new MShellVisitor(terminal);

                var buffer = new StringBuilder();

                try
                {
                    while (!ctx.CancellationToken.IsCancellationRequested)
                    {
                        var line = await terminal.PromptLineAsync(ctx.CancellationToken);

                        buffer.AppendLine(line);

                        if (!buffer.IsStatementComplete())
                        {
                            continue;
                        }

                        var content = buffer.ToString();

                        buffer.Clear();

                        // if (content is )

                        var input = new AntlrInputStream(content.ReplaceLineEndings(string.Empty));

                        var lexer = new MShellLexer(input) { ErrorListeners = { ConsoleErrorListener<int>.Instance } };

                        var tokens = new CommonTokenStream(lexer);

                        var parser = new MShellParser(tokens) { ErrorListeners = { ConsoleErrorListener<IToken>.Instance } };

                        var tree = parser.item();

                        var variant = visitor.Visit(tree);

                        terminal.WriteVariant(variant);
                    }

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