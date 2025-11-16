using System.Runtime.CompilerServices;

namespace Msh.StandardLibrary.Types;

public delegate IVariant Function(IVariant[] args);

internal readonly ref struct FunctionHelper
{
    internal static Function EnsureArgumentCount(int count, Function body, [CallerArgumentExpression(nameof(body))] string? name = null)
    {
        return args => args.Length == count
            ? body(args)
            : throw new InvalidOperationException($"Function '{name}' expects {count} argument(s) but received {args.Length}.");
    }
}