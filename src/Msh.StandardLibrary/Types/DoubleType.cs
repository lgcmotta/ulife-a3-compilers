using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Msh.StandardLibrary.Types;

public sealed class DoubleType : Variant<DoubleType>, IVariant
{
    public DoubleType(double value)
    {
        Value = value;
    }

    public Kind Kind => Kind.Double;

    public double Value { get; }

    public static DoubleType operator +(DoubleType left, DoubleType right) => new(left.Value + right.Value);
    public static DoubleType operator -(DoubleType left, DoubleType right) => new(left.Value - right.Value);
    public static DoubleType operator *(DoubleType left, DoubleType right) => new(left.Value * right.Value);

    public static DoubleType operator /(DoubleType left, DoubleType right)
    {
        return right.Value != 0D
            ? new DoubleType(left.Value / right.Value)
            : throw new DivideByZeroException();
    }

    public static bool operator ==(DoubleType? left, DoubleType? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Value.Equals(right.Value);
    }

    public static bool operator !=(DoubleType? left, DoubleType? right) => !(left == right);

    public static bool operator <(DoubleType? left, DoubleType? right) =>
        left switch
        {
            null => right is not null,
            _ => right is not null && left.Value < right.Value
        };

    public static bool operator >(DoubleType? left, DoubleType? right) =>
        right switch
        {
            null => left is not null,
            _ => left is not null && left.Value > right.Value
        };

    public static bool operator <=(DoubleType? left, DoubleType? right) =>
        left switch
        {
            null => true,
            _ => right is not null && left.Value <= right.Value
        };

    public static bool operator >=(DoubleType? left, DoubleType? right) =>
        right switch
        {
            null => left is not null,
            _ => left is not null && left.Value >= right.Value
        };

    public override int CompareTo(DoubleType? other)
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
        return other is not DoubleType doubleType
            ? 1
            : Value.CompareTo(doubleType.Value);
    }

    public override bool Equals(DoubleType? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (other is null)
        {
            return false;
        }

        var diff = Math.Abs(Value - other.Value);
        var scale = Math.Max(Math.Abs(Value), Math.Abs(other.Value));
        return diff <= Math.Max(1e-9, 1e-9 * scale);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj is not DoubleType doubleType)
        {
            return false;
        }

        var diff = Math.Abs(Value - doubleType.Value);
        var scale = Math.Max(Math.Abs(Value), Math.Abs(doubleType.Value));
        return diff <= Math.Max(1e-9, 1e-9 * scale);
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