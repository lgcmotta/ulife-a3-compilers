namespace Msh.StandardLibrary.Types;

public sealed class ObjectType : Variant<ObjectType>, IVariant
{
    public ObjectType(object value)
    {
        Value = value;
    }

    public Kind Kind => Kind.Object;

    public object Value { get; }

    public static IVariant Null => new ObjectType(new object());

    public static bool operator ==(ObjectType? left, ObjectType? right)
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

    public static bool operator !=(ObjectType? left, ObjectType? right) => !(left == right);

    public static bool operator <(ObjectType? left, ObjectType? right) =>
        left switch
        {
            null => right is not null,
            _ => right is not null && left.Value.Equals(right.Value)
        };

    public static bool operator >(ObjectType? left, ObjectType? right) =>
        right switch
        {
            null => left is not null,
            _ => left is not null && left.Value.Equals(right.Value)
        };

    public static bool operator <=(ObjectType? left, ObjectType? right) =>
        left switch
        {
            null => true,
            _ => right is not null && left.Value.Equals(right.Value)
        };

    public static bool operator >=(ObjectType? left, ObjectType? right) =>
        right switch
        {
            null => left is not null,
            _ => left is not null && left.Value.Equals(right.Value)
        };

    public override int CompareTo(ObjectType? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        if (other is null)
        {
            return 1;
        }

        if (ReferenceEquals(Value, other.Value))
        {
            return 0;
        }

        if (Value.Equals(other.Value))
        {
            return 0;
        }

        if (Value.GetType() == other.Value.GetType() && Value is IComparable comparable)
        {
            return comparable.CompareTo(other.Value);
        }

        return string.CompareOrdinal(Value.ToString(), other.Value.ToString());
    }

    int IComparable<IVariant>.CompareTo(IVariant? other)
    {
        return other is ObjectType objectType
            ? CompareTo(objectType)
            : 1;
    }

    public override bool Equals(ObjectType? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return other is not null && Equals(Value, other.Value);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is ObjectType objectType && Equals(objectType);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}