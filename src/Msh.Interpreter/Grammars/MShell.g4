grammar MShell;

prog: item* EOF;

item
  : functionDef                                         # FuncDefStatement
  | stat                                                # Statement
  ;

functionDef
  : type ID_PASCAL '(' paramList? ')' block             # TypeFunctionDefinition
  ;

paramList
  : param (',' param)*
  ;

param
  : type ID
  ;

type
  : baseType ('[' ']')*                                 # TypeCompositeBase
  | VOID                                                # TypeVoid
  ;

baseType
  : INT                                                 # TypeInt
  | DOUBLE                                              # TypeDouble
  | DECIMAL                                             # TypeDecimal
  | BOOL                                                # TypeBool
  | STRING                                              # TypeString
  ;

stat
  : varDecl                                             # VarDeclStatement
  | assignment ';'                                      # AssignStatement
  | callStmt                                            # CallStatement
  | returnStmt                                          # ReturnStatement
  | ifStmt                                              # IfStatement
  | whileStmt                                           # WhileStatement
  | doWhileStmt                                         # DoWhileStatement
  | forStmt                                             # ForStatement
  | block                                               # BlockStatement
  ;

block
  : '{' stat* '}'
  ;

statementOrBlock
  : block
  | stat
  ;

varDecl
  : type ID '=' expr ';'                                # VarDeclTypedInit
  | type ID ';'                                         # VarDeclTypedEmpty
  | VAR ID '=' expr ';'                                 # VarDeclInferred
  ;

varDeclNoSemi
  : type ID '=' expr                                    # VarDeclInlineTyped
  | VAR ID '=' expr                                     # VarDeclInlineInferred
  ;

assignment
  : ID '=' expr                                         # AssignVariable
  | listAccess '=' expr                                 # AssignListElement
  | postfixExpr                                         # AssignPostfix
  | prefixExpr                                          # AssignPrefix
  | compoundAssignmentExpr                              # AssignCompound
  ;

listAccess
  : ID ('[' expr ']')+
  ;

listLiteral
  : '[' expr (',' expr)* ']'
  | '[' ']'
  ;

listCall
  : instance=ID '.' method=(ID | ID_PASCAL) '(' argList? ')'
  ;

callStmt
  : WRITE '(' argList? ')' ';'                          # WriteStatement
  | READ '(' argList? ')' ';'                           # ReadStatement
  | ID_PASCAL '(' argList? ')' ';'                      # UserCallStatement
  | listCall ';'                                        # ListCallStatement
  ;

returnStmt
  : 'return' expr? ';'
  ;

ifStmt
  : 'if' '(' cond=expr ')' thenPart=statementOrBlock elseIfClause* ('else' elsePart=statementOrBlock)?
  ;

elseIfClause
  : 'else' 'if' '(' cond=expr ')' body=statementOrBlock
  ;

whileStmt
  : 'while' '(' cond=expr ')' body=statementOrBlock
  ;

doWhileStmt
  : 'do' body=statementOrBlock 'while' '(' cond=expr ')' ';'
  ;

forStmt
  : 'for' '(' init=forInit? ';' cond=forCond? ';' iter=forIter? ')' body=statementOrBlock
  ;

forInit
  : varDeclNoSemi                                       # ForInitDecl
  | assignment                                          # ForInitAssignment
  ;

forCond
  : expr
  ;

forIter
  : assignment
  | expr
  ;

expr
  : 'if' '(' expr ')' expr 'else' expr                  # IfExpression
  | left=expr '?' then=expr ':' else=expr               # TernaryExpression
  | left=expr '||' right=expr                           # OrExpression
  | left=expr '&&' right=expr                           # AndExpression
  | left=expr op=(EQ|NEQ|LT|LE|GT|GE) right=expr        # CompareExpression
  | <assoc=right> left=expr op=POW right=expr           # PowExpression
  | SUB expr                                            # UnaryMinusExpression
  | left=expr op=(MUL|DIV) right=expr                   # MulDivExpression
  | left=expr op=(ADD|SUB) right=expr                   # AddSubExpression
  | BOOL_LIT                                            # BoolExpression
  | NUMBER_DECIMAL                                      # DecimalExpression
  | NUMBER_DOUBLE                                       # DoubleExpression
  | NUMBER_INT                                          # IntegerExpression
  | STR_LIT                                             # StringExpression
  | postfixExpr                                         # PostfixExpression
  | prefixExpr                                          # PrefixExpression
  | compoundAssignmentExpr                              # CompoundAssignmentExpression
  | WRITE '(' argList? ')'                              # WriteExpression
  | READ '(' argList? ')'                               # ReadExpression
  | ID_PASCAL '(' argList? ')'                          # CallExpression
  | listCall                                            # ListCallExpression
  | listLiteral                                         # ListLiteralExpression
  | listAccess                                          # ListAccessExpression
  | ID                                                  # VarExpression
  | '(' expr ')'                                        # ParensExpression
  ;

postfixExpr
  : ID INC                                              # PostfixIncrement
  | ID DEC                                              # PostfixDecrement
  ;

prefixExpr
  : INC ID                                              # PrefixIncrement
  | DEC ID                                              # PrefixDecrement
  ;

compoundAssignmentExpr
  : ID (CA_ADD|CA_SUB|CA_MUL|CA_DIV|CA_MOD) expr        # CompoundAssignment
  ;

argList
  : expr (',' expr)*
  ;

INT: 'int';
DOUBLE: 'double';
DECIMAL: 'decimal';
BOOL: 'bool';
STRING: 'string';
OBJECT: 'object';
VOID: 'void';
WRITE: 'Write';
READ: 'Read';
VAR: 'var';
NULL: 'null';

NUMBER_DECIMAL: [0-9]+ '.' [0-9]+ [mM];
NUMBER_DOUBLE:  [0-9]+ '.' [0-9]+ ([dD])?;
NUMBER_INT:     [0-9]+;
BOOL_LIT: 'true' | 'false';
STR_LIT: '"' ( ~["\\\r\n] | '\\' . )* '"';

ID_PASCAL: [A-Z_][a-zA-Z_0-9]*;
ID: [a-zA-Z_][a-zA-Z_0-9]*;

WS: [ \t\r\n]+ -> skip;
COMMENT: '//' ~[\r\n]* -> skip;

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

EQ: '==';
NEQ: '!=';
LT: '<';
LE: '<=';
GT: '>';
GE: '>=';