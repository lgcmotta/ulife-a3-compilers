// using Antlr4BuildTasks;
//
// using Msh.Interpreter.Definitions;
// using Msh.Interpreter.Extensions;
// using Msh.StandardLibrary.Types;
//
// namespace Msh.Interpreter.Visitors;
//
// public sealed partial class MShellVisitor
// {
//     public override IVariant VisitFunctionDefinition(MShellParser.FunctionDefinitionContext context)
//     {
//         var name = context.function().ID_PASCAL().GetText();
//
//         if (_context.Functions.ContainsKey(name))
//         {
//             throw new InvalidOperationException($"Function '{name}' is already defined.");
//         }
//
//         var returnType = context.function().type().NormalizeType();
//
//         var parameters = context.function().paramList().ToParameterDefinitions();
//
//         _context.Functions[name] = new FunctionDefinition(returnType, parameters, context.function().block());
//
//         return LongType.Zero;
//     }
//
//     public override IVariant VisitTypeFunctionDefinition(MShellParser.FunctionDefinitionContext context)
//     {
//
//     }
//
//     public override IVariant VisitVarDeclTypedInit(MShellParser.VarDeclTypedInitContext context)
//     {
//         var type = context.type().NormalizeType();
//
//         if (type is "void")
//         {
//             throw new InvalidOperationException("Variables cannot be void.");
//         }
//
//         var name = context.ID().GetText();
//
//         var scope = _context.Variables.Peek();
//
//         if (scope.ContainsKey(name))
//         {
//             throw new InvalidOperationException($"Identifier '{name}' already exists in this scope.");
//         }
//
//         var value = Visit(context.expr()) ?? context.type().Default();
//
//         scope[name] = new VariableDefinition(type, value);
//
//         return LongType.Zero;
//     }
//
//     public override IVariant VisitVarDeclTypedEmpty(MShellParser.VarDeclTypedEmptyContext context)
//     {
//         var type = context.type().NormalizeType();
//
//         if (type is "void")
//         {
//             throw new InvalidOperationException("Variables cannot be void.");
//         }
//
//         var name = context.ID().GetText();
//
//         var scope = _context.Variables.Peek();
//
//         if (scope.ContainsKey(name))
//         {
//             throw new InvalidOperationException($"Identifier '{name}' already exists in this scope.");
//         }
//
//         var value = context.type().Default();
//
//         scope[name] = new VariableDefinition(type, value);
//
//         return LongType.Zero;
//     }
//
//     public override IVariant VisitVarDeclInferred(MShellParser.VarDeclInferredContext context)
//     {
//         var name = context.ID().GetText();
//
//         var scope = _context.Variables.Peek();
//
//         if (scope.ContainsKey(name))
//         {
//             throw new InvalidOperationException($"Identifier '{name}' already exists in this scope.");
//         }
//
//         var value = Visit(context.expr());
//
//         if (value is null)
//         {
//             throw new InvalidOperationException("Cannot infer type from null.");
//         }
//
//         var type = value.InferTypeName();
//
//         scope[name] = new VariableDefinition(type, value);
//
//         return LongType.Zero;
//     }
//
//     public override IVariant VisitVarDeclInlineTyped(MShellParser.VarDeclInlineTypedContext context)
//     {
//         var type = context.type().NormalizeType();
//
//         if (type is "void")
//         {
//             throw new InvalidOperationException("Variables cannot be void.");
//         }
//
//         var name = context.ID().GetText();
//
//         var scope = _context.Variables.Peek();
//
//         if (scope.ContainsKey(name))
//         {
//             throw new InvalidOperationException($"Identifier '{name}' already exists in this scope.");
//         }
//
//         var value = Visit(context.expr()) ?? context.type().Default();
//
//         scope[name] = new VariableDefinition(type, value);
//
//         return LongType.Zero;
//     }
//
//     public override IVariant VisitVarDeclInlineInferred(MShellParser.VarDeclInlineInferredContext context)
//     {
//         var name = context.ID().GetText();
//
//         var scope = _context.Variables.Peek();
//
//         if (scope.ContainsKey(name))
//         {
//             throw new InvalidOperationException($"Identifier '{name}' already exists in this scope.");
//         }
//
//         var value = Visit(context.expr());
//
//         if (value is null)
//         {
//             throw new InvalidOperationException("Cannot infer type from null.");
//         }
//
//         var type = value.InferTypeName();
//
//         scope[name] = new VariableDefinition(type, value);
//
//         return LongType.Zero;
//     }
// }