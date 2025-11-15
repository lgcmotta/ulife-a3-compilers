using System.Text.RegularExpressions;

using Antlr4BuildTasks;

using Msh.Interpreter.Abstractions;
using Msh.Interpreter.Contexts;
using Msh.StandardLibrary.Types;

namespace Msh.Interpreter.Visitors;

public sealed partial class MShellVisitor : MShellBaseVisitor<IVariant>
{
    private readonly ITerminal _terminal;
    private readonly RuntimeContext _context = new();

    private static readonly Regex InterpolationPattern = CreateInterpolationPattern();
    private static readonly Regex IndexPattern = CreateIndexPattern();

    public MShellVisitor(ITerminal terminal)
    {
        _terminal = terminal;
    }

    [GeneratedRegex(@"\[(\d+)\]", RegexOptions.Compiled)]
    private static partial Regex CreateIndexPattern();

    [GeneratedRegex(@"\{([^{}]+)\}", RegexOptions.Compiled)]
    private static partial Regex CreateInterpolationPattern();
}