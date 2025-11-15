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

        if (cond is not BooleanType boolCond)
        {
            throw new InvalidOperationException("The 'if' statement condition must evaluate to a bool.");
        }

        if (boolCond)
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

            if (condition is not BooleanType boolCondition)
            {
                throw new InvalidOperationException("The 'while' statement condition must evaluate to a bool.");
            }

            if (!boolCondition.Value)
            {
                break;
            }

            last = Visit(context.body);
        }

        return last;
    }

    public override IVariant VisitDoWhileStatement(MShellParser.DoWhileStatementContext context)
    {
        IVariant last = LongType.Zero;

        do
        {
            var condition = Visit(context.condition);

            if (condition is not BooleanType boolCondition)
            {
                throw new InvalidOperationException("The 'while' statement condition must evaluate to a bool.");
            }

            if (!boolCondition.Value)
            {
                break;
            }

            last = Visit(context.body);
        } while (true);

        return last;
    }
}