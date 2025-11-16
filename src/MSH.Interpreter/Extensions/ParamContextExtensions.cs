using Antlr4BuildTasks;

using Msh.Interpreter.Definitions;

namespace Msh.Interpreter.Extensions;

internal static class ParamContextExtensions
{
    extension(MShellParser.ParamListContext? context)
    {
        internal ParameterDefinition[] ToParameterDefinitions()
        {
            if (context is null) return [];

            var parameters = context.param() is not null
                ? context.param()
                    .Select(ctx => ctx.ToParameterDefinition())
                    .ToArray()
                : [];

            return parameters;
        }
    }

    extension(MShellParser.ParamContext context)
    {
        internal ParameterDefinition ToParameterDefinition()
        {
            var type = context.type().NormalizeType();

            return new ParameterDefinition(type, context.ID().GetText());
        }
    }
}