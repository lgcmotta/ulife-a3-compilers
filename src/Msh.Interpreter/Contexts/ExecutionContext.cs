using Msh.Interpreter.Scopes;

namespace Msh.Interpreter.Contexts;

internal sealed class ExecutionContext : Stack<Scope>
{
    internal void PushEmptyScope() => Push([]);
}