namespace Msh.Interpreter.Tests.Visitors;

public class MShellVisitorControlFlowTests
{
    [Fact]
    public void WhileCondition_NotBool_ShouldThrow()
    {
        // Arrange
        const string code = """
                            int x = 1;
                            while (x)
                            {
                                x++;
                            }
                            """;


        var parser = code.CreateParserFromInput();

        // Act
        var visitor = MShellVisitorFixture.CreateVisitor();

        var exception = Should.Throw<InvalidOperationException>(() => visitor.VisitProg(parser.prog()));

        // Assert
        exception.Message.ShouldBe("The 'while' statement condition must evaluate to a bool.");
    }

    [Fact]
    public void ForCondition_NotBool_ShouldThrow()
    {
        // Arrange
        const string code = """
                            for (int i = 0; 1; i++)
                            {
                            }
                            """;


        var parser = code.CreateParserFromInput();

        // Act
        var visitor = MShellVisitorFixture.CreateVisitor();

        var exception = Should.Throw<InvalidOperationException>(() => visitor.VisitProg(parser.prog()));

        // Assert
        exception.Message.ShouldBe("The condition for the 'for' statement must evaluate to a bool.");
    }

    [Fact]
    public void ReturnOutsideFunction_ShouldThrow()
    {
        // Arrange
        const string code = """
                            return;
                            """;


        var parser = code.CreateParserFromInput();

        // Act
        var visitor = MShellVisitorFixture.CreateVisitor();

        var exception = Should.Throw<InvalidOperationException>(() => visitor.VisitProg(parser.prog()));

        // Assert
        exception.Message.ShouldBe("The keyword 'return' is only allowed inside a function.");
    }

    [Fact]
    public void ForWithoutCondition_ShouldNotExecuteBody()
    {
        // Arrange
        const string code = """
                            int counter = 0;

                            for (int i = 0; ; i++)
                            {
                                counter++;
                            }

                            Write(counter);
                            """;


        var visitor = MShellVisitorFixture.CreateVisitor();

        var parser = code.CreateParserFromInput();

        // Act
        var exception = Should.Throw<InvalidOperationException>(() => visitor.VisitProg(parser.prog()));

        // Assert
        exception.Message.ShouldBe("The condition for the 'for' statement must evaluate to a bool.");
    }

    [Fact]
    public void DoWhile_ExecutesAtLeastOnce()
    {
        // Arrange
        const string code = """
                            int counter = 0;

                            do
                            {
                                counter++;
                            }
                            while (false);

                            Write(counter);
                            """;


        var (visitor, console) = MShellVisitorFixture.CreateVisitorWithConsole();

        var parser = code.CreateParserFromInput();

        // Act
        visitor.VisitProg(parser.prog());

        // Assert
        console.Output.ShouldBe("1\n");
    }
}