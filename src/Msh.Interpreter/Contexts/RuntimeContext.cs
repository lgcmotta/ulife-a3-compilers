using Antlr4.Runtime.Tree;

using Antlr4BuildTasks;

using Msh.Interpreter.Definitions;
using Msh.Interpreter.Registries;
using Msh.Interpreter.Scopes;
using Msh.StandardLibrary.Types;

namespace Msh.Interpreter.Contexts;

internal class RuntimeContext
{
    internal VariablesRegistry Variables { get; } = [];

    internal FunctionsRegistry Functions { get; } = [];

    internal StatementScope Statements { get; } = [];

    public IVariant ExecuteBlock(MShellParser.BlockContext context,
        VariablesScope? seed,
        Func<IParseTree, IVariant> visit)
    {
        var scope = new VariablesScope();

        if (seed is not null)
        {
            foreach (var pair in seed)
            {
                scope[pair.Key] = new VariableDefinition(pair.Value.Type, pair.Value.Variant);
            }
        }

        Variables.Push(scope);

        try
        {
            var result = context.statement()
                .Aggregate(LongType.Zero, (_, next) => visit(next));

            return result;
        }
        finally
        {
            Variables.Pop();
        }
    }
}