using Antlr4BuildTasks;

using Msh.Interpreter.Definitions;
using Msh.Interpreter.Exceptions;
using Msh.Interpreter.Extensions;
using Msh.StandardLibrary.Types;

namespace Msh.Interpreter.Visitors;

public sealed partial class MShellVisitor
{
    public override IVariant VisitProg(MShellParser.ProgContext context)
    {
        foreach (var item in context.item())
        {
            switch (item)
            {
                case MShellParser.FunctionDefinitionContext funcDef:
                    Visit(funcDef);
                    continue;
                case MShellParser.StatementDefinitionContext statement:
                    _context.Statements.Enqueue(statement.statement());
                    continue;
            }
        }

        var result = ExecuteProgram();

        _context.Statements.Clear();

        return result;
    }

    public override IVariant VisitFunction(MShellParser.FunctionContext context)
    {
        var name = context.ID_PASCAL().GetText();

        if (_context.Functions.ContainsKey(name))
        {
            throw new InvalidOperationException($"Function '{name}' is already defined.");
        }

        var returnType = context.type().NormalizeType();

        var parameters = context.paramList().ToParameterDefinitions();

        _context.Functions[name] = new FunctionDefinition(returnType, parameters, context.block());

        return LongType.Zero;
    }

    public override IVariant VisitBlock(MShellParser.BlockContext context)
    {
        return _context.ExecuteBlock(context, null, Visit);
    }

    public override IVariant VisitStatementOrBlock(MShellParser.StatementOrBlockContext context)
    {
        return context.block() is { } block ? VisitBlock(block) : Visit(context.statement());
    }

    private IVariant ExecuteProgram()
    {
        if (_context.Statements.Count is 0)
        {
            return LongType.Zero;
        }

        _context.Functions.Scope.Push("Main");

        try
        {
            while (_context.Statements.TryDequeue(out var statement))
            {
                _ = Visit(statement);
            }

            return LongType.Zero;
        }
        catch (ReturnSignalException exception)
        {
            return exception.Variant ?? LongType.Zero;
        }
        finally
        {
            _context.Functions.Scope.Pop();
        }
    }
}