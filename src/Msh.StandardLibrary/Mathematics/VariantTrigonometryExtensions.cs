using Msh.StandardLibrary.Mathematics.Exceptions;
using Msh.StandardLibrary.Types;

namespace Msh.StandardLibrary.Mathematics;

public static class VariantTrigonometryExtensions
{
    extension(IVariant variant)
    {
        public IVariant Sin()
        {
            return variant.Kind switch
            {
                Kind.Long when variant is LongType l => new DoubleType(Math.Sin(l.Value)),
                Kind.Double when variant is DoubleType d => new DoubleType(Math.Sin(d.Value)),
                Kind.Decimal when variant is DecimalType m =>  new DecimalType((decimal)Math.Sin((double)m.Value)),
                Kind.List when variant is ListType list && list.IsScalarValuesCollection() => new ListType(list.Select(Sin)),
                Kind.Bool or Kind.String or Kind.Object => throw new OperationNotSupportedException(nameof(Sin)),
                _ => throw new OperationNotSupportedException(nameof(Sin))
            };
        }

        public IVariant Cos()
        {
            return variant.Kind switch
            {
                Kind.Long when variant is LongType l => new DoubleType(Math.Cos(l.Value)),
                Kind.Double when variant is DoubleType d => new DoubleType(Math.Cos(d.Value)),
                Kind.Decimal when variant is DecimalType m =>  new DecimalType((decimal)Math.Cos((double)m.Value)),
                Kind.List when variant is ListType list && list.IsScalarValuesCollection() => new ListType(list.Select(Cos)),
                Kind.Bool or Kind.String or Kind.Object => throw new OperationNotSupportedException(nameof(Cos)),
                _ => throw new OperationNotSupportedException(nameof(Cos))
            };
        }
    }
}