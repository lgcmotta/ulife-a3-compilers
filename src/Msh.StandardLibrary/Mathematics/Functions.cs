namespace Msh.StandardLibrary.Mathematics;

using static MathFunctionArgumentsHelper;

public sealed class Functions : Dictionary<string, MathFunction>
{
    private static readonly Lazy<Functions> Instance = new(() => new Functions(), isThreadSafe: true);

    public static Functions Registry => Instance.Value;

    private Functions()
    {
        Add("Sin", EnsureArgumentCount(1,  args => args[0].Sin()));
        Add("Cos", EnsureArgumentCount(1,  args => args[0].Cos()));
        Add("Sqrt", EnsureArgumentCount(1,  args => args[0].Sqrt()));
        Add("Abs", EnsureArgumentCount(1,  args => args[0].Abs()));
        Add("Max", EnsureArgumentCount(2,  args => args[0].Max(args[1])));
        Add("Min", EnsureArgumentCount(2,  args => args[0].Min(args[1])));
        Add("Pow", EnsureArgumentCount(2,  args => args[0].Pow(args[1])));
        Add("Add", EnsureArgumentCount(2,  args => args[0].Add(args[1])));
        Add("Subtract", EnsureArgumentCount(2,  args => args[0].Subtract(args[1])));
        Add("Multiply", EnsureArgumentCount(2,  args => args[0].Multiply(args[1])));
        Add("Divide", EnsureArgumentCount(2,  args => args[0].Divide(args[1])));
        Add("Sum", variants => variants.Sum());
        Add("Factorial", EnsureArgumentCount(1, args => args[0].Factorial()));
    }
}