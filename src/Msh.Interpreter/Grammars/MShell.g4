grammar MShell;

prog: item* EOF;

item
    : function                                                       # FunctionDefinition
    | statement                                                      # StatementDefinition
    ;

function
    : type ID_PASCAL '(' paramList? ')' block
    ;

paramList
    : param (',' param)*
    ;

param
    : type ID
    ;

block
    : '{' statement* '}'
    ;

statement
    : variable
    | assignment ';'
    | return
    | expression ';'
    | ifStatement
    | whileStatement
    | doWhileStatement
    | forStatement
    | invoke ';'
    ;

return
    : 'return' expression? ';'
    ;

ifStatement
    : 'if' '(' condition=expression ')' then=statementOrBlock elseIfStatement* ('else' else=statementOrBlock)?
    ;

elseIfStatement
    : 'else' 'if' '(' condition=expression ')' body=statementOrBlock
    ;

whileStatement
    : 'while' '(' condition=expression ')' body=statementOrBlock
    ;

doWhileStatement
    : 'do' body=statementOrBlock 'while' '(' condition=expression ')' ';'
    ;

forStatement
    : 'for' '(' init=variableInline ';' condition=expression ';' iteration ')' body=statementOrBlock
    ;

iteration
    : assignment
    | expression
    ;

variable
    : type ID ';'                                                       # VariableDeclarationDefault
    | variableInline ';'                                                # VariableDeclaration
    ;

variableInline
    : type ID ASSIGN expression                                         # StronglyTypedVariable
    | VAR ID ASSIGN expression                                          # InferredTypedVariable
    ;

assignment
    : target ASSIGN expression                                          # DefaultAssignment
    | target (CA_ADD|CA_SUB|CA_MUL|CA_DIV|CA_MOD) expression            # CompoundAssignment
    | (INC|DEC) target                                                  # PrefixAssignment
    | target (INC|DEC)                                                  # PostfixAssignment
    ;

target
    : ID
    | indexer
    ;

expression
    : ID                                                                # VarExpression
    | left=expression '?' then=expression ':' else=expression           # TernaryExpression
    | left=expression OR right=expression                               # OrExpression
    | left=expression AND right=expression                              # AndExpression
    | left=expression operator=(EQ|NEQ|LT|LTE|GT|GTE) right=expression  # CompareExpression
    | <assoc=right> left=expression operator=POW right=expression       # PowExpression
    | SUB expression                                                    # UnaryMinusExpression
    | left=expression operation=(MUL|DIV) right=expression              # MulDivExpression
    | left=expression operation=(ADD|SUB) right=expression              # AddSubExpression
    | left=expression operation=MOD right=expression                    # ModulusExpression
    | STRING_LITERAL                                                    # StringExpression
    | BOOL_LITERAL                                                      # BoolExpression
    | INT_LITERAL                                                       # IntegerExpression
    | DOUBLE_LITERAL                                                    # DoubleExpression
    | DECIMAL_LITERAL                                                   # DecimalExpression
    | invoke                                                            # InvokeExpression
    | list                                                              # ListLiteralExpression
    | indexer                                                           # IndexerExpression
    | listMethod                                                        # ListMethodExpression
    | '(' expression ')'                                                # ParenthesisExpression
    ;

invoke
    : ID_PASCAL '(' arguments ')'                                       # InvokeFunctionExpression
    | READ '(' ')'                                                      # ReadValueExpression
    | WRITE '(' arguments? ')'                                          # WriteValueExpression
    ;

arguments
    : expression (',' expression)*
    ;

list
    : '[' expression (',' expression)* ']'
    | '[' ']'
    ;

indexer
    : ID ('[' expression ']')+
    ;

listMethod
    : instance=ID '.' method=ID_PASCAL '(' arguments? ')'
    ;

statementOrBlock
    : statement
    | block
    ;

type
    : baseType ('[' ']')*                                               # TypeCompositeBase
    | VOID                                                              # TypeVoid
    ;

baseType
  : INT
  | DOUBLE
  | DECIMAL
  | BOOL
  | STRING
  | OBJECT
  ;

// Types:
INT: 'int';
DOUBLE: 'double';
DECIMAL: 'decimal';
BOOL: 'bool';
STRING: 'string';
OBJECT: 'object';
VOID: 'void';
VAR: 'var';

// Standard IO
WRITE: 'Write';
READ: 'Read';

// Literals
DECIMAL_LITERAL: [0-9]+ '.' [0-9]+ [mM];
DOUBLE_LITERAL:  [0-9]+ '.' [0-9]+ ([dD])?;
INT_LITERAL:     [0-9]+;
BOOL_LITERAL: 'true' | 'false';
STRING_LITERAL: '"' ( ~["\\\r\n] | '\\' . )* '"';

// Identifiers
ID_PASCAL: [A-Z_][a-zA-Z_0-9]*;
ID: [a-zA-Z_][a-zA-Z_0-9]*;

// White Spaces
WS: [ \t\r\n]+ -> skip;

// Comments
COMMENT: '//' ~[\r\n]* -> skip;

// Operands
ASSIGN: '=';
ADD: '+';
SUB: '-';
MUL: '*';
DIV: '/';
POW: '^';
MOD: '%';
INC: '++';
DEC: '--';
CA_ADD: '+=';
CA_SUB: '-=';
CA_MUL: '*=';
CA_DIV: '/=';
CA_MOD: '%=';
OR: '||';
AND: '&&';
EQ: '==';
NEQ: '!=';
LT: '<';
LTE: '<=';
GT: '>';
GTE: '>=';