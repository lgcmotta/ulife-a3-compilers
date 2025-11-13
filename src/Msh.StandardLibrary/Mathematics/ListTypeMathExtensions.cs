using Msh.StandardLibrary.Types;

namespace Msh.StandardLibrary.Mathematics;

internal static class ListTypeMathExtensions
{
    extension(ListType left)
    {
        internal ListType ZipAndCalculate(ListType right, Func<IVariant, IVariant, IVariant> calc)
        {
            return new ListType(left.Items.Zip(right.Items, calc));
        }

        internal bool IsScalarValuesCollection()
        {
            return !left.Any(item => item.Kind is Kind.Bool or Kind.String or Kind.List or Kind.Object);
        }
    }
}

internal static class ScalarVariantExtensions
{
    extension(IVariant variant)
    {
        internal bool IsScalarValue()
        {
            return variant.Kind is Kind.Long or Kind.Double or Kind.Decimal;
        }
    }

    extension(IEnumerable<IVariant> variants)
    {
        internal bool AreSuitableForMath()
        {
            return !variants.Any(item => item.Kind is Kind.Bool or Kind.String or Kind.Object);
        }
    }
}