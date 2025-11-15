using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Msh.StandardLibrary.Types;

public sealed class DecimalType : Variant<DecimalType>, IVariant
{
    public DecimalType(decimal value)
    {
        Value = value;
    }

    public Kind Kind => Kind.Decimal;

    public decimal Value { get; }

    public static DecimalType Zero => new(decimal.Zero);
    public static DecimalType One => new(decimal.One);
    public static DecimalType MinusOne => new(decimal.MinusOne);

    public static DecimalType operator +(DecimalType left, DecimalType right) => new(decimal.Add(left.Value, right.Value));
    public static DecimalType operator -(DecimalType left, DecimalType right) => new(decimal.Subtract(left.Value, right.Value));
    public static DecimalType operator *(DecimalType left, DecimalType right) => new(decimal.Multiply(left.Value, right.Value));

    public static DecimalType operator /(DecimalType left, DecimalType right)
    {
        return right.Value != decimal.Zero
            ? new DecimalType(decimal.Divide(left.Value, right.Value))
            : throw new DivideByZeroException();
    }

    public static DecimalType operator %(DecimalType left, DecimalType right)
    {
        return right.Value != decimal.Zero
            ? new DecimalType(decimal.Remainder(left.Value, right.Value))
            : throw new DivideByZeroException();
    }

    public static bool operator ==(DecimalType? left, DecimalType? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return decimal.Equals(left.Value, right.Value);
    }

    public static bool operator !=(DecimalType? left, DecimalType? right) => !(left == right);

    public static bool operator <(DecimalType? left, DecimalType? right) =>
        left switch
        {
            null => right is not null,
            _ => right is not null && left.Value < right.Value
        };

    public static bool operator >(DecimalType? left, DecimalType? right) =>
        right switch
        {
            null => left is not null,
            _ => left is not null && left.Value > right.Value
        };

    public static bool operator <=(DecimalType? left, DecimalType? right) =>
        left switch
        {
            null => true,
            _ => right is not null && left.Value <= right.Value
        };

    public static bool operator >=(DecimalType? left, DecimalType? right) =>
        right switch
        {
            null => left is not null,
            _ => left is not null && left.Value >= right.Value
        };

    public override int CompareTo(DecimalType? other)
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
        return other is not DecimalType decimalType
            ? 1
            : decimal.Compare(Value, decimalType.Value);
    }

    public override bool Equals(DecimalType? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (other is null)
        {
            return false;
        }

        return decimal.Compare(Value, other.Value) is 0;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is DecimalType decimalType && decimal.Compare(Value, decimalType.Value) is 0;
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