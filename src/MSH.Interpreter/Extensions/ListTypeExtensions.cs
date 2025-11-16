using Msh.Interpreter.Definitions;
using Msh.StandardLibrary.Types;

namespace Msh.Interpreter.Extensions;

internal static class ListTypeExtensions
{
    extension(ListType variant)
    {
        internal string InferElementType()
        {
            if (variant.Count is 0)
            {
                throw new InvalidOperationException("Cannot infer type from an empty list.");
            }

            var elementTypes = variant.Select(v => v.InferTypeName()).Distinct().ToArray();

            return elementTypes is { Length: 1 }
                ? elementTypes[0]
                : throw new InvalidOperationException("List elements must have the same inferred type.");
        }

        internal IVariant VisitAdd(ref VariableDefinition variable, IVariant[] arguments)
        {
            arguments.EnsureArgumentCount(nameof(ListType.Add), 1);

            var elementType = variable.InferIndexedType(1);

            variable.EnsureTypeCompatibility(arguments[0].InferTypeName(), elementType);

            variant.Add(arguments[0]);

            return LongType.Zero;
        }

        internal IVariant VisitRemoveAt(ref VariableDefinition variable, IVariant[] arguments)
        {
            arguments.EnsureArgumentCount(nameof(ListType.RemoveAt), 1);

            var index = arguments[0].ToIntegerIndex();

            if ((uint)index >= (uint)variant.Count)
            {
                throw new InvalidOperationException("List index is out of range.");
            }

            variant.RemoveAt(index);

            variable.OverrideVariant(variant);

            return LongType.Zero;
        }

        internal IVariant VisitInsert(ref VariableDefinition variable, IVariant[] arguments)
        {
            arguments.EnsureArgumentCount(nameof(ListType.Insert), 2);

            var index = arguments[0].ToIntegerIndex();

            if ((uint)index >= (uint)variant.Count)
            {
                throw new InvalidOperationException("List index is out of range.");
            }

            var elementType = variable.InferIndexedType(1);

            variable.EnsureTypeCompatibility(arguments[1].InferTypeName(), elementType);

            variant.Insert(index, arguments[1]);

            variable.OverrideVariant(variant);

            return LongType.Zero;
        }

        internal IVariant VisitClear(ref VariableDefinition variable, IVariant[] arguments)
        {
            arguments.EnsureArgumentCount(nameof(ListType.Clear), 0);

            variant.Clear();

            variable.OverrideVariant(variant);

            return LongType.Zero;
        }

        internal IVariant VisitSize(IVariant[] arguments)
        {
            arguments.EnsureArgumentCount("Size", 0);

            return new LongType(variant.Count);
        }
    }

    extension(IVariant[] arguments)
    {
        void EnsureArgumentCount(string method, int expected)
        {
            if (arguments.Length != expected)
            {
                throw new InvalidOperationException($"{method} expects {expected} argument(s) but received {arguments.Length}.");
            }
        }
    }
}