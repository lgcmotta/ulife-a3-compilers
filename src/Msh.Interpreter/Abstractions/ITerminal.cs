using System.Runtime.CompilerServices;

namespace Msh.Interpreter.Abstractions;

public interface ITerminal
{
    [OverloadResolutionPriority(1)]
    void WriteLine(string value);

    [OverloadResolutionPriority(2)]
    void WriteLine(FormattableString value);

    string ReadLine();
}