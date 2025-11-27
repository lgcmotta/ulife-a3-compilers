namespace Msh.Interpreter.Tests.Visitors;

public class MShellVisitorStringInterpolationTests
{
    [Fact]
    public void Interpolation_ShouldReplaceVariableValues()
    {
        // Arrange
        const string code = """
                            int x = 5;
                            Write("Value: {x}");
                            """;

        var parser = code.CreateParserFromInput();

        // Act
        var (visitor, console) = MShellVisitorFixture.CreateVisitorWithConsole();

        visitor.VisitProg(parser.prog());

        // Assert
        console.Output.ShouldBe("Value: 5\n");
    }

    [Fact]
    public void Interpolation_WithPrecision_ShouldFormatNumeric()
    {
        // Arrange
        const string code = """
                            double pi = 3.14159d;
                            Write("{pi:2}");
                            """;

        var parser = code.CreateParserFromInput();

        // Act
        var (visitor, console) = MShellVisitorFixture.CreateVisitorWithConsole();

        visitor.VisitProg(parser.prog());

        // Assert
        console.Output.ShouldBe("3.14\n");
    }

    [Fact]
    public void Interpolation_EmptyPlaceholder_ShouldThrow()
    {
        // Arrange
        const string code = """
                            Write("{}");
                            """;

        var parser = code.CreateParserFromInput();

        // Act
        var visitor = MShellVisitorFixture.CreateVisitor();

        var exception = Should.Throw<InvalidOperationException>(() => visitor.VisitProg(parser.prog()));

        // Assert
        exception.Message.ShouldBe("When using string interpolation an expression is required between the brackets.");
    }

    [Fact]
    public void Interpolation_IndexOutOfRange_ShouldThrow()
    {
        // Arrange
        const string code = """
                            int[] nums = [1];
                            Write("{nums[1]}");
                            """;

        var parser = code.CreateParserFromInput();

        // Act
        var visitor = MShellVisitorFixture.CreateVisitor();

        var exception = Should.Throw<InvalidOperationException>(() => visitor.VisitProg(parser.prog()));

        // Assert
        exception.Message.ShouldBe("Index 1 is out of range for 'nums'.");
    }

    [Fact]
    public void Interpolation_InvalidPrecision_ShouldThrow()
    {
        // Arrange
        const string code = """
                            double value = 1.23d;
                            Write("{value:bad}");
                            """;

        var parser = code.CreateParserFromInput();

        // Act
        var visitor = MShellVisitorFixture.CreateVisitor();

        var exception = Should.Throw<InvalidOperationException>(() => visitor.VisitProg(parser.prog()));

        // Assert
        exception.Message.ShouldBe("Invalid precision specifier 'bad'.");
    }
}