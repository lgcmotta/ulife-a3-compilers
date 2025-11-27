namespace Msh.Interpreter.Tests.Visitors;

public class MShellVisitorExpressionTests
{
    [Fact]
    public void ArithmeticPrecedence_ShouldRespectMultiplicationOverAddition()
    {
        // Arrange
        const string code = """
                            var result = 2 + 3 * 4;
                            Write(result);
                            """;


        var parser = code.CreateParserFromInput();

        // Act
        var (visitor, console) = MShellVisitorFixture.CreateVisitorWithConsole();

        visitor.VisitProg(parser.prog());

        // Assert
        console.Output.ShouldBe("14\n");
    }

    [Fact]
    public void PowOperator_ShouldBeRightAssociative()
    {
        // Arrange
        const string code = """
                            var value = 2 ^ 3 ^ 2;
                            Write(value);
                            """;

        var parser = code.CreateParserFromInput();

        // Act
        var (visitor, console) = MShellVisitorFixture.CreateVisitorWithConsole();

        visitor.VisitProg(parser.prog());

        // Assert
        console.Output.ShouldBe("512\n");
    }

    [Fact]
    public void Ternary_ShouldUseComparisonAsCondition()
    {
        // Arrange
        const string code = """
                            var x = 1;
                            var y = x > 10 ? "Hello" : "World";
                            Write(y);
                            """;


        var parser = code.CreateParserFromInput();

        // Act
        var (visitor, console) = MShellVisitorFixture.CreateVisitorWithConsole();

        visitor.VisitProg(parser.prog());

        // Assert
        console.Output.ShouldBe("World\n");
    }

    [Fact]
    public void UnaryMinus_OnBool_ShouldInvert()
    {
        // Arrange
        const string code = """
                            bool flag = true;
                            Write(-flag);
                            """;


        var parser = code.CreateParserFromInput();

        // Act
        var (visitor, console) = MShellVisitorFixture.CreateVisitorWithConsole();

        visitor.VisitProg(parser.prog());

        // Assert
        console.Output.ShouldBe("false\n");
    }

    [Fact]
    public void UnaryMinus_OnString_ShouldReverse()
    {
        // Arrange
        const string code = """
                            string text = "abc";
                            Write(-text);
                            """;


        var parser = code.CreateParserFromInput();

        // Act
        var (visitor, console) = MShellVisitorFixture.CreateVisitorWithConsole();

        visitor.VisitProg(parser.prog());

        // Assert
        console.Output.ShouldBe("cba\n");
    }
}