namespace Msh.Interpreter.Tests.Fixtures;

public static class MShellVisitorFixture
{
    public static MShellVisitor CreateVisitor() => new(new ConsoleTerminal(new TestConsole()));

    public static (MShellVisitor Visitor, TestConsole Console) CreateVisitorWithConsole()
    {
        var console = new TestConsole();

        var visitor = new MShellVisitor(new ConsoleTerminal(console));

        return (visitor, console);
    }
}