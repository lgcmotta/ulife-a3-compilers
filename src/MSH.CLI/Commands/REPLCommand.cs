using System.Text;

using Antlr4.Runtime;

using Antlr4BuildTasks;

using Cocona;

using Msh.CommandLineInterface.Extensions;
using Msh.CommandLineInterface.Terminals;
using Msh.Interpreter.Listeners;
using Msh.Interpreter.Visitors;
using Msh.StandardLibrary.Exceptions;

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
                terminal.WriteInstructions();

                var visitor = new MShellVisitor(terminal);
                var listener = new MShellErrorListener(terminal);

                var buffer = new StringBuilder();

                while (!ctx.CancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var line = await terminal.PromptLineAsync(ctx.CancellationToken);

                        buffer.AppendLine(line);

                        if (buffer.IsExitingREPL())
                        {
                            break;
                        }

                        if (buffer.IsCleaningTerminal())
                        {
                            terminal.Console.Clear();
                            buffer.Clear();
                            terminal.WriteWelcomeMessage();
                            terminal.WriteInstructions();
                            continue;
                        }

                        if (!buffer.IsStatementComplete())
                        {
                            continue;
                        }

                        var content = buffer.ToString();

                        buffer.Clear();

                        var input = new AntlrInputStream(content.ReplaceLineEndings(string.Empty));

                        var lexer = new MShellLexer(input);
                        lexer.RemoveErrorListeners();
                        lexer.AddErrorListener(listener);

                        var tokens = new CommonTokenStream(lexer);

                        var parser = new MShellParser(tokens);
                        parser.RemoveErrorListeners();
                        parser.AddErrorListener(listener);

                        var tree = parser.item();

                        var variant = visitor.Visit(tree);

                        terminal.WriteVariant(variant);
                    }
                    catch (Exception exception) when (exception is InvalidOperationException or InvalidVariantCastException)
                    {
                        terminal.Console.WriteException(exception, ExceptionFormats.ShortenPaths | ExceptionFormats.ShortenTypes |
                                                                   ExceptionFormats.ShortenMethods | ExceptionFormats.ShowLinks);
                    }
                    catch (Exception exception) when (exception is not OperationCanceledException)
                    {
                        // pass
                    }
                    catch (OperationCanceledException)
                    {
                        return 0L;
                    }
                }

                return 0L;
            });

            return app;
        }
    }
}