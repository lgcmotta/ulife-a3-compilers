using System.Globalization;

using Msh.StandardLibrary.Exceptions;
using Msh.StandardLibrary.Mathematics;
using Msh.StandardLibrary.Operators;
using Msh.StandardLibrary.Types;

namespace Msh.Interpreter.Extensions;

internal static class VariantExtensions
{
    extension(IVariant variant)
    {
        internal string InferTypeName()
        {
            return variant.Kind switch
            {
                Kind.Long => "int",
                Kind.Double => "double",
                Kind.Decimal => "decimal",
                Kind.Bool => "bool",
                Kind.String => "string",
                Kind.List => $"{InferElementType((ListType)variant)}[]",
                Kind.Object => throw new InvalidOperationException("Unsupported type inference."),
                _ => throw new InvalidOperationException("Unsupported type inference.")
            };
        }

        internal string FormatInterpolationValue(string? precision)
        {
            if (precision is null)
            {
                return variant switch
                {
                    StringType s => s.Value,
                    BooleanType b => b.Value.ToString().ToLowerInvariant(),
                    LongType l => l.Value.ToString(CultureInfo.InvariantCulture),
                    DoubleType d => d.Value.ToString(CultureInfo.InvariantCulture),
                    DecimalType m => m.Value.ToString(CultureInfo.InvariantCulture),
                    ListType list => $"[{string.Join(", ", list.Select(v => FormatInterpolationValue(v, null)))}]",
                    _ => variant.ToString()
                } ?? string.Empty;
            }

            return int.TryParse(precision, NumberStyles.Integer, CultureInfo.InvariantCulture, out var digits)
                ? variant switch
                {
                    DoubleType d => d.Value.ToString($"F{digits}", CultureInfo.InvariantCulture),
                    DecimalType m => m.Value.ToString($"F{digits}", CultureInfo.InvariantCulture),
                    LongType l => l.Value.ToString(CultureInfo.InvariantCulture), // precision ignored
                    _ => throw new InvalidOperationException("Precision specifier is only supported for numeric values.")
                }
                : throw new InvalidOperationException($"Invalid precision specifier '{precision}'.");
        }

        internal int ToIntegerIndex()
        {
            return variant is LongType longVariant
                ? checked((int)longVariant.Value)
                : throw new InvalidOperationException("List indexes must evaluate to an integer.");
        }

        internal TVariant Cast<TVariant>()
            where TVariant : class, IVariant
        {
            return variant as TVariant ?? throw new InvalidVariantCastException(typeof(TVariant).Name);
        }

        internal IVariant ReverseSign()
        {
            return variant.Kind switch
            {
                Kind.Long => new LongType(-variant.Cast<LongType>().Value),
                Kind.Double => new DoubleType(-variant.Cast<DoubleType>().Value),
                Kind.Decimal => new DecimalType(-variant.Cast<DecimalType>().Value),
                Kind.Bool => !variant.Cast<BooleanType>(),
                Kind.List => new ListType(variant.Cast<ListType>().Select(ReverseSign)),
                Kind.String => new StringType(new string([..variant.Cast<StringType>().Value.Reverse()])),
                Kind.Object => throw new InvalidOperationException(),
                _ => throw new InvalidOperationException()
            };
        }

        internal IVariant MutateUnary(string op)
        {
            var one = variant.Kind switch
            {
                Kind.Decimal => DecimalType.One,
                Kind.Double => DoubleType.One,
                Kind.Long => LongType.One,
                Kind.Bool => throw new InvalidOperationException("Unary operators require numeric types."),
                Kind.String => throw new InvalidOperationException("Unary operators require numeric types."),
                Kind.List => throw new InvalidOperationException("Unary operators require numeric types."),
                Kind.Object => throw new InvalidOperationException("Unary operators require numeric types."),
                _ => throw new InvalidOperationException("Unary operators require numeric types.")
            };

            return op switch
            {
                Operator.Increment => variant.CompoundAssignment(one, Operator.CompoundAdd),
                Operator.Decrement => variant.CompoundAssignment(one, Operator.CompoundSubtract),
                _ => throw new InvalidOperationException("Unknown unary assignment operator.")
            };
        }

        internal IVariant CompoundAssignment(IVariant right, string @operator)
        {
            return @operator switch
            {
                "+=" => variant.CompoundAssignmentAdd(right),
                "-=" => variant.CompoundAssignmentSubtract(right),
                "*=" => variant.CompoundAssignmentMultiply(right),
                "/=" => variant.CompoundAssignmentDivide(right),
                "%=" => variant.CompoundAssignmentModulus(right),
                _ => throw new InvalidOperationException("Unknown compound assignment operator.")
            };
        }

        private IVariant CompoundAssignmentAdd(IVariant other)
        {
            return variant switch
            {
                StringType left when other is StringType right => left + right,
                LongType or DoubleType or DecimalType when other is LongType or DoubleType or DecimalType => variant.Add(other),
                _ => throw new InvalidOperationException("Compound assignment '+=' requires numeric or string types.")
            };
        }

        private IVariant CompoundAssignmentSubtract(IVariant other)
        {
            return variant switch
            {
                LongType or DoubleType or DecimalType when other is LongType or DoubleType or DecimalType => variant.Subtract(other),
                _ => throw new InvalidOperationException("Compound assignment '/=' requires numeric types.")
            };
        }

        private IVariant CompoundAssignmentMultiply(IVariant other)
        {
            return variant switch
            {
                LongType or DoubleType or DecimalType when other is LongType or DoubleType or DecimalType => variant.Multiply(other),
                _ => throw new InvalidOperationException("Compound assignment '*=' requires numeric types.")
            };
        }

        private IVariant CompoundAssignmentDivide(IVariant other)
        {
            return variant switch
            {
                LongType or DoubleType or DecimalType when other is LongType or DoubleType or DecimalType => variant.Divide(other),
                _ => throw new InvalidOperationException("Compound assignment '/=' requires numeric types.")
            };
        }

        private IVariant CompoundAssignmentModulus(IVariant other)
        {
            return variant switch
            {
                LongType or DoubleType or DecimalType when other is LongType or DoubleType or DecimalType => variant.Modulus(other),
                _ => throw new InvalidOperationException("Compound assignment '%=' requires numeric types.")
            };
        }
    }

    extension(ListType variant)
    {
        private string InferElementType()
        {
            if (variant.Count is 0)
            {
                throw new InvalidOperationException("Cannot infer type from an empty list.");
            }

            var elementTypes = variant.Select(InferTypeName).Distinct().ToArray();

            return elementTypes is { Length: 1 }
                ? elementTypes[0]
                : throw new InvalidOperationException("List elements must have the same inferred type.");
        }
    }
}