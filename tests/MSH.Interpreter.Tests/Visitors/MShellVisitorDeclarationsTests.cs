namespace Msh.Interpreter.Tests.Visitors;

public class MShellVisitorDeclarationsTests
{
    [Fact]
    public void VariableRedeclarationInSameScope_ShouldThrow()
    {
        // Arrange
        const string code = """
                            int x = 1;
                            int x = 2;
                            """;


        var parser = code.CreateParserFromInput();

        // Act
        var visitor = MShellVisitorFixture.CreateVisitor();

        var exception = Should.Throw<InvalidOperationException>(() => visitor.VisitProg(parser.prog()));

        // Assert
        exception.Message.ShouldBe("Identifier 'x' already exists in this scope.");
    }

    [Fact]
    public void IndexAssignment_OutOfRange_ShouldThrow()
    {
        // Arrange
        const string code = """
                            int[] nums = [1];
                            nums[1] = 2;
                            """;


        var parser = code.CreateParserFromInput();

        // Act
        var visitor = MShellVisitorFixture.CreateVisitor();

        var exception = Should.Throw<InvalidOperationException>(() => visitor.VisitProg(parser.prog()));

        // Assert
        exception.Message.ShouldBe("List index is out of range.");
    }

    [Fact]
    public void ListAdd_WithMismatchedType_ShouldThrow()
    {
        // Arrange
        const string code = """
                            int[] nums = [1];
                            nums.Add("oops");
                            """;


        var parser = code.CreateParserFromInput();

        // Act
        var visitor = MShellVisitorFixture.CreateVisitor();

        var exception = Should.Throw<InvalidOperationException>(() => visitor.VisitProg(parser.prog()));

        // Assert
        exception.Message.ShouldBe("Cannot assign type 'string' to 'int'.");
    }
}