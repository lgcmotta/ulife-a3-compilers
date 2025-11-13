using System.Runtime.CompilerServices;

namespace Msh.StandardLibrary.Mathematics;

internal readonly ref struct MathFunctionArgumentsHelper
{
    internal static MathFunction EnsureArgumentCount(int count, MathFunction body, [CallerArgumentExpression(nameof(body))] string? name = null)
    {
        return args => args.Length == count
            ? body(args)
            : throw new InvalidOperationException($"Function '{name}' expects {count} argument(s) but received {args.Length}.");
    }
}