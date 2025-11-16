using Msh.Interpreter.Scopes;

namespace Msh.Interpreter.Registries;

internal class VariablesRegistry : Stack<VariablesScope>;