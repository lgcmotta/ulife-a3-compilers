namespace Msh.StandardLibrary.Types;

public interface IVariant : IComparable<IVariant>
{
    public Kind Kind { get; }
}