using Msh.Interpreter.Slots;

namespace Msh.Interpreter.Scopes;

internal sealed class Scope() : Dictionary<string, VariableSlot>(StringComparer.OrdinalIgnoreCase);