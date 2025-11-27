using Antlr4.Runtime;

using Msh.Interpreter.Abstractions;

namespace Msh.Interpreter.Listeners;

internal class MShellErrorListener(ITerminal terminal) : IAntlrErrorListener<IToken>, IAntlrErrorListener<int>
{
    private readonly ITerminal _terminal = terminal;

    public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e) =>
        WriteError(line, charPositionInLine, msg);

    public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e) =>
        WriteError(line, charPositionInLine, msg);

    private void WriteError(int line, int column, string message) => _terminal.WriteLine($"[red]Error at: {line}:{column} - {message}[/]");
}