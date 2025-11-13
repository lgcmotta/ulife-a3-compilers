using Msh.StandardLibrary.Types;

namespace Msh.Interpreter.Slots;

internal class VariableSlot
{
    public VariableSlot(string type, IVariant variant)
    {
        Type = type;
        Variant = variant;
    }

    internal string Type { get; private set; }

    internal IVariant Variant { get; private set; }

    internal void ChangeType(string type, IVariant variant)
    {
        Type = type;
        Variant = variant;
    }

    internal void OverrideVariant(IVariant variant)
    {
        Variant = variant;
    }
}