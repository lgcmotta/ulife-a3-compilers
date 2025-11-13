grammar MShell;

prog: item* EOF;

item
  : functionDef                               # FuncDefStatement
  | stat                                      # Statement
  ;

functionDef
  : type ID_PASCAL '(' paramList? ')' block
  | arrayType ID_PASCAL '(' paramList? ')' block
  ;

paramList
  : param (',' param)*
  ;

param
  : type ID
  ;

type
  : INT                                       # TypeInt
  | DOUBLE                                    # TypeDouble
  | DECIMAL                                   # TypeDecimal
  | BOOL                                      # TypeBool
  | STRING                                    # TypeString
  | VOID                                      # TypeVoid
  ;

arrayType
  : type '[' ']'
  ;

stat
  : varDecl                                   # VarDeclStatement
  | assignment ';'                            # AssignStatement
  | postfixStmt                               # PostfixStatement
  | callStmt                                  # CallStatement
  | returnStmt                                # ReturnStatement
  | ifStmt                                    # IfStatement
  | whileStmt                                 # WhileStatement
  | doWhileStmt                               # DoWhileStatement
  | forStmt                                   # ForStatement
  | block                                     # BlockStatement
  ;

callStmt
  : WRITE '(' argList? ')' ';'                # WriteStatement
  | READ '(' argList? ')' ';'                 # ReadStatement
  | ID_PASCAL '(' argList? ')' ';'            # UserCallStatement
  | arrayCall ';'                             # ArrayCallStatement
  ;

returnStmt
  : 'return' expr? ';'
  ;

block
  : '{' stat* '}'
  ;

varDecl
  : type ID '=' expr ';'                      # VarDeclTypedInit
  | type ID ';'                               # VarDeclTypedEmpty
  | VAR ID '=' expr ';'                       # VarDeclInferred
  | type '[' ']' ID '=' arrayLiteral ';'      # VarDeclTypedArrayLiteral
  | type '[' ']' ID '=' expr ';'              # VarDeclTypedArrayExpr
  ;

varDeclNoSemi
  : type ID '=' expr                          # VarDeclInlineTyped
  | VAR ID '=' expr                           # VarDeclInlineInferred
  ;

assignment
  : ID '=' expr                               # AssignVariable
  | arrayAccess '=' expr                      # AssignArrayElement
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
  : varDeclNoSemi                           # ForInitDecl
  | assignment                              # ForInitAssignment
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

postfixExpr
  : ID INC                                    # PostfixIncrement
  | ID DEC                                    # PostfixDecrement
  ;

expr
  : 'if' '(' expr ')' expr 'else' expr                  # IfExpr
  | left=expr '?' then=expr ':' else=expr               # Ternary
  | left=expr '||' right=expr                           # Or
  | left=expr '&&' right=expr                           # And
  | left=expr op=(EQ|NEQ|LT|LE|GT|GE) right=expr        # Cmp
  | <assoc=right> left=expr op=POW right=expr           # Pow
  | SUB expr                                            # UnaryMinus
  | left=expr op=(MUL|DIV) right=expr                   # MulDiv
  | left=expr op=(ADD|SUB) right=expr                   # AddSub
  | BOOL_LIT                                            # Bool
  | NUMBER_DECIMAL                                      # Decimal
  | NUMBER_DOUBLE                                       # Double
  | NUMBER_INT                                          # Integer
  | STR_LIT                                             # String
  | postfixExpr                                         # Postfix
  | WRITE '(' argList? ')'                              # Write
  | READ '(' argList? ')'                               # Read
  | ID_PASCAL '(' argList? ')'                          # Call
  | arrayCall                                           # ArrayCallExpr
  | arrayLiteral                                        # ArrayLiteralExpr
  | arrayAccess                                         # ArrayAccessExpr
  | ID                                                  # Var
  | '(' expr ')'                                        # Parens
  ;

arrayLiteral
  : '[' expr (',' expr)* ']'
  | '[' ']'
  ;

arrayAccess
  : ID '[' expr ']'
  ;

arrayCall
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
INC: '++';
DEC: '--';

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

EQ: '==';
NEQ: '!=';
LT: '<';
LE: '<=';
GT: '>';
GE: '>=';