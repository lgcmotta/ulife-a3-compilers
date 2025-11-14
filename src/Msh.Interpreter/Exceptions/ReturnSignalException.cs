using Msh.StandardLibrary.Types;

namespace Msh.Interpreter.Exceptions;

internal class ReturnSignalException(IVariant? variant) : Exception
{
    internal IVariant? Variant { get; } = variant;
}