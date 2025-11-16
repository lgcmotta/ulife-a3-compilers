using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Msh.StandardLibrary.Types;

public sealed class LongType : Variant<LongType>, IVariant
{
    public LongType(long value)
    {
        Value = value;
    }

    public Kind Kind => Kind.Long;

    public long Value { get; }

    public static IVariant Zero => new LongType(0L);

    public static IVariant One => new LongType(1L);

    public static LongType operator +(LongType left, LongType right) => new(left.Value + right.Value);
    public static LongType operator -(LongType left, LongType right) => new(left.Value - right.Value);
    public static LongType operator *(LongType left, LongType right) => new(left.Value * right.Value);

    public static LongType operator /(LongType left, LongType right)
    {
        return right.Value is not 0L
            ? new LongType(left.Value / right.Value)
            : throw new DivideByZeroException();
    }

    public static LongType operator %(LongType left, LongType right)
    {
        return right.Value is not 0L
            ? new LongType(left.Value % right.Value)
            : throw new DivideByZeroException();
    }

    public static bool operator ==(LongType? left, LongType? right)
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

    public static bool operator !=(LongType? left, LongType? right) => !(left == right);

    public static bool operator <(LongType? left, LongType? right) =>
        left switch
        {
            null => right is not null,
            _ => right is not null && left.Value < right.Value
        };

    public static bool operator >(LongType? left, LongType? right) =>
        right switch
        {
            null => left is not null,
            _ => left is not null && left.Value > right.Value
        };

    public static bool operator <=(LongType? left, LongType? right) =>
        left switch
        {
            null => true,
            _ => right is not null && left.Value <= right.Value
        };

    public static bool operator >=(LongType? left, LongType? right) =>
        right switch
        {
            null => left is not null,
            _ => left is not null && left.Value >= right.Value
        };

    public override int CompareTo(LongType? other)
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
        return other is not LongType longType
            ? 1
            : Value.CompareTo(longType.Value);
    }

    public override bool Equals(LongType? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (other is null)
        {
            return false;
        }

        return Value.CompareTo(other.Value) is 0;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is LongType longType && Value == longType.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    public string ToString([StringSyntax("NumericFormat")] string? format, IFormatProvider? provider)
    {
        return Value.ToString(format, provider);
    }
}