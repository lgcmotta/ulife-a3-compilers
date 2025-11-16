using JetBrains.Annotations;

using Msh.Interpreter.Abstractions;

using Spectre.Console;

namespace MSH.CLI.Terminals;

[UsedImplicitly]
public class ConsoleTerminal : ITerminal
{
    internal IAnsiConsole Console { get; }

    public ConsoleTerminal(IAnsiConsole console)
    {
        Console = console;
    }

    public void WriteLine(FormattableString value)
    {
        Console.MarkupInterpolated(value);
    }

    public void WriteLine(string value)
    {
        Console.MarkupLine(value);
    }

    public string ReadLine()
    {
        return Console.Prompt(new TextPrompt<string>(string.Empty));
    }
}