using Msh.StandardLibrary.Mathematics.Exceptions;
using Msh.StandardLibrary.Types;

namespace Msh.StandardLibrary.Mathematics;

public static class VariantFactorialExtensions
{
    extension(IVariant variant)
    {
        public IVariant Factorial()
        {
            return variant.Kind switch
            {
                Kind.Long when variant is LongType l => new LongType(Factorial(l.Value)),
                Kind.Double when variant is DoubleType d => new LongType(Factorial((long)d.Value)),
                Kind.Decimal when variant is DecimalType m => new LongType(Factorial((long)m.Value)),
                Kind.List when variant is ListType lt && lt.IsScalarValuesCollection() => new ListType(lt.Select(Factorial)),
                Kind.Bool or Kind.String or Kind.Object => throw new OperationNotSupportedException(nameof(Factorial)),
                _ => throw new OperationNotSupportedException(nameof(Factorial))
            };
        }
    }

    private static long Factorial(long value)
    {
        if (value is 0 or 1)
        {
            return 1;
        }

        var result = 1L;

        for (var i = 2L; i <= value; i++)
        {
            result *= i;
        }

        return result;
    }
}