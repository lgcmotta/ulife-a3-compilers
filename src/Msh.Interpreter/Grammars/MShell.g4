grammar MShell;

prog: item* EOF;

item
  : functionDef                                         # FuncDefStatement
  | stat                                                # Statement
  ;

functionDef
  : type ID_PASCAL '(' paramList? ')' block
  | listType ID_PASCAL '(' paramList? ')' block
  ;

paramList
  : param (',' param)*
  ;

param
  : type ID
  ;

listType
  : type '[' ']'
  ;

type
  : INT                                                 # TypeInt
  | DOUBLE                                              # TypeDouble
  | DECIMAL                                             # TypeDecimal
  | BOOL                                                # TypeBool
  | STRING                                              # TypeString
  | VOID                                                # TypeVoid
  ;

stat
  : varDecl                                             # VarDeclStatement
  | assignment ';'                                      # AssignStatement
  | postfixStmt                                         # PostfixStatement
  | prefixStmt                                          # PrefixStatement
  | compoundAssignmentStmt                              # CompoundAssignmentStatement
  | callStmt                                            # CallStatement
  | returnStmt                                          # ReturnStatement
  | ifStmt                                              # IfStatement
  | whileStmt                                           # WhileStatement
  | doWhileStmt                                         # DoWhileStatement
  | forStmt                                             # ForStatement
  | block                                               # BlockStatement
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

block
  : '{' stat* '}'
  ;

varDecl
  : type ID '=' expr ';'                                # VarDeclTypedInit
  | type ID ';'                                         # VarDeclTypedEmpty
  | VAR ID '=' expr ';'                                 # VarDeclInferred
  | type '[' ']' ID '=' listLiteral ';'                 # VarDeclTypedListLiteral
  | type '[' ']' ID '=' expr ';'                        # VarDeclTypedListExpr
  ;

varDeclNoSemi
  : type ID '=' expr                                    # VarDeclInlineTyped
  | VAR ID '=' expr                                     # VarDeclInlineInferred
  ;

assignment
  : ID '=' expr                                         # AssignVariable
  | listAccess '=' expr                                 # AssignListElement
  ;

statementOrBlock
  : block
  | stat
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

postfixStmt
  : postfixExpr ';'
  ;

prefixStmt
  : prefixExpr ';'
  ;

compoundAssignmentStmt
  : compoundAssignmentExpr ';'
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
  | prefixStmt                                          # PrefixExpression
  | compoundAssignmentStmt                              # CompoundAssignmentExpression
  | WRITE '(' argList? ')'                              # WriteExpression
  | READ '(' argList? ')'                               # ReadExpression
  | ID_PASCAL '(' argList? ')'                          # CallExpression
  | listCall                                            # ListCallExpression
  | listLiteral                                         # ListLiteralExpression
  | listAccess                                          # ListAccessExpression
  | ID                                                  # VarExpression
  | '(' expr ')'                                        # ParensExpression
  ;

listLiteral
  : '[' expr (',' expr)* ']'
  | '[' ']'
  ;

listAccess
  : ID '[' expr ']'
  ;

listCall
  : instance=ID '.' method=(ID | ID_PASCAL) '(' argList? ')'
  ;

argList
  : expr (',' expr)*
  ;

INT: 'int';
DOUBLE: 'double';
DECIMAL: 'decimal';
BOOL: 'bool';
STRING: 'string';
VOID: 'void';
WRITE: 'Write';
READ: 'Read';
VAR: 'var';

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