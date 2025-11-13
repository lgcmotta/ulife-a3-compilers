namespace Msh.StandardLibrary.Mathematics.Exceptions;

public class OperationNotSupportedException(string name)
    : InvalidOperationException($"{name} requires argument(s) to be lists of scalars or scalar values.");