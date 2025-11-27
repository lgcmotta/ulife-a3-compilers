namespace Msh.Interpreter.Tests.Visitors;

public class MShellVisitorFunctionTests
{
    [Fact]
    public void FunctionArityMismatch_ShouldThrow()
    {
        // Arrange
        const string code = """
                            int Echo(int value)
                            {
                                return value;
                            }

                            Echo(1, 2);
                            """;


        var parser = code.CreateParserFromInput();

        // Act
        var visitor = MShellVisitorFixture.CreateVisitor();

        var exception = Should.Throw<InvalidOperationException>(() => visitor.VisitProg(parser.prog()));

        // Assert
        exception.Message.ShouldBe("Function 'Echo' expects 1 argument(s) but received 2.");
    }

    [Fact]
    public void UndefinedFunction_ShouldThrow()
    {
        // Arrange
        const string code = """
                            Foo();
                            """;


        var parser = code.CreateParserFromInput();

        // Act
        var visitor = MShellVisitorFixture.CreateVisitor();

        var exception = Should.Throw<InvalidOperationException>(() => visitor.VisitProg(parser.prog()));

        // Assert
        exception.Message.ShouldBe("Function 'Foo' is not defined.");
    }

    [Fact]
    public void NonVoidFunction_ReturnsValue()
    {
        // Arrange
        const string code = """
                            int SumNumbers(int a, int b)
                            {
                                return a + b;
                            }

                            Write(SumNumbers(2, 3));
                            """;


        var (visitor, console) = MShellVisitorFixture.CreateVisitorWithConsole();

        var parser = code.CreateParserFromInput();

        // Act
        visitor.VisitProg(parser.prog());

        // Assert
        console.Output.ShouldBe("5\n");
    }

    [Fact]
    public void VoidFunction_ReturnsZeroWhenUsedInExpression()
    {
        // Arrange
        const string code = """
                            void Noop()
                            {
                                return;
                            }

                            Write(Noop());
                            """;


        var (visitor, console) = MShellVisitorFixture.CreateVisitorWithConsole();

        var parser = code.CreateParserFromInput();

        // Act
        visitor.VisitProg(parser.prog());

        // Assert
        console.Output.ShouldBe("0\n");
    }

    [Fact]
    public void VoidFunctionReturningValue_StillReturnsZero()
    {
        // Arrange
        const string code = """
                            void Noop()
                            {
                                return 5;
                            }

                            Write(Noop());
                            """;


        var (visitor, console) = MShellVisitorFixture.CreateVisitorWithConsole();

        var parser = code.CreateParserFromInput();

        // Act
        visitor.VisitProg(parser.prog());

        // Assert
        console.Output.ShouldBe("0\n");
    }
}