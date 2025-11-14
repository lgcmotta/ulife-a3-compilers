using JetBrains.Annotations;

namespace Msh.Interpreter.Definitions;

[UsedImplicitly]
internal sealed record ParameterDefinition(string Type, string Name);