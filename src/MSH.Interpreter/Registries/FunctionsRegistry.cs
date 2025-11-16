using Msh.Interpreter.Definitions;
using Msh.Interpreter.Scopes;

namespace Msh.Interpreter.Registries;

internal class FunctionsRegistry : Dictionary<string, FunctionDefinition>
{
    public FunctionScope Scope { get; } = [];
}