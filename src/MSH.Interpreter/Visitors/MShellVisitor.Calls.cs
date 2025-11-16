using System.Text;

using Antlr4BuildTasks;

using Msh.Interpreter.Definitions;
using Msh.Interpreter.Exceptions;
using Msh.Interpreter.Extensions;
using Msh.Interpreter.Scopes;
using Msh.StandardLibrary.Mathematics;
using Msh.StandardLibrary.Types;

namespace Msh.Interpreter.Visitors;

public sealed partial class MShellVisitor
{
    public override IVariant VisitInvokeFunctionExpression(MShellParser.InvokeFunctionExpressionContext context)
    {
        var name = context.ID_PASCAL().GetText();

        var arguments = context.arguments() is not null
            ? context.arguments().expression().Select(Visit).ToArray()
            : [];

        if (Functions.Registry.TryGetValue(name, out var function))
        {
            return function(arguments);
        }

        if (!_context.Functions.TryGetValue(name, out var def))
        {
            throw new InvalidOperationException($"Function '{name}' is not defined.");
        }

        if (def.Parameters.Count != arguments.Length)
        {
            throw new InvalidOperationException($"Function '{name}' expects {def.Parameters.Count} argument(s) but received {arguments.Length}.");
        }

        var seed = new VariablesScope();

        for (var i = 0; i < def.Parameters.Count; i++)
        {
            (string type, string paramName) = def.Parameters[i];

            seed[paramName] = new VariableDefinition(type, arguments[i]);
        }

        _context.Functions.Scope.Push(def.ReturnType);

        try
        {
            var result = _context.ExecuteBlock(def.Body, seed, Visit);

            return def.ReturnType == "void" ? LongType.Zero : result;
        }
        catch (ReturnSignalException exception)
        {
            return def.ReturnType == "void" ? LongType.Zero : (exception.Variant ?? LongType.Zero);
        }
        finally
        {
            _context.Functions.Scope.Pop();
        }
    }

    public override IVariant VisitWriteValueExpression(MShellParser.WriteValueExpressionContext context)
    {
        var arguments = context.arguments() is not null
            ? context.arguments().expression().Select(Visit).ToArray()
            : [];

        var builder = new StringBuilder();

        foreach (var argument in arguments)
        {
            builder.Append(argument);
        }

        _terminal.WriteLine(builder.ToString());

        return LongType.Zero;
    }

    public override IVariant VisitReadValueExpression(MShellParser.ReadValueExpressionContext context)
    {
        var value = _terminal.ReadLine().Trim();

        return value.TryParseVariant(out var variant) && variant is not null
            ? variant
            : new StringType(value);
    }
}