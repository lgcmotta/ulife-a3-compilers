using Msh.StandardLibrary.Mathematics.Exceptions;
using Msh.StandardLibrary.Types;

namespace Msh.StandardLibrary.Mathematics;

public static class VariantArithmeticExtensions
{
    extension(IVariant variant)
    {
        internal IVariant Add(IVariant other)
        {
            if (variant is ListType left && other is ListType right)
            {
                return left.ZipAndCalculate(right, Add);
            }

            if (!variant.IsScalarValue() || !other.IsScalarValue())
            {
                throw new UnsupportedOperationException(nameof(Add), variant.Kind, other.Kind);
            }

            return variant switch
            {
                LongType l when other is LongType r => l + r,
                LongType l when other is DoubleType r => new DoubleType(l.Value + r.Value),
                LongType l when other is DecimalType r => new DecimalType(l.Value + r.Value),

                DoubleType l when other is LongType r => new DoubleType(l.Value + r.Value),
                DoubleType l when other is DoubleType r => l + r,
                DoubleType l when other is DecimalType r => new DecimalType((decimal)l.Value + r.Value),

                DecimalType l when other is LongType r => new DecimalType(l.Value + r.Value),
                DecimalType l when other is DoubleType r => new DecimalType(l.Value + (decimal)r.Value),
                DecimalType l when other is DecimalType r => l + r,

                _ => throw new OperationNotSupportedException(nameof(Add))
            };
        }

        internal IVariant Subtract(IVariant other)
        {
            if (variant is ListType left && other is ListType right)
            {
                return left.ZipAndCalculate(right, Subtract);
            }

            if (!variant.IsScalarValue() || !other.IsScalarValue())
            {
                throw new UnsupportedOperationException(nameof(Subtract), variant.Kind, other.Kind);
            }

            return variant switch
            {
                LongType l when other is LongType r => l - r,
                LongType l when other is DoubleType r => new DoubleType(l.Value - r.Value),
                LongType l when other is DecimalType r => new DecimalType(l.Value - r.Value),

                DoubleType l when other is LongType r => new DoubleType(l.Value - r.Value),
                DoubleType l when other is DoubleType r => l - r,
                DoubleType l when other is DecimalType r => new DecimalType((decimal)l.Value - r.Value),

                DecimalType l when other is LongType r => new DecimalType(l.Value - r.Value),
                DecimalType l when other is DoubleType r => new DecimalType(l.Value - (decimal)r.Value),
                DecimalType l when other is DecimalType r => l - r,

                _ => throw new OperationNotSupportedException(nameof(Subtract))
            };
        }

        internal IVariant Multiply(IVariant other)
        {
            if (variant is ListType left && other is ListType right)
            {
                return left.ZipAndCalculate(right, Multiply);
            }

            if (!variant.IsScalarValue() || !other.IsScalarValue())
            {
                throw new UnsupportedOperationException(nameof(Multiply), variant.Kind, other.Kind);
            }

            return variant switch
            {
                LongType l when other is LongType r => l * r,
                LongType l when other is DoubleType r => new DoubleType(l.Value * r.Value),
                LongType l when other is DecimalType r => new DecimalType(l.Value * r.Value),

                DoubleType l when other is LongType r => new DoubleType(l.Value * r.Value),
                DoubleType l when other is DoubleType r => l * r,
                DoubleType l when other is DecimalType r => new DecimalType((decimal)l.Value * r.Value),

                DecimalType l when other is LongType r => new DecimalType(l.Value * r.Value),
                DecimalType l when other is DoubleType r => new DecimalType(l.Value * (decimal)r.Value),
                DecimalType l when other is DecimalType r => l * r,

                _ => throw new OperationNotSupportedException(nameof(Multiply))
            };
        }

        internal IVariant Divide(IVariant other)
        {
            if (variant is ListType left && other is ListType right)
            {
                return left.ZipAndCalculate(right, Divide);
            }

            if (!variant.IsScalarValue() || !other.IsScalarValue())
            {
                throw new UnsupportedOperationException(nameof(Divide), variant.Kind, other.Kind);
            }

            return variant switch
            {
                LongType l when other is LongType r => l / r,
                LongType l when other is DoubleType r => new DoubleType(l.Value / r.Value),
                LongType l when other is DecimalType r => new DecimalType(l.Value / r.Value),

                DoubleType l when other is LongType r => new DoubleType(l.Value / r.Value),
                DoubleType l when other is DoubleType r => l / r,
                DoubleType l when other is DecimalType r => new DecimalType((decimal)l.Value / r.Value),

                DecimalType l when other is LongType r => new DecimalType(l.Value / r.Value),
                DecimalType l when other is DoubleType r => new DecimalType(l.Value / (decimal)r.Value),
                DecimalType l when other is DecimalType r => l / r,

                _ => throw new OperationNotSupportedException(nameof(Divide))
            };
        }
    }
}