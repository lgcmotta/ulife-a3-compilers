using System.Globalization;

using JetBrains.Annotations;

using Msh.StandardLibrary.Types;

using Shouldly;

namespace Msh.StandardLibrary.Tests.Types;

[TestSubject(typeof(StringType))]
public class StringTypeTest
{
    public static TheoryData<string, string> EqualPairs => new() { { "foo", "foo" } };

    [Theory]
    [MemberData(nameof(EqualPairs))]
    public void Operator_Equals_ReturnsTrueForSameValue(string left, string right)
    {
        // Arrange
        var leftType = new StringType(left);
        var rightType = new StringType(right);

        // Act
        var result = leftType == rightType;

        // Assert
        result.ShouldBeTrue();
    }

    public static TheoryData<string, string> NonEqualPairs => new() { { "foo", "bar" } };

    [Theory]
    [MemberData(nameof(NonEqualPairs))]
    public void Operator_NotEquals_ReturnsTrueForDifferentValue(string left, string right)
    {
        // Arrange
        var leftType = new StringType(left);
        var rightType = new StringType(right);

        // Act
        var result = leftType != rightType;

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void Operator_Equals_ReturnsTrueForReferenceEquals()
    {
        // Arrange
        var instance = new StringType("value");

        // Act
        // ReSharper disable once EqualExpressionComparison
        var result = instance == instance;

        // Assert
        result.ShouldBeTrue();
    }

    public static TheoryData<string, string> LessPairs => new() { { "alpha", "beta" } };

    [Theory]
    [MemberData(nameof(LessPairs))]
    public void Operator_LessThan(string left, string right)
    {
        // Arrange
        var leftType = new StringType(left);
        var rightType = new StringType(right);

        // Act
        var result = leftType < rightType;

        // Assert
        result.ShouldBeTrue();
    }


    public static TheoryData<string, string> LessOrEqualPairs => new() { { "alpha", "alpha" }, { "alpha", "beta" } };

    [Theory]
    [MemberData(nameof(LessOrEqualPairs))]
    public void Operator_LessThanOrEqual(string left, string right)
    {
        // Arrange
        var leftType = new StringType(left);
        var rightType = new StringType(right);

        // Act
        var result = leftType <= rightType;

        // Assert
        result.ShouldBeTrue();
    }

    public static TheoryData<string, string> GreaterThanPairs => new() { { "beta", "alpha" } };

    [Theory]
    [MemberData(nameof(GreaterThanPairs))]
    public void Operator_GreaterThan(string left, string right)
    {
        // Arrange
        var leftType = new StringType(left);
        var rightType = new StringType(right);

        // Act
        var result = leftType > rightType;

        // Assert
        result.ShouldBeTrue();
    }

    public static TheoryData<string, string> GreaterOrEqualPairs => new() { { "beta", "beta" }, { "beta", "alpha" } };

    [Theory]
    [MemberData(nameof(GreaterOrEqualPairs))]
    public void Operator_GreaterThanOrEqual(string left, string right)
    {
        // Arrange
        var leftType = new StringType(left);
        var rightType = new StringType(right);

        // Act
        var result = leftType >= rightType;

        // Assert
        result.ShouldBeTrue();
    }

    public static TheoryData<string, string, string> ConcatPairs => new() { { "hello", " world", "hello world" } };

    [Theory]
    [MemberData(nameof(ConcatPairs))]
    public void Operator_Add_ConcatenatesStrings(string left, string right, string expected)
    {
        // Arrange
        var leftType = new StringType(left);
        var rightType = new StringType(right);

        // Act
        var result = leftType + rightType;

        // Assert
        result.Value.ShouldBe(expected);
    }

    [Fact]
    public void ImplicitConversion_ToString_ReturnsUnderlyingValue()
    {
        // Arrange & Act
        string value = new StringType("test");

        // Assert
        value.ShouldBe("test");
    }

    [Fact]
    public void ImplicitConversion_FromString_CreatesStringType()
    {
        // Arrange & Act
        StringType value = "hello";

        // Assert
        value.Value.ShouldBe("hello");
    }

    [Fact]
    public void CompareTo_ReturnsZeroForSameInstance()
    {
        // Arrange
        var instance = new StringType("value");

        // Act
        int compareTo = instance.CompareTo(instance);

        // Assert
        compareTo.ShouldBe(0);
    }

    [Fact]
    public void CompareTo_ReturnsPositiveWhenOtherIsNull()
    {
        // Arrange
        var instance = new StringType("value");

        // Act
        int compareTo = instance.CompareTo(null);

        // Assert
        compareTo.ShouldBe(1);
    }

    [Theory]
    [MemberData(nameof(EqualPairs))]
    public void IComparableVariant_ReturnsZeroForEqualValues(string left, string right)
    {
        // Arrange
        IComparable<IVariant> comparer = new StringType(left);
        var rightType = new StringType(right);

        // Act
        int compareTo = comparer.CompareTo(rightType);

        // Assert
        compareTo.ShouldBe(0);
    }

    [Fact]
    public void IComparableVariant_ReturnsPositiveForDifferentType()
    {
        // Arrange
        IComparable<IVariant> comparer = new StringType("value");
        IVariant rightType = new LongType(1);

        // Act
        int compareTo = comparer.CompareTo(rightType);

        // Assert
        compareTo.ShouldBe(1);
    }

    [Theory]
    [MemberData(nameof(EqualPairs))]
    public void Equals_StringType_ReturnsTrueForSameValue(string left, string right)
    {
        // Arrange
        var leftType = new StringType(left);
        var rightType = new StringType(right);

        // Act
        bool equals = leftType.Equals(rightType);

        // Assert
        equals.ShouldBeTrue();
    }

    [Fact]
    public void Equals_StringType_ReturnsFalseForNull()
    {
        // Arrange
        var leftType = new StringType("value");
        StringType? rightType = null;

        // Act
        bool equals = leftType.Equals(rightType);

        // Assert
        equals.ShouldBeFalse();
    }

    [Fact]
    public void Equals_Object_ReturnsTrueForReferenceEquals()
    {
        // Arrange
        var leftType = new StringType("value");
        object rightType = leftType;

        // Act
        bool equals = leftType.Equals(rightType);

        // Assert
        equals.ShouldBeTrue();
    }

    [Fact]
    public void Equals_Object_ReturnsFalseForDifferentType()
    {
        // Arrange
        var leftType = new StringType("value");
        var rightType = new object();

        // Act
        bool equals = leftType.Equals(rightType);

        // Assert
        equals.ShouldBeFalse();
    }

    [Fact]
    public void GetHashCode_UsesOrdinalHash()
    {
        // Arrange
        var instance = new StringType("value");
        int expected = "value".GetHashCode(StringComparison.Ordinal);

        // Act
        int hashCode = instance.GetHashCode();

        // Assert
        hashCode.ShouldBe(expected);
    }

    [Fact]
    public void ToString_UsesInvariantCulture()
    {
        // Arrange
        var instance = new StringType("value");

        // Act
        var result = instance.ToString();

        // Assert
        result.ShouldBe("value");
    }

    [Fact]
    public void ToString_WithFormatProvider_UsesProvidedCulture()
    {
        // Arrange
        var instance = new StringType("value");

        // Act
        var result = instance.ToString(CultureInfo.InvariantCulture);

        // Assert
        result.ShouldBe("value");
    }
}