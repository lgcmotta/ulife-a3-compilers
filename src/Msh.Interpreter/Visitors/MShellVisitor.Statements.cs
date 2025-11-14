using Antlr4BuildTasks;

using Msh.StandardLibrary.Types;

namespace Msh.Interpreter.Visitors;

public sealed partial class MShellVisitor
{
    public override IVariant VisitAssignStatement(MShellParser.AssignStatementContext context)
    {
        return Visit(context.assignment());
    }

    public override IVariant VisitBlockStatement(MShellParser.BlockStatementContext context)
    {
        return Visit(context.block());
    }

    public override IVariant VisitCallStatement(MShellParser.CallStatementContext context)
    {
        return Visit(context.callStmt());
    }

    public override IVariant VisitCompoundAssignmentStatement(MShellParser.CompoundAssignmentStatementContext context)
    {
        return Visit(context.compoundAssignmentStmt());
    }

    public override IVariant VisitDoWhileStatement(MShellParser.DoWhileStatementContext context)
    {
        return Visit(context.doWhileStmt());
    }

    public override IVariant VisitIfStatement(MShellParser.IfStatementContext context)
    {
        return Visit(context.ifStmt());
    }

    public override IVariant VisitForStatement(MShellParser.ForStatementContext context)
    {
        return Visit(context.forStmt());
    }

    public override IVariant VisitFuncDefStatement(MShellParser.FuncDefStatementContext context)
    {
        return Visit(context.functionDef());
    }

    public override IVariant VisitListCallStatement(MShellParser.ListCallStatementContext context)
    {
        return Visit(context.listCall());
    }

    public override IVariant VisitPostfixStatement(MShellParser.PostfixStatementContext context)
    {
        return Visit(context.postfixStmt());
    }

    public override IVariant VisitPrefixStatement(MShellParser.PrefixStatementContext context)
    {
        return Visit(context.prefixStmt());
    }

    public override IVariant VisitReadStatement(MShellParser.ReadStatementContext context)
    {
        // TODO: Invoke stdio read
        return base.VisitReadStatement(context);
    }

    public override IVariant VisitReturnStatement(MShellParser.ReturnStatementContext context)
    {
        return Visit(context.returnStmt());
    }

    public override IVariant VisitStatementOrBlock(MShellParser.StatementOrBlockContext context)
    {
        return context.block() is { } block ? Visit(block) : Visit(context.stat());
    }

    public override IVariant VisitStatement(MShellParser.StatementContext context)
    {
        return Visit(context.stat());
    }

    public override IVariant VisitVarDeclStatement(MShellParser.VarDeclStatementContext context)
    {
        return Visit(context.varDecl());
    }

    public override IVariant VisitUserCallStatement(MShellParser.UserCallStatementContext context)
    {
        // TODO: Invoke User defined function
        return base.VisitUserCallStatement(context);
    }

    public override IVariant VisitWhileStatement(MShellParser.WhileStatementContext context)
    {
        return Visit(context.whileStmt());
    }

    public override IVariant VisitWriteStatement(MShellParser.WriteStatementContext context)
    {
        // TODO: Invoke stdio write
        return base.VisitWriteStatement(context);
    }
}