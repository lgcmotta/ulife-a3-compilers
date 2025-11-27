namespace Msh.Interpreter.Tests.Visitors;

public class MShellVisitorProgramTests
{
    [Fact]
    public void VisitProg_ResolvesFunctionDeclaredAfterUsage()
    {
        // Arrange
        const string code = """
                            var result = AddForward(2, 3);

                            int AddForward(int a, int b)
                            {
                                return a + b;
                            }

                            return result;
                            """;


        var parser = code.CreateParserFromInput();

        // Act
        var visitor = MShellVisitorFixture.CreateVisitor();

        var result = visitor.VisitProg(parser.prog());

        // Assert
        var value = result.ShouldBeOfType<LongType>();
        value.Value.ShouldBe(5);
    }

    [Fact]
    public void VisitProg_DuplicateFunctionDefinition_Throws()
    {
        // Arrange
        const string code = """
                            int Foo()
                            {
                                return 1;
                            }

                            int Foo()
                            {
                                return 2;
                            }
                            """;


        var parser = code.CreateParserFromInput();

        // Act
        var visitor = MShellVisitorFixture.CreateVisitor();

        var exception = Should.Throw<InvalidOperationException>(() => visitor.VisitProg(parser.prog()));

        // Assert
        exception.Message.ShouldBe("Function 'Foo' is already defined.");
    }
}