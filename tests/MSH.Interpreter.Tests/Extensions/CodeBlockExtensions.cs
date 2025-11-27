namespace Msh.Interpreter.Tests.Extensions;

internal static class CodeBlockExtensions
{
    extension(string content)
    {
        internal MShellParser CreateParserFromInput()
        {
            var input = new AntlrInputStream(content.ReplaceLineEndings(string.Empty));

            var lexer = new MShellLexer(input) { ErrorListeners = { ConsoleErrorListener<int>.Instance } };

            var tokens = new CommonTokenStream(lexer);

            return new MShellParser(tokens) { ErrorListeners = { ConsoleErrorListener<IToken>.Instance } };
        }
    }
}