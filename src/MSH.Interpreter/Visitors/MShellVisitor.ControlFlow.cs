using Antlr4BuildTasks;

using Msh.Interpreter.Exceptions;
using Msh.StandardLibrary.Types;

namespace Msh.Interpreter.Visitors;

public sealed partial class MShellVisitor
{
    public override IVariant VisitReturn(MShellParser.ReturnContext context)
    {
        if (_context.Functions.Count is 0)
        {
            throw new InvalidOperationException("The keyword 'return' is only allowed inside a function.");
        }

        var value = context.expression() is { } expression ? Visit(expression) : LongType.Zero;

        throw new ReturnSignalException(value);
    }

    public override IVariant VisitIfStatement(MShellParser.IfStatementContext context)
    {
        var cond = Visit(context.condition);

        if (cond is not BooleanType boolean)
        {
            throw new InvalidOperationException("The 'if' statement condition must evaluate to a bool.");
        }

        if (boolean)
        {
            return Visit(context.then);
        }

        foreach (var clause in context.elseIfStatement())
        {
            if (Visit(clause.condition) is not BooleanType boolClause)
            {
                throw new InvalidOperationException("The 'else if' statement condition must evaluate to a bool.");
            }

            if (boolClause)
            {
                return Visit(clause.body);
            }
        }

        return context.@else is not null ? Visit(context.@else) : BooleanType.False;
    }

    public override IVariant VisitWhileStatement(MShellParser.WhileStatementContext context)
    {
        IVariant last = LongType.Zero;

        while (true)
        {
            var condition = Visit(context.condition);

            if (condition is not BooleanType boolean)
            {
                throw new InvalidOperationException("The 'while' statement condition must evaluate to a bool.");
            }

            if (!boolean.Value)
            {
                break;
            }

            last = Visit(context.body);
        }

        return last;
    }

    public override IVariant VisitDoWhileStatement(MShellParser.DoWhileStatementContext context)
    {
        IVariant last;

        do
        {
            last = Visit(context.body);

            var condition = Visit(context.condition);

            if (condition is not BooleanType boolean)
            {
                throw new InvalidOperationException("The 'while' statement condition must evaluate to a bool.");
            }

            if (!boolean.Value)
            {
                break;
            }

        } while (true);

        return last;
    }

    public override IVariant VisitForStatement(MShellParser.ForStatementContext context)
    {
        _context.Variables.Push([]);

        try
        {
            if (context.init is not null)
            {
                Visit(context.init);
            }

            IVariant last = LongType.Zero;

            while (EvaluateCondition())
            {
                last = Visit(context.body);

                Visit(context.iteration());
            }

            return last;

            bool EvaluateCondition()
            {
                if (context.condition is null) return false;

                var condition = Visit(context.condition);

                return condition is not BooleanType boolean
                    ? throw new InvalidOperationException("The condition for the 'for' statement must evaluate to a bool.")
                    : boolean.Value;
            }
        }
        finally
        {
            _context.Variables.Pop();
        }
    }
}