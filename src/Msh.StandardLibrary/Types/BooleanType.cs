namespace Msh.StandardLibrary.Types;

public sealed class BooleanType : Variant<BooleanType>, IVariant
{
    public BooleanType(bool value)
    {
        Value = value;
    }

    public Kind Kind => Kind.Bool;

    public bool Value { get; }

    public static BooleanType operator &(BooleanType left, BooleanType right) => new(left.Value & right.Value);
    public static BooleanType operator |(BooleanType left, BooleanType right) => new(left.Value | right.Value);
    public static BooleanType operator ^(BooleanType left, BooleanType right) => new(left.Value ^ right.Value);
    public static BooleanType operator !(BooleanType value) => new(!value.Value);

    public static bool operator ==(BooleanType? left, BooleanType? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Value == right.Value;
    }

    public static bool operator !=(BooleanType? left, BooleanType? right) => !(left == right);

    public static bool operator <(BooleanType? left, BooleanType? right) =>
        left switch
        {
            null => right is not null,
            _ => right is not null && left.Value.CompareTo(right.Value) < 0
        };

    public static bool operator >(BooleanType? left, BooleanType? right) =>
        right switch
        {
            null => left is not null,
            _ => left is not null && left.Value.CompareTo(right.Value) > 0
        };

    public static bool operator <=(BooleanType? left, BooleanType? right) =>
        left switch
        {
            null => true,
            _ => right is not null && left.Value.CompareTo(right.Value) <= 0
        };

    public static bool operator >=(BooleanType? left, BooleanType? right) =>
        right switch
        {
            null => left is not null,
            _ => left is not null && left.Value.CompareTo(right.Value) >= 0
        };

    public override int CompareTo(BooleanType? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        if (other is null)
        {
            return 1;
        }

        return Value.CompareTo(other.Value);
    }

    int IComparable<IVariant>.CompareTo(IVariant? other)
    {
        return other is not BooleanType booleanType
            ? 1
            : Value.CompareTo(booleanType.Value);
    }

    public override bool Equals(BooleanType? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (other is null)
        {
            return false;
        }

        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is BooleanType booleanType && Value == booleanType.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value
            ? bool.TrueString.ToLowerInvariant()
            : bool.FalseString.ToLowerInvariant();
    }
}