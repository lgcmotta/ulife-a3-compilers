using Msh.Interpreter.Definitions;

namespace Msh.Interpreter.Extensions;

internal static class VariableDefinitionExtensions
{
    extension(VariableDefinition variable)
    {
        internal string InferIndexedType(int depth)
        {
            var type = variable.Type;

            for (var i = 0; i < depth; i++)
            {
                var suffix = type.LastIndexOf("[]", StringComparison.Ordinal);

                if (suffix < 0)
                {
                    throw new InvalidOperationException("Cannot apply indexer to a non-list type.");
                }

                type = type[..suffix];
            }

            return type;
        }

        internal void EnsureTypeCompatibility(string actual, string? expected = null)
        {
            if (!string.Equals(expected ?? variable.Type, actual, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Cannot assign type '{actual}' to '{variable.Type}'.");
            }
        }
    }
}