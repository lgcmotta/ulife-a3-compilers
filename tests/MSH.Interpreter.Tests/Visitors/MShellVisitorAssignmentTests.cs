namespace Msh.Interpreter.Tests.Visitors;

public class MShellVisitorAssignmentTests
{
    [Fact]
    public void StringPlusEquals_Numeric_ShouldThrow()
    {
        // Arrange
        const string code = """
                            string name = "A";
                            name += 1;
                            """;


        var parser = code.CreateParserFromInput();

        // Act
        var visitor = MShellVisitorFixture.CreateVisitor();

        var exception = Should.Throw<InvalidOperationException>(() => visitor.VisitProg(parser.prog()));

        // Assert
        exception.Message.ShouldBe("Compound assignment '+=' requires numeric or string types.");
    }

    [Fact]
    public void PrefixIncrement_OnString_ShouldThrow()
    {
        // Arrange
        const string code = """
                            string text = "hi";
                            ++text;
                            """;


        var parser = code.CreateParserFromInput();

        // Act
        var visitor = MShellVisitorFixture.CreateVisitor();

        var exception = Should.Throw<InvalidOperationException>(() => visitor.VisitProg(parser.prog()));

        // Assert
        exception.Message.ShouldBe("Unary operators require numeric types.");
    }
}