namespace Msh.Interpreter.Tests.Visitors;

public class MShellVisitorVariableTests
{
    [Fact]
    public void VoidVariableDeclaration_ShouldThrow()
    {
        // Arrange
        const string code = "void x;";

        var parser = code.CreateParserFromInput();

        // Act
        var visitor = MShellVisitorFixture.CreateVisitor();

        var exception = Should.Throw<InvalidOperationException>(() => visitor.VisitProg(parser.prog()));

        // Assert
        exception.Message.ShouldBe("Variables cannot be void.");
    }

    [Fact]
    public void VarInferenceFromEmptyList_ShouldThrow()
    {
        // Arrange
        const string code = "var items = [];";


        var parser = code.CreateParserFromInput();

        // Act
        var visitor = MShellVisitorFixture.CreateVisitor();

        var exception = Should.Throw<InvalidOperationException>(() => visitor.VisitProg(parser.prog()));

        // Assert
        exception.Message.ShouldBe("Cannot infer type from an empty list.");
    }

    [Fact]
    public void ExplicitIntDefault_ShouldBeZero()
    {
        // Arrange
        const string code = """
                            int x;
                            Write(x);
                            """;


        var (visitor, console) = MShellVisitorFixture.CreateVisitorWithConsole();

        var parser = code.CreateParserFromInput();

        // Act
        visitor.VisitProg(parser.prog());

        // Assert
        console.Output.ShouldBe("0\n");
    }

    [Fact]
    public void VarInferenceFromNestedList_ShouldInferElementType()
    {
        // Arrange
        const string code = """
                            var matrix = [[1], [2]];
                            Write(matrix[0][0]);
                            """;


        var (visitor, console) = MShellVisitorFixture.CreateVisitorWithConsole();

        var parser = code.CreateParserFromInput();

        // Act
        visitor.VisitProg(parser.prog());

        // Assert
        console.Output.ShouldBe("1\n");
    }
}