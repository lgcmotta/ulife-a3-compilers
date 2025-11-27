namespace Msh.Interpreter.Tests.Visitors;

public class MShellVisitorIoTests
{
    [Fact]
    public void Read_ShouldParseBoolCaseInsensitive()
    {
        // Arrange
        const string code = """
                            var flag = Read();

                            if (flag)
                            {
                                Write("ok");
                            }
                            """;


        var (visitor, console) = MShellVisitorFixture.CreateVisitorWithConsole();

        console.Interactive();
        console.Input.PushText("TrUe");
        console.Input.PushKey(ConsoleKey.Enter);

        var parser = code.CreateParserFromInput();

        // Act
        visitor.VisitProg(parser.prog());

        // Assert
        console.Output.ShouldContain("ok");
    }

    [Fact]
    public void Write_ShouldConcatenateArguments()
    {
        // Arrange
        const string code = """
                            Write("Hello", 1, "!", true);
                            """;


        var parser = code.CreateParserFromInput();

        // Act
        var (visitor, console) = MShellVisitorFixture.CreateVisitorWithConsole();

        visitor.VisitProg(parser.prog());

        // Assert
        console.Output.ShouldBe("Hello1!true\n");
    }
}