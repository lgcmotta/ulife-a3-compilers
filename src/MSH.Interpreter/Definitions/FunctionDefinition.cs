using Antlr4BuildTasks;

namespace Msh.Interpreter.Definitions;

internal sealed record FunctionDefinition(string ReturnType, IReadOnlyList<ParameterDefinition> Parameters, MShellParser.BlockContext Body);