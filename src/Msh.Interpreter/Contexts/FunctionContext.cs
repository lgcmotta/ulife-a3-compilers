namespace Msh.Interpreter.Contexts;

internal sealed class FunctionContext : Stack<string>
{
    public Stack<string> Functions { get; set; }

    public ExecutionContext Execution { get; set; }
}