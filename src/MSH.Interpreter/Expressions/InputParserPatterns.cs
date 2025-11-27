using System.Text.RegularExpressions;

using JetBrains.Annotations;

namespace Msh.Interpreter.Expressions;

[UsedImplicitly]
internal partial class InputParserPatterns
{
    protected InputParserPatterns()
    { }

    internal static readonly Regex LongPattern = CreateLongPattern();
    internal static readonly Regex DoublePattern = CreateDoublePattern();
    internal static readonly Regex DecimalPattern = CreateDecimalPattern();

    [GeneratedRegex("^[0-9]+$", RegexOptions.Compiled)]
    private static partial Regex CreateLongPattern();

    [GeneratedRegex("^[0-9]+\\.[0-9]+([dD])?$", RegexOptions.Compiled)]
    private static partial Regex CreateDoublePattern();

    [GeneratedRegex("^[0-9]+\\.[0-9]+[mM]$", RegexOptions.Compiled)]
    private static partial Regex CreateDecimalPattern();
}