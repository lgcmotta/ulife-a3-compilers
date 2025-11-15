using Antlr4BuildTasks;

using Msh.Interpreter.Definitions;
using Msh.Interpreter.Extensions;
using Msh.StandardLibrary.Types;

namespace Msh.Interpreter.Visitors;

public sealed partial class MShellVisitor
{
    public override IVariant VisitVariableDeclarationDefault(MShellParser.VariableDeclarationDefaultContext context)
    {
        var type = context.type().NormalizeType();

        if (type is "void")
        {
            throw new InvalidOperationException("Variables cannot be void.");
        }

        var name = context.ID().GetText();

        var scope = _context.Variables.Peek();

        if (scope.ContainsKey(name))
        {
            throw new InvalidOperationException($"Identifier '{name}' already exists in this scope.");
        }

        var value = context.type().Default();

        scope[name] = new VariableDefinition(type, value);

        return LongType.Zero;
    }

    public override IVariant VisitStronglyTypedVariable(MShellParser.StronglyTypedVariableContext context)
    {
        var type = context.type().NormalizeType();

        if (type is "void")
        {
            throw new InvalidOperationException("Variables cannot be void.");
        }

        var name = context.ID().GetText();

        var scope = _context.Variables.Peek();

        if (scope.ContainsKey(name))
        {
            throw new InvalidOperationException($"Identifier '{name}' already exists in this scope.");
        }

        scope[name] = new VariableDefinition(type, Visit(context.expression()));

        return LongType.Zero;
    }

    public override IVariant VisitInferredTypedVariable(MShellParser.InferredTypedVariableContext context)
    {
        var name = context.ID().GetText();

        var scope = _context.Variables.Peek();

        if (scope.ContainsKey(name))
        {
            throw new InvalidOperationException($"Identifier '{name}' already exists in this scope.");
        }

        var value = Visit(context.expression());

        var type = value is not null
            ? value.InferTypeName()
            : throw new InvalidOperationException("Cannot infer type from null.");

        scope[name] = new VariableDefinition(type, value);

        return LongType.Zero;
    }

    public override IVariant VisitDefaultAssignment(MShellParser.DefaultAssignmentContext context)
    {
        var value = Visit(context.expression());
        var target = context.target();

        if (target.indexer() is { } indexer)
        {
            IndexerAssignment(indexer, Visit(context.expression()));
        }

        var name = target.ID().GetText();

        var variable = _context.ResolveVariable(name);

        variable.EnsureTypeCompatibility(value.InferTypeName());

        variable.OverrideVariant(value);

        return LongType.Zero;
    }

    private void IndexerAssignment(MShellParser.IndexerContext indexerContext, IVariant newValue)
    {
        var variable = _context.ResolveVariable(indexerContext.ID().GetText());

        var indices = indexerContext.expression();

        if (indices.Length is 0)
        {
            throw new InvalidOperationException("Indexer must have at least one index.");
        }

        var parentList = ResolveTargetList(variable, indices);

        var lastIndex = Visit(indices[^1]).ToIntegerIndex();

        if (lastIndex < 0 || lastIndex >= parentList.Items.Count)
        {
            throw new InvalidOperationException("List index is out of range.");
        }

        var expectedType = variable.InferIndexedType(indices.Length);

        variable.EnsureTypeCompatibility(newValue.InferTypeName(), expectedType);

        parentList.Items[lastIndex] = newValue;
    }

    private ListType ResolveTargetList(VariableDefinition variable, MShellParser.ExpressionContext[] indexes)
    {
        IVariant current = variable.Variant;

        ListType? parentList = null;

        for (var i = 0; i < indexes.Length - 1; i++)
        {
            if (current is not ListType list)
            {
                throw new InvalidOperationException("Attempted to index a non-list value.");
            }

            parentList = list;

            var index = Visit(indexes[i]).ToIntegerIndex();

            current = parentList.Items[index];
        }

        parentList ??= current as ListType
                       ?? throw new InvalidOperationException("Target is not a list.");

        return parentList;
    }
}