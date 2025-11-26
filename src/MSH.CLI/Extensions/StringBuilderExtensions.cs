using System.Text;

namespace Msh.CommandLineInterface.Extensions;

internal static class StringBuilderExtensions
{
    extension(StringBuilder buffer)
    {
        internal bool IsStatementComplete()
        {
            var braceBalance = 0;

            char lastNonWhitespace = '\0';

            var chunks = buffer.GetChunks();

            while (chunks.MoveNext())
            {
                ReadOnlySpan<char> span = chunks.Current.Span;

                foreach (var ch in span)
                {
                    switch (ch)
                    {
                        case '{':
                            braceBalance++;
                            continue;
                        case '}':
                            braceBalance--;
                            continue;
                    }

                    if (!char.IsWhiteSpace(ch))
                    {
                        lastNonWhitespace = ch;
                    }
                }
            }

            return braceBalance <= 0 && lastNonWhitespace is ';' or '}';
        }
    }
}