using Msh.Interpreter.Definitions;

namespace Msh.Interpreter.Scopes;

internal class VariablesScope() : Dictionary<string, VariableDefinition>(StringComparer.CurrentCultureIgnoreCase);