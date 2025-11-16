using System.Text;

using Antlr4.Runtime;

using Antlr4BuildTasks;

using Cocona;

using MSH.CLI.Terminals;

using Msh.Interpreter.Visitors;

using Spectre.Console;

namespace MSH.CLI.Commands;

internal static class REPL
{
    internal static CoconaApp AddREPLCommand(this CoconaApp app)
    {
        app.AddCommand(name: "repl", async Task<long> (ConsoleTerminal terminal, CoconaAppContext ctx) =>
        {
            terminal.WriteLine("Welcome to [yellow]msh[/] REPL ([green]Read, Eval, Print and Loop!)[/]");

            var visitor = new MShellVisitor(terminal);

            var buffer = new StringBuilder();

            while (!ctx.CancellationToken.IsCancellationRequested)
            {
                var line = await terminal.Console.PromptAsync(new TextPrompt<string?>("[yellow]>[/] "), ctx.CancellationToken);

                buffer.AppendLine(line);

                var braceBalance = 0;

                char lastNonWhitespace = '\0';

                var chunks = buffer.GetChunks();

                while (chunks.MoveNext())
                {
                    ReadOnlySpan<char> span = chunks.Current.Span;

                    foreach (var ch in span)
                    {
                        if (ch == '{')
                        {
                            braceBalance++;
                            continue;
                        }

                        if (ch == '}')
                        {
                            braceBalance--;
                            continue;
                        }

                        if (!char.IsWhiteSpace(ch))
                        {
                            lastNonWhitespace = ch;
                        }
                    }
                }

                var statementComplete = braceBalance <= 0 && lastNonWhitespace is ';' or '}';

                if (!statementComplete)
                {
                    continue;
                }

                var content = buffer.ToString();

                buffer.Clear();

                var input = new AntlrInputStream(content.ReplaceLineEndings(string.Empty));

                var lexer = new MShellLexer(input) { ErrorListeners = { ConsoleErrorListener<int>.Instance } };

                var tokens = new CommonTokenStream(lexer);

                var parser = new MShellParser(tokens) { ErrorListeners = { ConsoleErrorListener<IToken>.Instance } };

                var tree = parser.item();

                var variant = visitor.Visit(tree);

                terminal.WriteLine($"[green]{variant}[/]");
            }

            terminal.WriteLine($"[green]Exit Code: {0L}[/]");

            return 0L;
        });

        return app;
    }
}