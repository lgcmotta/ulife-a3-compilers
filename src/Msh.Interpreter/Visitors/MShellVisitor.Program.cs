using Antlr4BuildTasks;

using Msh.Interpreter.Exceptions;
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
                case MShellParser.FuncDefStatementContext funcDef:
                    Visit(funcDef);
                    continue;
                case MShellParser.StatementContext statement:
                    _context.Statements.Enqueue(statement.stat());
                    continue;
            }
        }

        var result = ExecuteProgram();

        _context.Statements.Clear();

        return result;
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