namespace Msh.StandardLibrary.Mathematics;

using static ArgumentsHelper;

public sealed class Functions : Dictionary<string, MathFunction>
{
    private static readonly Lazy<Functions> Instance = new(() => new Functions(), isThreadSafe: true);

    public static Functions Registry => Instance.Value;

    private Functions()
    {
        Add("Sin", ExpectArgs(1,  args => args[0].Sin()));
        Add("Cos", ExpectArgs(1,  args => args[0].Cos()));
        Add("Sqrt", ExpectArgs(1,  args => args[0].Sqrt()));
        Add("Abs", ExpectArgs(1,  args => args[0].Abs()));
        Add("Max", ExpectArgs(2,  args => args[0].Max(args[1])));
        Add("Min", ExpectArgs(2,  args => args[0].Min(args[1])));
        Add("Pow", ExpectArgs(2,  args => args[0].Pow(args[1])));
        Add("Add", ExpectArgs(2,  args => args[0].Add(args[1])));
        Add("Subtract", ExpectArgs(2,  args => args[0].Subtract(args[1])));
        Add("Multiply", ExpectArgs(2,  args => args[0].Multiply(args[1])));
        Add("Divide", ExpectArgs(2,  args => args[0].Divide(args[1])));
        Add("Sum", variants => variants.Sum());
        Add("Factorial", ExpectArgs(1, args => args[0].Factorial()));
    }
}