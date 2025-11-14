using Antlr4BuildTasks;

using Msh.Interpreter.Abstractions;
using Msh.Interpreter.Contexts;
using Msh.StandardLibrary.Types;

namespace Msh.Interpreter.Visitors;

public sealed partial class MShellVisitor : MShellBaseVisitor<IVariant>
{
    private readonly ITerminal _terminal;

    private readonly RuntimeContext _context = new();

    public MShellVisitor(ITerminal terminal)
    {
        _terminal = terminal;
    }
}