using Antlr4BuildTasks;

using Msh.Interpreter.Extensions;
using Msh.StandardLibrary.Types;

namespace Msh.Interpreter.Visitors;

public sealed partial class MShellVisitor
{
    public override IVariant VisitListLiteralExpression(MShellParser.ListLiteralExpressionContext context)
    {
        var expressions = context.list().expression();

        var items = expressions is { Length: > 0 }
            ? expressions.Select(Visit).ToList()
            : [];

        var variant = new ListType(items);

        _ = variant.InferTypeName();

        return variant;
    }

    public override IVariant VisitIndexer(MShellParser.IndexerContext context)
    {
        string name = context.ID().GetText();

        var variable = _context.ResolveVariable(name);

        var current = variable.Variant;

        foreach (var indexExpression in context.expression())
        {
            if (current is not ListType list)
            {
                throw new InvalidOperationException($"'{name}' is not a list.");
            }

            var index = Visit(indexExpression).ToIntegerIndex();

            if ((uint)index >= (uint)list.Count)
            {
                throw new InvalidOperationException("List index is out of range.");
            }

            current = list[index];
        }

        return current;
    }

    public override IVariant VisitListMethod(MShellParser.ListMethodContext context)
    {
        var name = context.instance.Text;

        var variable = _context.ResolveVariable(name);

        if (variable.Variant is not ListType list)
        {
            throw new InvalidOperationException($"'{name}' is not a list.");
        }

        var arguments = context.arguments()?.expression().Select(Visit).ToArray() ?? [];

        var method = context.method.Text;

        return method switch
        {
            nameof(ListType.Add) => list.VisitAdd(ref variable, arguments),
            nameof(ListType.RemoveAt) => list.VisitRemoveAt(ref variable, arguments),
            nameof(ListType.Insert) => list.VisitInsert(ref variable, arguments),
            nameof(ListType.Clear) => list.VisitClear(ref variable, arguments),
            "Size" => list.VisitSize(arguments),
            _ => throw new InvalidOperationException($"Unknown list method '{method}'.")
        };
    }
}