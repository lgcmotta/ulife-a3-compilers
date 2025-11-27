namespace Msh.Interpreter.Tests.Visitors;

public class MShellVisitorListTests
{
    [Fact]
    public void Insert_IndexEqualCount_ShouldThrowOutOfRange()
    {
        // Arrange
        const string code = """
                            int[] nums = [1, 2];
                            nums.Insert(2, 3);
                            """;

        var parser = code.CreateParserFromInput();

        // Act
        var visitor = MShellVisitorFixture.CreateVisitor();

        var exception = Should.Throw<InvalidOperationException>(() => visitor.VisitProg(parser.prog()));

        // Assert
        exception.Message.ShouldBe("List index is out of range.");
    }

    [Fact]
    public void MultiDimensionalIndex_OnNonList_ShouldThrow()
    {
        // Arrange
        const string code = """
                            int number = 1;
                            number[0][0] = 2;
                            """;


        var parser = code.CreateParserFromInput();

        // Act
        var visitor = MShellVisitorFixture.CreateVisitor();

        var exception = Should.Throw<InvalidOperationException>(() => visitor.VisitProg(parser.prog()));

        // Assert
        exception.Message.ShouldBe("Attempted to index a non-list value.");
    }

    [Fact]
    public void Insert_WithMismatchedType_ShouldThrow()
    {
        // Arrange
        const string code = """
                            int[] nums = [1, 2];
                            nums.Insert(1, "oops");
                            """;

        var parser = code.CreateParserFromInput();

        // Act
        var visitor = MShellVisitorFixture.CreateVisitor();

        var exception = Should.Throw<InvalidOperationException>(() => visitor.VisitProg(parser.prog()));

        // Assert
        exception.Message.ShouldBe("Cannot assign type 'string' to 'int'.");
    }

    [Fact]
    public void Add_ShouldAppendAndRespectType()
    {
        // Arrange
        const string code = """
                            int[] nums = [1];
                            nums.Add(2);
                            Write(nums.Size());
                            """;


        var (visitor, console) = MShellVisitorFixture.CreateVisitorWithConsole();

        var parser = code.CreateParserFromInput();

        // Act
        visitor.VisitProg(parser.prog());

        // Assert
        console.Output.ShouldBe("2\n");
    }

    [Fact]
    public void RemoveAt_ShouldRemoveElement()
    {
        // Arrange
        const string code = """
                            int[] nums = [1, 2, 3];
                            nums.RemoveAt(1);
                            Write(nums.Size());
                            """;


        var (visitor, console) = MShellVisitorFixture.CreateVisitorWithConsole();

        var parser = code.CreateParserFromInput();

        // Act
        visitor.VisitProg(parser.prog());

        // Assert
        console.Output.ShouldBe("2\n");
    }

    [Fact]
    public void Clear_ShouldEmptyList()
    {
        // Arrange
        const string code = """
                            int[] nums = [1, 2];
                            nums.Clear();
                            Write(nums.Size());
                            """;


        var (visitor, console) = MShellVisitorFixture.CreateVisitorWithConsole();

        var parser = code.CreateParserFromInput();

        // Act
        visitor.VisitProg(parser.prog());

        // Assert
        console.Output.ShouldBe("0\n");
    }

    [Fact]
    public void IndexerRead_OutOfRange_ShouldThrow()
    {
        // Arrange
        const string code = """
                            int[] nums = [1];
                            var x = nums[5];
                            """;


        var parser = code.CreateParserFromInput();

        // Act
        var visitor = MShellVisitorFixture.CreateVisitor();

        var exception = Should.Throw<InvalidOperationException>(() => visitor.VisitProg(parser.prog()));

        // Assert
        exception.Message.ShouldBe("List index is out of range.");
    }
}