using Msh.StandardLibrary.Types;

namespace Msh.StandardLibrary.Mathematics.Exceptions;

public class UnsupportedOperationException(string name, Kind left, Kind right)
    : InvalidOperationException($"Unsupported operation: '{name}' between kinds '{left}' and '{right}'.");