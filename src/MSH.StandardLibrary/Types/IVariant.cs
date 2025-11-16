namespace Msh.StandardLibrary.Types;

public enum Kind
{
    Long,
    Double,
    Decimal,
    Bool,
    String,
    List,
    Object
}

public interface IVariant : IComparable<IVariant>
{
    public Kind Kind { get; }
}