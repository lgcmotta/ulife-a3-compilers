using Antlr4.Runtime.Tree;

using Msh.Interpreter.Contexts;
using Msh.Interpreter.Definitions;
using Msh.StandardLibrary.Types;

namespace Msh.Interpreter.Scopes;

internal class FunctionScope : Dictionary<string, FunctionDefinition>
{
    private readonly FunctionContext _context;

    public FunctionScope(FunctionContext context)
    {
        _context = context;
    }

    internal IVariant InvokeFunction(string name, IVariant[] args, Func<IParseTree, IVariant> visit)
    {

    }
}