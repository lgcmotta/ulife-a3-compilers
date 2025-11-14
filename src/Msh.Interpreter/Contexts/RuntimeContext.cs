using Msh.Interpreter.Registries;
using Msh.Interpreter.Scopes;

namespace Msh.Interpreter.Contexts;

internal class RuntimeContext
{
    internal VariablesRegistry Variables { get; } = [];

    internal FunctionsRegistry Functions { get; } = [];

    internal StatementScope Statements { get; } = [];

    internal ExecutionScope Execution { get; } = [];
}