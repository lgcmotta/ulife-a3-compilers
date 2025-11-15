using Antlr4BuildTasks;

using Msh.StandardLibrary.Operators;

namespace Msh.Interpreter.Extensions;

internal static class AssignmentContextExtensions
{
    extension<TContext>(TContext context) where TContext : MShellParser.AssignmentContext
    {
        internal string ResolveOperator()
        {
            return context switch
            {
                MShellParser.CompoundAssignmentContext ctx when ctx.CA_ADD() is not null => Operator.CompoundAdd,
                MShellParser.CompoundAssignmentContext ctx when ctx.CA_SUB() is not null => Operator.CompoundSubtract,
                MShellParser.CompoundAssignmentContext ctx when ctx.CA_MUL() is not null => Operator.CompoundMultiply,
                MShellParser.CompoundAssignmentContext ctx when ctx.CA_DIV() is not null => Operator.CompoundDivide,
                MShellParser.CompoundAssignmentContext ctx when ctx.CA_MOD() is not null => Operator.CompoundModulus,

                MShellParser.PrefixAssignmentContext ctx when ctx.INC() is not null => Operator.Increment,
                MShellParser.PrefixAssignmentContext ctx when ctx.DEC() is not null => Operator.Decrement,

                MShellParser.PostfixAssignmentContext ctx when ctx.INC() is not null => Operator.Increment,
                MShellParser.PostfixAssignmentContext ctx when ctx.DEC() is not null => Operator.Decrement,

                _ => throw new InvalidOperationException("Unknown operator.")
            };
        }
    }
}