using Antlr4BuildTasks;

namespace Msh.Interpreter.Scopes;

internal class StatementScope : Queue<MShellParser.StatementContext>;