namespace Msh.Interpreter.Abstractions;

public interface ITerminal
{
    void WriteLine(string value);

    string ReadLine();
}