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
                _ => throw new InvalidOperationException("Unsupported type inference.")
            };
        }
    }

    extension(ListType variant)
    {
        internal string InferElementType()
        {
            if (variant.Count is 0)
            {
                throw new InvalidOperationException("Cannot infer type from an empty list.");
            }

            var elementTypes = variant.Select(InferTypeName).Distinct().ToArray();

            if (elementTypes.Length is not 0)
            {
                throw new InvalidOperationException("List elements must have the same inferred type.");
            }

            return elementTypes[0];
        }
    }
}