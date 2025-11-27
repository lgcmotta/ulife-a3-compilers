using Msh.StandardLibrary.Mathematics.Exceptions;
using Msh.StandardLibrary.Types;

namespace Msh.StandardLibrary.Mathematics;

public static class VariantAlgebraExtensions
{
    extension(IVariant variant)
    {
        internal IVariant Sqrt()
        {
            return variant switch
            {
                LongType l => new DoubleType(Math.Sqrt(l.Value)),
                DoubleType d => new DoubleType(Math.Sqrt(d.Value)),
                DecimalType m => new DoubleType(Math.Sqrt((double)m.Value)),
                ListType lt when lt.IsScalarValuesCollection() => new ListType([.. lt.Select(Sqrt)]),
                _ => throw new OperationNotSupportedException(nameof(Sqrt))
            };
        }

        internal IVariant Pow(IVariant other)
        {
            return variant switch
            {
                LongType left when other is LongType right => new LongType((long)Math.Pow(left.Value, right.Value)),
                LongType left when other is DoubleType right => new DoubleType(Math.Pow(left.Value, right.Value)),
                LongType left when other is DecimalType right => new DoubleType(Math.Pow(left.Value, (double)right.Value)),

                DoubleType left when other is LongType right => new DoubleType(Math.Pow(left.Value, right.Value)),
                DoubleType left when other is DoubleType right => new DoubleType(Math.Pow(left.Value, right.Value)),
                DoubleType left when other is DecimalType right => new DoubleType(Math.Pow(left.Value, (double)right.Value)),

                DecimalType left when other is LongType right => new DoubleType(Math.Pow((double)left.Value, right.Value)),
                DecimalType left when other is DoubleType right => new DoubleType(Math.Pow((double)left.Value, right.Value)),
                DecimalType left when other is DecimalType right => new DoubleType(Math.Pow((double)left.Value, (double)right.Value)),

                ListType left when left.IsScalarValuesCollection() && other is ListType right => left.ZipAndCalculate(right, Pow),
                _ => throw new UnsupportedOperationException(nameof(Pow), variant.Kind, other.Kind)
            };
        }
    }
}