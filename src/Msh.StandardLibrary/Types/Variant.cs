namespace Msh.StandardLibrary.Types;

public abstract class Variant<TVariant> : IComparable<TVariant>, IEquatable<TVariant>
    where TVariant : IVariant
{
    public abstract int CompareTo(TVariant? other);

    public abstract bool Equals(TVariant? other);

    public abstract override bool Equals(object? obj);

    public abstract override int GetHashCode();
}