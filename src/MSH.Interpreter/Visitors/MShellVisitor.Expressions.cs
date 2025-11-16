using System.Globalization;
using System.Text.RegularExpressions;

using Antlr4BuildTasks;

using Msh.Interpreter.Extensions;
using Msh.StandardLibrary.Mathematics;
using Msh.StandardLibrary.Types;

namespace Msh.Interpreter.Visitors;

public sealed partial class MShellVisitor
{
    public override IVariant VisitVarExpression(MShellParser.VarExpressionContext context)
    {
        return _context.ResolveVariable(context.ID().GetText()).Variant;
    }

    public override IVariant VisitTernaryExpression(MShellParser.TernaryExpressionContext context)
    {
        var condition = Visit(context.left).Cast<BooleanType>();

        return condition ? Visit(context.then) : Visit(context.@else);
    }

    public override IVariant VisitOrExpression(MShellParser.OrExpressionContext context)
    {
        return new BooleanType(Visit(context.left).Cast<BooleanType>().Value ||
                               Visit(context.right).Cast<BooleanType>().Value);
    }

    public override IVariant VisitAndExpression(MShellParser.AndExpressionContext context)
    {
        return new BooleanType(Visit(context.left).Cast<BooleanType>().Value &&
                               Visit(context.right).Cast<BooleanType>().Value);
    }

    public override IVariant VisitCompareExpression(MShellParser.CompareExpressionContext context)
    {
        var left = Visit(context.left);
        var right = Visit(context.right);

        return context.@operator.Type switch
        {
            MShellParser.EQ => new BooleanType(left.Equals(right)),
            MShellParser.NEQ => new BooleanType(!left.Equals(right)),
            MShellParser.LT => new BooleanType(left.CompareTo(right) < 0),
            MShellParser.LTE => new BooleanType(left.CompareTo(right) <= 0),
            MShellParser.GT => new BooleanType(left.CompareTo(right) > 0),
            MShellParser.GTE => new BooleanType(left.CompareTo(right) >= 0),
            _ => BooleanType.False
        };
    }

    public override IVariant VisitPowExpression(MShellParser.PowExpressionContext context)
    {
        return Visit(context.left).Pow(Visit(context.right));
    }

    public override IVariant VisitUnaryMinusExpression(MShellParser.UnaryMinusExpressionContext context)
    {
        return Visit(context.expression()).ReverseSign();
    }

    public override IVariant VisitMulDivExpression(MShellParser.MulDivExpressionContext context)
    {
        var left = Visit(context.left);

        var right = Visit(context.right);

        return context.operation.Type switch
        {
            MShellParser.MUL => left.Multiply(right),
            MShellParser.DIV => left.Divide(right),
            _ => LongType.Zero
        };
    }

    public override IVariant VisitAddSubExpression(MShellParser.AddSubExpressionContext context)
    {
        var left = Visit(context.left);

        var right = Visit(context.right);

        return context.operation.Type switch
        {
            MShellParser.ADD => left.Add(right),
            MShellParser.SUB => left.Subtract(right),
            _ => LongType.Zero
        };
    }

    public override IVariant VisitStringExpression(MShellParser.StringExpressionContext context)
    {
        var raw = context.STRING_LITERAL().GetText();

        var inner = raw is { Length: >= 2 } ? raw[1..^1] : string.Empty;

        var unescaped = Regex.Unescape(inner);

        var formatted = InterpolationPattern.Replace(unescaped, match =>
        {
            var token = match.Groups[1].Value.Trim();

            string? format = null;

            var colonIndex = token.LastIndexOf(':');

            if (colonIndex >= 0)
            {
                format = token[(colonIndex + 1)..].Trim();
                token = token[..colonIndex].Trim();
            }

            var value = ResolveInterpolationValue(token);

            return value.FormatInterpolationValue(format);
        });

        return new StringType(formatted);
    }

    public override IVariant VisitBoolExpression(MShellParser.BoolExpressionContext context)
    {
        var text = context.BOOL_LITERAL().GetText();

        return !string.IsNullOrWhiteSpace(text) && text.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase)
            ? BooleanType.True
            : BooleanType.False;
    }

    public override IVariant VisitIntegerExpression(MShellParser.IntegerExpressionContext context)
    {
        var text = context.INT_LITERAL().GetText();

        return long.TryParse(text, out var @long)
            ? new LongType(@long)
            : LongType.Zero;
    }

    public override IVariant VisitDoubleExpression(MShellParser.DoubleExpressionContext context)
    {
        var text = context.DOUBLE_LITERAL().GetText();

        return double.TryParse(text[..^1], CultureInfo.InvariantCulture, out var number)
            ? new DoubleType(number)
            : DoubleType.Zero;
    }

    public override IVariant VisitDecimalExpression(MShellParser.DecimalExpressionContext context)
    {
        var text = context.DECIMAL_LITERAL().GetText();

        return decimal.TryParse(text[..^1], CultureInfo.InvariantCulture, out var number)
            ? new DecimalType(number)
            : DecimalType.Zero;
    }

    public override IVariant VisitParenthesisExpression(MShellParser.ParenthesisExpressionContext context)
    {
        return Visit(context.expression());
    }

    private IVariant ResolveInterpolationValue(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new InvalidOperationException("Interpolation placeholder cannot be empty.");
        }

        var bracketIndex = token.IndexOf('[');

        var name = bracketIndex >= 0 ? token[..bracketIndex] : token;

        if (name.TryParseVariant(out var literal) && literal is not null)
        {
            return literal;
        }

        var variable = _context.ResolveVariable(name);

        if (bracketIndex < 0)
        {
            return variable.Variant;
        }

        var current = variable.Variant;

        foreach (Match match in IndexPattern.Matches(token[bracketIndex..]))
        {
            var index = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);

            if (current is not ListType list)
            {
                throw new InvalidOperationException($"'{name}' is not a list.");
            }

            if ((uint)index >= (uint)list.Items.Count)
            {
                throw new InvalidOperationException($"Index {index} is out of range for '{name}'.");
            }

            current = list.Items[index];
        }

        return current;
    }
}