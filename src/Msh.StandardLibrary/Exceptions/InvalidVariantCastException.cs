namespace Msh.StandardLibrary.Exceptions;

public class InvalidVariantCastException(string type)
    : InvalidCastException(message: $"Variant cannot be casted to type '{type}'");