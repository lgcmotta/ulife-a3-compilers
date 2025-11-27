using Antlr4BuildTasks;

using Msh.StandardLibrary.Types;

namespace Msh.Interpreter.Extensions;

internal static class TypedContextExtensions
{
    extension(MShellParser.TypeContext context)
    {
        internal string NormalizeType()
        {
            return context.GetText().ToLowerInvariant();
        }

        internal IVariant Default()
        {
            var type = context.NormalizeType();

            return type is "void"
                ? throw new InvalidOperationException("Type void has no default value.")
                : type switch
                {
                    "int" => LongType.Zero,
                    "double" => DoubleType.Zero,
                    "decimal" => DecimalType.Zero,
                    "bool" => BooleanType.False,
                    "string" => StringType.Empty,
                    "object" => ObjectType.Null,
                    _ when type.Contains('[') => ListType.Empty,
                    _ => throw new InvalidOperationException()
                };
        }
    }
}