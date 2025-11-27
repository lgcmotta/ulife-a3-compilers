namespace Msh.Interpreter.Tests.Visitors;

public class MShellVisitorScopeTests
{
    [Fact]
    public void Shadowing_ShouldNotLeakInnerValue()
    {
        // Arrange
        const string code = """
                            int x = 1;

                            void Show()
                            {
                                int x = 2;
                                Write(x);
                            }

                            Show();
                            Write(x);
                            """;


        var parser = code.CreateParserFromInput();

        // Act
        var (visitor, console) = MShellVisitorFixture.CreateVisitorWithConsole();

        visitor.VisitProg(parser.prog());

        // Assert
        console.Output.ShouldBe("2\n1\n");
    }
}