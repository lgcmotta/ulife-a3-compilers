using Msh.StandardLibrary.Mathematics.Exceptions;
using Msh.StandardLibrary.Types;

namespace Msh.StandardLibrary.Mathematics;

public static class VariantCalculusExtensions
{
    extension(IVariant variant)
    {
        public IVariant Abs()
        {
            return variant.Kind switch
            {
                Kind.Long when variant is LongType l => new LongType(Math.Abs(l.Value)),
                Kind.Double when variant is DoubleType d => new DoubleType(Math.Abs(d.Value)),
                Kind.Decimal when variant is DecimalType m => new DecimalType(decimal.Abs(m.Value)),
                Kind.List when variant is ListType list && list.IsScalarValuesCollection() => new ListType(list.Select(Abs)),
                Kind.Bool or Kind.String or Kind.Object => throw new OperationNotSupportedException(nameof(Abs)),
                _ => throw new OperationNotSupportedException(nameof(Abs))
            };
        }

        public IVariant Min(IVariant other)
        {
            return variant switch
            {
                LongType left when other is LongType right => new LongType(long.Min(left.Value, right.Value)),
                LongType left when other is DoubleType right => new DoubleType(double.Min(left.Value, right.Value)),
                LongType left when other is DecimalType right => new DecimalType(decimal.Min(left.Value, right.Value)),

                DoubleType left when other is LongType right => new DoubleType(double.Min(left.Value, right.Value)),
                DoubleType left when other is DoubleType right => new DoubleType(double.Min(left.Value, right.Value)),
                DoubleType left when other is DecimalType right => new DecimalType(decimal.Min((decimal)left.Value, right.Value)),

                DecimalType left when other is LongType right => new DecimalType(decimal.Min(left.Value, right.Value)),
                DecimalType left when other is DoubleType right => new DecimalType(decimal.Min(left.Value, (decimal)right.Value)),
                DecimalType left when other is DecimalType right => new DecimalType(decimal.Min(left.Value, right.Value)),

                ListType left when other is ListType right => left.ZipAndCalculate(right, Min),

                _ => throw new OperationNotSupportedException(nameof(Min))
            };
        }

        public IVariant Max(IVariant other)
        {
            return variant switch
            {
                LongType left when other is LongType right => new LongType(long.Max(left.Value, right.Value)),
                LongType left when other is DoubleType right => new DoubleType(double.Max(left.Value, right.Value)),
                LongType left when other is DecimalType right => new DecimalType(decimal.Max(left.Value, right.Value)),

                DoubleType left when other is LongType right => new DoubleType(double.Max(left.Value, right.Value)),
                DoubleType left when other is DoubleType right => new DoubleType(double.Max(left.Value, right.Value)),
                DoubleType left when other is DecimalType right => new DecimalType(decimal.Max((decimal)left.Value, right.Value)),

                DecimalType left when other is LongType right => new DecimalType(decimal.Max(left.Value, right.Value)),
                DecimalType left when other is DoubleType right => new DecimalType(decimal.Max(left.Value, (decimal)right.Value)),
                DecimalType left when other is DecimalType right => new DecimalType(decimal.Max(left.Value, right.Value)),

                ListType left when other is ListType right => left.ZipAndCalculate(right, Max),

                _ => throw new OperationNotSupportedException(nameof(Max))
            };
        }
    }

    public static IVariant Sum(this IEnumerable<IVariant> variants)
    {
        if (variants.TryGetNonEnumeratedCount(out var count) && count is 0)
        {
            return new LongType(0L);
        }

        var values = variants.ToList();

        if (!values.AreSuitableForMath())
        {
            throw new OperationNotSupportedException(nameof(Sum));
        }

        var lists = values.Where(v => v.Kind is Kind.List).Cast<ListType>().ToList();

        if (lists.Any(list => !list.Items.AreSuitableForMath()))
        {
            throw new OperationNotSupportedException(nameof(Sum));
        }

        var scalars = values.Where(v => v.Kind is not Kind.List).Concat(lists.SelectMany(list => list.Items)).ToList();

        if (scalars.Any(scalar => scalar.Kind is Kind.Decimal))
        {
            decimal total = scalars.Aggregate(0m, (current, variant) => decimal.Add(current, variant switch
            {
                LongType l => l.Value,
                DoubleType d => (decimal)d.Value,
                DecimalType m => m.Value,
                _ => throw new OperationNotSupportedException(nameof(Sum))
            }));

            return new DecimalType(total);
        }

        if (scalars.Any(scalar => scalar.Kind is Kind.Double))
        {
            double total = scalars.Aggregate(0d, (current, variant) =>
            {
                current += variant switch
                {
                    LongType l => l.Value,
                    DoubleType d => d.Value,
                    _ => throw new OperationNotSupportedException(nameof(Sum))
                };
                return current;
            });

            return new DoubleType(total);
        }

        {
            var total = scalars.Aggregate(0L, (current, variant) =>
            {
                if (variant is LongType l)
                {
                    current += l.Value;
                }

                return current;
            });

            return new LongType(total);
        }
    }
}