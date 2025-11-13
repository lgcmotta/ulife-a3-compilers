using System.Globalization;

namespace Msh.StandardLibrary.Types;

public sealed class StringType : Variant<StringType>, IVariant
{
    public StringType(string? value)
    {
        Value = value ?? string.Empty;
    }

    public Kind Kind => Kind.String;

    public string Value { get; }

    public static StringType operator +(StringType left, StringType right)
        => new($"{left.Value}{right.Value}");

    public static bool operator ==(StringType? left, StringType? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return string.Equals(left.Value, right.Value, StringComparison.Ordinal);
    }

    public static bool operator !=(StringType? left, StringType? right) => !(left == right);

    public static bool operator <(StringType? left, StringType? right) =>
        left switch
        {
            null => right is not null,
            _ => right is not null && string.Compare(left.Value, right.Value, StringComparison.Ordinal) < 0
        };

    public static bool operator >(StringType? left, StringType? right) =>
        right switch
        {
            null => left is not null,
            _ => left is not null && string.Compare(left.Value, right.Value, StringComparison.Ordinal) > 0
        };

    public static bool operator <=(StringType? left, StringType? right) =>
        left switch
        {
            null => true,
            _ => right is not null && string.Compare(left.Value, right.Value, StringComparison.Ordinal) <= 0
        };

    public static bool operator >=(StringType? left, StringType? right) =>
        right switch
        {
            null => left is not null,
            _ => left is not null && string.Compare(left.Value, right.Value, StringComparison.Ordinal) >= 0
        };

    public override int CompareTo(StringType? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        if (other is null)
        {
            return 1;
        }

        return string.Compare(Value, other.Value, StringComparison.Ordinal);
    }

    int IComparable<IVariant>.CompareTo(IVariant? other)
    {
        return other is not StringType stringType
            ? 1
            : Value.CompareTo(stringType.Value, StringComparison.Ordinal);
    }

    public override bool Equals(StringType? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (other is null)
        {
            return false;
        }

        return string.Equals(Value, other.Value, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj is StringType stringType && string.Equals(Value, stringType.Value, StringComparison.Ordinal);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode(StringComparison.Ordinal);
    }

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    public string ToString(IFormatProvider? provider)
    {
        return Value.ToString(provider);
    }
}