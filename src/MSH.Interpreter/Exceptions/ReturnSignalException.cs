using Msh.StandardLibrary.Types;

namespace Msh.Interpreter.Exceptions;

public class ReturnSignalException(IVariant? variant) : Exception
{
    internal IVariant? Variant { get; } = variant;
}