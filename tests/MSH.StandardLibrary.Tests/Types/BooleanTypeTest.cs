using JetBrains.Annotations;
using Msh.StandardLibrary.Types;
using Shouldly;
using Xunit;

namespace Msh.StandardLibrary.Tests.Types;

[TestSubject(typeof(BooleanType))]
public class BooleanTypeTest
{
    public static TheoryData<bool, bool> BinaryPairs => new()
    {
        { true, true },
        { true, false },
        { false, true },
        { false, false }
    };

    [Theory]
    [MemberData(nameof(BinaryPairs))]
    public void Operator_And(bool left, bool right)
    {
        var result = (BooleanType)left & right;
        result.Value.ShouldBe(left & right);
    }

    [Theory]
    [MemberData(nameof(BinaryPairs))]
    public void Operator_Or(bool left, bool right)
    {
        var result = (BooleanType)left | right;
        result.Value.ShouldBe(left | right);
    }

    [Theory]
    [MemberData(nameof(BinaryPairs))]
    public void Operator_Xor(bool left, bool right)
    {
        var result = (BooleanType)left ^ right;
        result.Value.ShouldBe(left ^ right);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Operator_Not(bool value)
    {
        var result = !(BooleanType)value;
        result.Value.ShouldBe(!value);
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void Operator_Equals_ReturnsTrueForSameValue(bool left, bool right)
    {
        var leftType = new BooleanType(left);
        var rightType = new BooleanType(right);
        (leftType == rightType).ShouldBeTrue();
    }

    [Fact]
    public void Operator_Equals_ReturnsTrueForReferenceEquals()
    {
        var instance = new BooleanType(true);
        (instance == instance).ShouldBeTrue();
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void Operator_NotEquals_ReturnsTrueForDifferentValues(bool left, bool right)
    {
        var leftType = new BooleanType(left);
        var rightType = new BooleanType(right);
        (leftType != rightType).ShouldBeTrue();
    }

    [Theory]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public void Operator_LessThanOrEqual(bool left, bool right)
    {
        var leftType = new BooleanType(left);
        var rightType = new BooleanType(right);
        (leftType <= rightType).ShouldBeTrue();
    }

    [Theory]
    [InlineData(true, false)]
    public void Operator_GreaterThan(bool left, bool right)
    {
        var leftType = new BooleanType(left);
        var rightType = new BooleanType(right);
        (leftType > rightType).ShouldBeTrue();
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void Operator_GreaterThanOrEqual(bool left, bool right)
    {
        var leftType = new BooleanType(left);
        var rightType = new BooleanType(right);
        (leftType >= rightType).ShouldBeTrue();
    }

    [Fact]
    public void ImplicitConversion_ToBool_ReturnsUnderlyingValue()
    {
        bool value = new BooleanType(true);
        value.ShouldBeTrue();
    }

    [Fact]
    public void ImplicitConversion_FromBool_CreatesBooleanType()
    {
        BooleanType type = true;
        type.Value.ShouldBeTrue();
    }

    [Fact]
    public void CompareTo_ReturnsZeroForSameInstance()
    {
        var instance = new BooleanType(true);
        instance.CompareTo(instance).ShouldBe(0);
    }

    [Fact]
    public void CompareTo_ReturnsPositiveWhenOtherIsNull()
    {
        var instance = new BooleanType(true);
        instance.CompareTo(null).ShouldBe(1);
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void CompareTo_ReturnsZeroForEqualValues(bool left, bool right)
    {
        var result = new BooleanType(left).CompareTo(new BooleanType(right));
        result.ShouldBe(0);
    }

    [Fact]
    public void IComparableVariant_ReturnsZeroForSameValue()
    {
        IComparable<IVariant> comparer = new BooleanType(true);
        var other = new BooleanType(true);
        comparer.CompareTo(other).ShouldBe(0);
    }

    [Fact]
    public void IComparableVariant_ReturnsPositiveForDifferentType()
    {
        IComparable<IVariant> comparer = new BooleanType(true);
        IVariant other = new LongType(1);
        comparer.CompareTo(other).ShouldBe(1);
    }

    [Fact]
    public void Equals_BooleanType_ReturnsTrueForSameValue()
    {
        var left = new BooleanType(true);
        var right = new BooleanType(true);
        left.Equals(right).ShouldBeTrue();
    }

    [Fact]
    public void Equals_BooleanType_ReturnsFalseForNull()
    {
        var left = new BooleanType(true);
        BooleanType? right = null;
        left.Equals(right).ShouldBeFalse();
    }

    [Fact]
    public void Equals_Object_ReturnsTrueForReferenceEquals()
    {
        var instance = new BooleanType(true);
        object boxed = instance;
        instance.Equals(boxed).ShouldBeTrue();
    }

    [Fact]
    public void Equals_Object_ReturnsFalseForDifferentType()
    {
        var instance = new BooleanType(true);
        object other = new object();
        instance.Equals(other).ShouldBeFalse();
    }

    [Fact]
    public void GetHashCode_UsesUnderlyingBoolHash()
    {
        var instance = new BooleanType(true);
        instance.GetHashCode().ShouldBe(true.GetHashCode());
    }

    [Theory]
    [InlineData(true, "true")]
    [InlineData(false, "false")]
    public void ToString_ReturnsLowercaseLiteral(bool value, string expected)
    {
        var instance = new BooleanType(value);
        instance.ToString().ShouldBe(expected);
    }
}