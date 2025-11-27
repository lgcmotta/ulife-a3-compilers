using System.Globalization;
using System.Text;

using Msh.Interpreter.Expressions;
using Msh.StandardLibrary.Types;

namespace Msh.Interpreter.Extensions;

using static InputParserPatterns;

internal static class InputParsingExtensions
{
    extension(string text)
    {
        internal bool TryParseVariant(out IVariant? variant)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                variant = StringType.Empty;
                return true;
            }

            if (text.TryParseList(out variant))
            {
                return true;
            }

            if (DecimalPattern.IsMatch(text))
            {
                var value = text.EndsWith("m", StringComparison.OrdinalIgnoreCase) ? text[..^1] : text;

                if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var dec))
                {
                    variant = new DecimalType(dec);
                    return true;
                }
            }

            if (DoublePattern.IsMatch(text))
            {
                var value = text.EndsWith("d", StringComparison.OrdinalIgnoreCase) ? text[..^1] : text;

                if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var dbl))
                {
                    variant = new DoubleType(dbl);
                    return true;
                }
            }

            if (LongPattern.IsMatch(text) && long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var lng))
            {
                variant = new LongType(lng);
                return true;
            }

            if (bool.TryParse(text, out var b))
            {
                variant = new BooleanType(b);
                return true;
            }

            variant = null;
            return false;
        }

        private bool TryParseList(out IVariant? variant)
        {
            var trimmed = text.Trim();

            if (!trimmed.StartsWith('[') || !trimmed.EndsWith(']'))
            {
                variant = null;
                return false;
            }

            var inner = trimmed[1..^1].Trim();

            if (string.IsNullOrWhiteSpace(inner))
            {
                variant = new ListType([]);
                return true;
            }

            var items = inner.SplitTopLevel();

            var parsedItems = new List<IVariant>(items.Count);

            foreach (var part in items.Select(item => item.Trim()))
            {
                if (part.TryParseVariant(out var parsed) && parsed is not null)
                {
                    parsedItems.Add((IVariant)parsed);
                    continue;
                }

                parsedItems.Add(new StringType(part));
            }

            var list = new ListType(parsedItems);

            _ = list.InferTypeName();

            variant = list;

            return true;
        }

        private List<string> SplitTopLevel()
        {
            var parts = new List<string>();
            var depth = 0;
            var builder = new StringBuilder();

            foreach (var ch in text)
            {
                switch (ch)
                {
                    case '[':
                        depth++;
                        builder.Append(ch);
                        break;
                    case ']':
                        depth--;
                        builder.Append(ch);
                        break;
                    case ',' when depth == 0:
                        parts.Add(builder.ToString());
                        builder.Clear();
                        break;
                    default:
                        builder.Append(ch);
                        break;
                }
            }

            if (builder is { Length: > 0 })
            {
                parts.Add(builder.ToString());
            }

            return parts;
        }
    }
}