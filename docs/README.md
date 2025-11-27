# MShell Tutorial

MShell is a compact, strongly typed scripting language designed for clarity, consistency, and educational value.

It was created to demonstrate fundamental concepts of compilers, interpreters, type systems, and execution models in a practical, approachable way.

---

## Running Scripts

Before writing more advanced code, the first step is learning how to execute an MShell program.
MShell scripts are plain-text files with the .ms extension that are executed with the msh command-line tool.

Running a script is the most common way to use the language, and every MShell program — no matter how simple or complex — follows this same pattern.

### Creating Your First Script

To create a new script, make a file ending with `.ms`.

```bash
touch hello.ms
```

Inside it, write a simple statement:

```pwsh
Write("Hello from MShell!");
```

This file is a complete program.
MShell executes top-level statements in the order they appear.

### Executing a Script File

To run a script, use the `run` command:

```bash
msh run --file hello.ms
```

Or the short version:

```bash
msh run -f hello.ms
```

MShell will:

1. Load and parse the file
2. Perform type checking
3. Execute the program statement by statement

If the script contains any errors (syntax errors, type mismatches, undefined identifiers, etc.), MShell prints a clear error message and stops.

> ⚠️ An MShell script does not require a `Main()` function, it uses C#-like top level statements.
> The `Main` function is created virtually, and MShell embeds your global scope code inside the virtual `Main` function. ⚠️

---

## Using the REPL

While script files are ideal for building complete programs, the MShell REPL provides an interactive environment where you can experiment with the language in real time.

REPL stands for Read–Eval–Print Loop, and it behaves exactly as the name suggests:

1. Read your input
2. Evaluate it
3. Print the result (when appropriate)
3. Loop back to read more input

### Starting the REPL

To open the interactive MShell environment, run:

```bash
msh repl
```

You’ll see a prompt where you can type any valid MShell statement or expression.

```txt
Welcome to msh REPL (Read, Eval, Print, and Loop!)
Type clear to clean the terminal.
Type exit to exit the quit REPL mode.
> 
```

### How the REPL Evaluates Input

The REPL interprets input line by line, but only executes when the code forms a complete statement or block.

For example:

```csharp
int x = 10;
```

This immediately declares the variable.

But if you start typing a block:

```csharp
> if (x > 5)
> {
```

The REPL will wait for the closing brace before executing:

```csharp
> if (x > 5)
> {
>     Write("Greater");
> }
Greater
```

### Variables and Functions Persist During the Session

Anything you declare — variables, functions, lists — stays available until you exit the REPL.

Example:

```csharp
> int a = 5;
> int b = 7;
>
> int Add(int x, int y)
> {
>      return x + y;
> }
> var z = Add(b, b)
```

### Exiting the REPL

Depending on your implementation, the REPL may accept a built-in command such as:

```bash
exit
```

### Clear the REPL

You can type `clear` at any time to clean your terminal.

```bash
clear
```
---

## Program Structure

Before learning variables, expressions, or control flow, it is essential to understand how an MShell program is organized.
Although the language is simple, it follows clear structural rules that help keep code consistent and predictable.

An MShell program is made of:

- Top-level statements
- Function declarations
- Blocks enclosed in braces
- Statements terminated by semicolons
- A consistent brace style (C#-like)
- Case-sensitive identifiers

### Top-Level Statements

In MShell, you do not need a `Main` function. Execution begins at the first statement written in the file. 
If MShell encounters a function declaration in the middle of your program, it will first store all function declarations, then it will run all statements.

```csharp
Write("Start");
int x = 5;
Write(x);
```

Execution order:

- Print “Start”
- Declare x
- Print 5

This makes MShell ideal for learning interpreters and compiler behavior because nothing is hidden behind abstractions.

### Statements

A statement is any executable instruction. Every simple statement must end with a semicolon.

Valid statements include:

- Variable declarations
- Assignments
- Function calls
- Expressions used as statements
- List indexing assignments

```csharp
int counter = 0;
counter = counter + 1;
Write(counter);
```

These always end with `;`.

### Blocks

A block groups multiple statements between braces. Blocks create a new scope and are commonly used in:

- `if`
- `else`
- `while`
- `for`
- Function bodies

Example:

```csharp
if (x > 0)
{
    Write("Positive");
    x = x - 1;
}
```

Blocks do not end with a semicolon.

### Control-Flow Headers Do NOT End with Semicolons

You must **NEVER** write:

```csharp
if (x > 0);
{
    Write("Wrong");
}
```

The semicolon after the if prematurely ends the statement — this is incorrect.

Correct version:

```csharp
if (x > 0)
{
    Write("Correct");
}
```

### Brace Style

MShell uses a fixed brace style similar to C#:

- Opening brace on the following line
- Closing brace aligned with the start of the construct

Example:

```csharp
while (x < 3)
{
    Write(x);
    x++;
}
```

This is only a convention to keep the code uniform across all projects; MShell will accept any bracing style.

### Comments

MShell supports single-line comments using `//`.

Example:

```csharp
// This is a comment
Write("Hello");
```

### Case Sensitivity

Identifiers in MShell are case-sensitive, and MShell **DOES NOT** accept Pascal Case variables.

```csharp
int value = 1;
int Value = 2; // line 2:4 mismatched input 'Value' expecting ID
```

---

## Identifiers & Naming

Identifiers are the names you give to variables, functions, and other elements in MShell.
They are fundamental to writing readable, well-structured programs.
MShell applies strict naming rules to maintain clarity and consistency across all code.

### Allowed Characters

Identifiers may include:

- Letters (`a–z`, `A–Z`)
- Digits (`0–9`)
- Underscores (`_`)

But they cannot begin with a digit or an underscore.

### Reserved Naming Conventions

MShell adopts strict conventions to keep code uniform and easy to understand:

Variables: camelCase

Examples:

```csharp
int itemCount = 10;
string userName = "Ana";
bool isReady = true;
```

Camel case means:

- First letter lowercase
- Each next word starts with an uppercase letter

Functions: PascalCase

Examples:

```csharp
int Add(int a, int b)
void PrintMessage(string text)
```

PascalCase means:

- Every word starts with an uppercase letter.
- No leading lowercase letters.

---

## Variables

Variables are one of the core building blocks of any programming language.
In MShell, a variable is a named storage location that holds a value of a specific type.
Because MShell is strongly typed, every variable has a type that determines what kind of data it may contain.

### Declaring a Variable

Variables are declared using:

```csharp
type variableName = initialValue;
```

Examples:

```csharp
int count = 10;
double temperature = 18.5;
bool isReady = true;
string message = "Hello";
```

Every declaration must:

- Specify a type
- Use a camelCase variable name
- End with `;`

### The var Keyword (Type Inference)

MShell supports type inference using the `var` keyword.

This means:

- The interpreter deduces the variable’s type from the assigned value
- The type becomes fixed — it behaves just like an explicit type declaration

```csharp
var total = 42;       // inferred int
var name = "Ana";     // inferred string
var pi = 3.14;        // inferred double
var ok = false;       // inferred bool
```

After inference, you cannot assign a value of a different type.

### Reassigning Variables

Variables can be reassigned as long as the new value is of the same type.

Example:

```csharp
int x = 5;
x = 7;       // OK
```

Invalid:

```csharp
int x = 5;
x = "hello";     // Error: type mismatch
```

### Compound Assignment

MShell supports standard compound operators:

- `+=`
- `-=`
- `*=`
- `/=`
- `%=`

Example:

```csharp
int total = 10;
total += 5;       // total = 15
total *= 2;       // total = 30
```

### Increment and Decrement

Both prefix and postfix operators are available:

- `++variable`
- `variable++`
- `--variable`
- `variable--`

Example:

```csharp
int i = 0;
i++;    // 1
++i;    // 2
```

### Assigning to List Elements

You may assign values to elements of a list using index notation.
Type and bounds are strictly checked.

Example:

```csharp
int[] scores = [10, 20, 30];
scores[1] = 25;     // OK
````

Invalid:

```csharp
scores[1] = "hello";    // Error: type mismatch
scores[10] = 5;         // Error: index out of range
```

---

## Types Overview

MShell is a **strongly typed** language.
That means every variable, every expression, and every function parameter must have a well-defined type.

Types determine:

- What operations can you perform
- What values are valid
- How expressions behave
- How lists store their elements
- What functions may return

### Primitive Types in MShell

MShell includes the following primitive types:

|   Type    | Description                                              |
|:---------:|:---------------------------------------------------------|
|   `int`   | whole numbers                                            |
| `double`  | floating-point numbers                                   |
| `decimal` | precise decimal values (usually for money)               |
|  `bool`   | boolean values (`true` or `false`)                       |
| `string`  | text values                                              |
|  `void`   | special type used only for functions that return nothing |

You will learn each of these in its own subsection.

MShell also supports:

- Typed lists of any supported type, such as `int[]`, `double[][]`, `string[]`, etc.
- Type inference using `var`.

#### Integers

The `int` type stores whole numbers

Example:

```csharp
int a = 10;
int b = -3;
int c = 0;
```

Using type inference:

```csharp
var count = 42;    // inferred as int
```

Valid operations include:

- Addition
- Subtraction
- Multiplication
- Division
- Modulus %
- Comparisons
- Logical conditions (after a comparison)

#### Double

The `double` type represents floating-point numbers.

You can use it when you need fractional values or approximate calculations.

Examples:

```csharp
double price = 19.99;
double ratio = 3.1415;
```

With inference:

```csharp
var pi = 3.14;     // inferred as double
```

You can add either the "d" or "D" suffix to declare a double: `var pi = 3.14d;` or `var pi = 3.14D;`

> ⚠️ Remember: floating-point operations may carry rounding precision errors.

#### Decimal

The `decimal` type offers precise arithmetic ideal for:

- Currency
- Financial operations
- Accurate decimal representation

Decimal types require either the "m" or "M" suffix.

```csharp
decimal salary = 1200.50m;
decimal tax = 0.15m;
```

Using inference:

```csharp
var discount = 9.99m;    // inferred as decimal
```

Decimals do not suffer from floating-point precision issues.

#### Boolean

The `bool` type stores only two values:

- `true`
- `false`

Examples:

```csharp
bool isValid = true;
bool isReady = false;
```

With inference:

```csharp
var allowed = true;     // inferred as bool
```

Booleans are essential for conditions in `if`, `while`, and `for` statements.

#### String

Strings represent text enclosed in double quotes.

Examples:

```csharp
string name = "Ana";
string message = "Hello world";
```

Escape sequences are supported:

```csharp
string text = "Line 1\nLine 2";
```

With inference:

```csharp
var greeting = "Hi";
```

Strings support concatenation:

```csharp
string full = "Hello, " + "world";
```

#### String Interpolation

String interpolation lets you embed values directly inside a string using `"{...}"`.

Example:

```csharp
int x = 5;
Write("Value: {x}");
```

Interpolation supports:

- Variables
- Expressions
- Indexers
- Numeric formatting

> To format a number with the desired decimal places, add a `:` followed by the number of desired decimal places:


Example with formatting:

```csharp
double pi = 3.14159;
Write("PI rounded: {pi:2}"); // will round the PI to two decimal places.
```

#### Lists

Lists are typed collections defined using `[]`. Lists can be resized as desired, but they cannot mix different types.

Examples:

```csharp
int[] numbers = [1, 2, 3];
double[] temps = [19.5, 21.0, 18.9];
string[] names = ["Ana", "Bruno"];
````

Multidimensional lists:

```csharp
int[][] matrix = [[1, 2], [3, 4]];
```

Empty lists must be declared with an explicit type:

```csharp
int[] emptyInts;
double[][] emptyGrid;
```

> Lists are heavily used in MShell and are covered in a dedicated section later.

#### Void

The `void` type is a special type used only as the return type of functions that do not return a value.

Example:

```csharp
void Log(string text)
{
    Write(text);
}
```

Variables cannot be of type `void`.

### Type Inference With var

Although explicit typing is encouraged, MShell allows inference:

```csharp
var a = 10;           // int
var b = 3.14;         // double
var c = 3.14d;        // double
var d = "Hello";      // string
var e = true;         // bool
var f = 9.99m;        // decimal
```

The inferred type becomes fixed and cannot be changed later.

Understanding types is essential — they define how expressions behave, when assignments succeed, and when the interpreter raises errors.
With these foundations, the following section introduces Expressions & Operators, where types interact through computations.

---

## Expressions & Operators

Expressions are the core of computation in MShell.
Any time you perform a calculation, compare values, combine strings, or evaluate logic, you are using expressions.

An expression can be:

- A literal (`10`, `"hello"`, `true`)
- A variable (`x`, `name`, `isValid`)
- An operation (`x + 2`)
- A function call (`Add(2, 3`))
- A list indexer (`nums[0]`)
- A complex nested expression

This section introduces operators, precedence rules, and how MShell evaluates expressions.

### Literals

A literal is a raw value written directly in the code.

Examples:

```csharp
5
3.14
9.99m
true
"Hello"
[1, 2, 3]
```

Each literal has a specific type.

### Variables as Expressions

Any variable name is also a valid expression.

```csharp
int x = 5;
Write(x);    // x evaluates to 5
```

### Arithmetic Operators

MShell supports the standard arithmetic operators:

- `+` (addition)
- `-` (subtraction)
- `*` (multiplication)
- `/` (division)
- `%` (modulus)
- `^` (exponentiation — right-associative)

```csharp
int a = 2 + 3;      // 5
int b = 7 - 4;      // 3
int c = 2 * 6;      // 12
int d = 8 / 2;      // 4
int e = 7 % 3;      // 1
int f = 2 ^ 3;      // 8
```

### Comparisons

Comparison operators always return a bool:

- `==`
- `!=`
- `<`
- `<=`
- `>`
- `>=`

Examples:

```csharp
bool check1 = 5 > 3;     // true
bool check2 = 10 == 4;   // false
bool check3 = 3 <= 3;    // true
```

These are commonly used in conditional (`if`, `while`, etc.) and loop constructs.

### Logical Operators

Logical operators combine booleans:

- `&&` — logical **AND**
- `||` — logical **OR**
- `!` — logical **NOT**

Examples:

```csharp
bool ok = true && false;     // false
bool ready = !false;         // true
bool valid = (x > 0) || (y < 10);
```

Operands must be of type `bool` or return a `bool` type.

### String Concatenation

Strings can be concatenated using the `+` operator:

```csharp
string full = "Hello, " + "world";
```

Or using the [String Interpolation Feature](#string-interpolation)

```csharp
string hello = "Hello,";
string world = "world";

string full = "{hello} {world}";
```

This operation is only defined for string operands.

### Numeric Type Promotion

MShell follows a clear rule:

When mixing numeric types:

- `int` + `double` → `double`
- `int` + `decimal` → `decimal`
- `double` + `decimal` → depends on interpreter implementation, but usually `decimal` prevails.

Example:

```csharp
var result = 2 + 3.5;     // inferred double
```

### Parentheses

Parentheses force evaluation order:

```csharp
int a = 2 + 3 * 4;     // 14
int b = (2 + 3) * 4;   // 20
```

Use parentheses for clarity and to ensure your intention is explicit.

### Operator Precedence

From highest to lowest priority:

- `()`
- Unary `-`, `!`
- `^` (exponentiation, right-associative)
- `*`, `/`
- `%`
- `+`, `-`
- Comparisons (`==`, `!=`, `<`, `<=`, `>`, `>=`)
- `&&`
- `||`
- Ternary `?:`

Understanding precedence helps avoid surprising results.

### Ternary Operator

The ternary operator chooses between two values:

```
condition ? valueIfTrue : valueIfFalse
```

Example:

```csharp
int x = 5;
string msg = (x > 3) ? "Greater" : "Not greater";
```

### Functions as Expressions

Function calls produce values if the function has a return type.

```csharp
int result = Add(2, 3);     // Add returns an int
```

If the function returns void, it cannot be used inside an expression.

### List Indexers in Expressions

Accessing list elements yields values usable in expressions:

```csharp
int[] nums = [10, 20, 30];
int x = nums[1] + 5;     // x = 25
```

Multidimensional access:

```csharp
double[][] grid = [[1.0, 2.0], [3.0, 4.0]];
double v = grid[1][0];   // 3.0
```

### Expression Evaluation Order

MShell evaluates expressions left-to-right, respecting precedence. This deterministic rule ensures predictable behavior.

---
## Lists

Lists In Depth

Lists are one of the most expressive and flexible data structures in MShell.
Unlike arrays in many languages, MShell lists are:

- Strongly typed
- Resizable
- Safe (bounds-checked)
- Capable of being nested (multidimensional lists)

Lists allow you to store collections of values and manipulate them using built-in methods.

### Declaring Lists

Lists are declared by appending [] to a type.

Examples:

```csharp
int[] numbers = [1, 2, 3];
double[] temps = [19.5, 21.0, 18.9];
string[] names = ["Ana", "Bruno"];
````

MShell requires that list literals be non-empty to infer the element type.

If you need an empty list, you must declare it with an explicit type:

```
int[] scores = [];
double[][] matrix;
```

These start as empty lists of the declared type.

### Multidimensional Lists

Lists can be nested as deeply as you need:

```csharp
int[][] grid = [[1, 2], [3, 4]];
double[][][] cube = [[[1.1, 1.2], [2.1, 2.2]]];
```

Each dimension must remain consistent:

- `grid` is a list of lists of int
- `cube` is a list of lists of lists of double

> ⚠️ Mixing types will cause errors.

### Indexers

Lists are accessed using zero-based indexes:

```csharp
int[] nums = [10, 20, 30];
Write(nums[1]);     // prints 20
```

Multidimensional indexing:

```csharp
double[][] matrix = [[1.0, 2.0], [3.0, 4.0]];
Write(matrix[1][0]);   // prints 3.0
```

> ⚠️ All indexing operations are bounds-checked. Invalid indexes cause an immediate runtime error.

### Assigning to List Elements

You may update elements using index notation:

```csharp
int[] nums = [1, 2, 3];
nums[0] = 10;
```

Assignments verify:

- The index is valid
- The assigned value matches the list’s element type

### List Methods

MShell includes several built-in list methods, each with its own rules and validation.

Every method uses **PascalCase**in accordance with the function-naming standard.

Below, each method is documented individually.

#### Add

Adds a new element to the end of the list.

Syntax:

```csharp
list.Add(value)
```

Example:

```csharp
int[] nums;
nums.Add(10);
nums.Add(20);
Write(nums);     // [10, 20]
```

Type rules:
- The value must match the list’s element type.

#### RemoveAt

Removes an element at a specific index.

Syntax:

```csharp
list.RemoveAt(index)
```

Example:

```csharp
int[] nums = [10, 20, 30];
nums.RemoveAt(1);
Write(nums);    // [10, 30]
```

Rules:

- Index must be valid
- Causes elements to shift left

#### Insert

Inserts an element at a specific index.

Syntax:

```csharp
list.Insert(index, value)
```

Example:

```csharp
int[] nums = [10, 20, 30];
nums.Insert(1, 15);
Write(nums);    // [10, 15, 20, 30]
```

Rules:

- Index must be valid (0 to Size())
- Element type must match

#### Clear

Removes all elements from the list.

Syntax:

```csharp
list.Clear()
```

Example:

```csharp
int[] nums = [10, 20, 30];
nums.Clear();
Write(nums);     // []
```

The list remains the same type — only elements are removed.

#### Size

Returns the number of elements in the list.

Syntax:

```csharp
int length = list.Size();
```

Example:

```csharp
int[] nums = [10, 20, 30];
Write(nums.Size());     // 3
```

Useful in loops:

```csharp
for (int i = 0; i < nums.Size(); i++)
{
    Write(nums[i]);
}
```

### Lists in Expressions

List indexers can appear inside expressions, including interpolation:

```csharp
int[] scores = [12, 34, 56];
Write($"First score: {scores[0]}");
```

---

## Control Flow

Control flow determines how your program decides what to do and how many times to do it.
MShell provides classic control-flow constructs that behave predictably and consistently, helping you express conditions, loops, and decision-making.

This section covers:

- `if`, `else if`, `else`.
- `while`
- The MShell version of `do...while` (guarded variant).
- `for` loops.

All control-flow structures require:

- Conditions of type `bool`.
- Proper use of braces `{}` - _when more than one line to evaluate_.
- No semicolon after the header line.

### if Statement

The `if` statement executes a block when the condition is evaluated to `true`.

Example:

```csharp
int x = 5;

if (x > 3)
{
    Write("Greater than 3");
}

// One-liners
if (x > 3)
    Write("Greater than 3");
// OR
if (x > 3) Write("Greater than 3");
```

The condition must evaluate to `bool`.

Invalid:

```csharp
if (10)          // Error: not a bool

int x = 5;
if (x > 3)
    Write("Greater than 3");
    Write("Greater than 3"); // Needs to be wrapped inside braces
```

### else if and else

Use `else if` for additional conditions, and `else` for the fallback case.

Example:

```csharp
int x = 10;

if (x > 10)
{
    Write("Greater");
}
else if (x == 10)
{
    Write("Equal");
}
else
{
    Write("Less");
}
```

Rules:

- Only one `else` is allowed.
- You may have multiple `else if` blocks.
- Conditions must return type `bool`.

### while Loop

Repeats a block as long as the condition is `true`.

Example:

```csharp
int i = 0;

while (i < 3)
{
    Write(i);
    i++;
}
```

If the condition is initially `false`, the body never executes.

### Do/While Loop

In MShell, the `do...while` loop follows the traditional behavior found in most C-style languages:

- The block always executes at least once.
- After executing the block, the condition is evaluated.
- If the condition is true, the loop repeats.
- If the condition is false, the loop stops.

This structure is useful when the body must run before the condition is checked.

Example:

```csharp
int x = 0;

do
{
    Write("This will always run once");
    x++;
}
while (x < 3);
```

Execution flow:

- Run the block.
- Check condition.
- Repeat if `true`.

Even when the condition is `false` initially, the block still executes one time:

```csharp
do
{
    Write("Always runs once");
}
while (false);
```

This makes `do...while` ideal for:

- Menus
- Input reading
- Situations where an operation must occur before validation

### For Loop

The `for` loop has the familiar C-style structure:

```csharp
for (init; condition; iteration)
{
    ...
}
```

Example:

```csharp
for (int j = 0; j < 3; j++)
{
    Write(j);
}
```


Key rules:

- The init section can declare a variable.
- The loop variable is scoped to the loop.
- If the condition is omitted, the loop does not execute.
- The condition must be a `bool`.

Examples:

```csharp
for (int i = 0; i < nums.Size(); i++)
{
    Write(nums[i]);
}
```

Loop with missing condition (special MShell rule):

```csharp
for (int i = 0; ; i++)
{
    // This loop DOES NOT execute at all in MShell
}
```

### Using break or continue

If implemented in your MShell version, typical behavior applies:

- `break` exits the loop
- `continue` skips to the next iteration

---

## Functions

Functions allow you to group logic into reusable, named blocks of code.
They are essential for structuring programs, avoiding repetition, organizing algorithms, and improving readability.

In MShell, functions are:

- Strongly typed.
- Named using PascalCase.
- Declared with explicit parameter types.
- Allowed to return values using `return`.
- Capable of recursion.
- Scoped: variables inside belong only to the function.

This section teaches how to declare, call, and use functions effectively.

### Function Declaration Syntax

The structure of a function is:

```csharp
returnType FunctionName(type param1, type param2)
{
    // body
}
```

Example:

```csharp
int Add(int left, int right)
{
    return left + right;
}
```

Rules:

- The name must follow PascalCase.
- Parameters must be typed.
- The return type can be any valid type (including lists)
- The return type may be void if no value is returned

### Calling Functions

To call a function, use its name followed by arguments:

```csharp
int result = Add(2, 3);
Write(result);         // 5
```

Arguments must match:

- The number of parameters
- The types of parameters
- The order of parameters

Otherwise, a runtime error occurs.

### Void Functions

Functions that do not return a value use void:

```csharp
void PrintGreeting(string name)
{
    Write($"Hello, {name}!");
}
```

They can still be called normally:

```csharp
PrintGreeting("Ana");
```

A void function may use a `return;` statement to exit early:

```csharp
void Check(int value)
{
    if (value < 0)
    {
        return;
    }

    Write("Valid");
}
```

### Functions With Return Values

Functions that specify a return type must return a value:

```csharp
double Discount(double price)
{
    return price * 0.1;
}
```

Failing to return a value on every path is an error.

Invalid example:

```csharp
int Bad(int x)
{
    if (x > 0)
    {
        return x;
    }
    // Missing return here → error
}
```

### Parameters

Parameters are:

- Local to the function
- Read-only unless reassigned to new variables
- Typed explicitly

Example:

```csharp
int Multiply(int a, int b)
{
    return a * b;
}
````

Parameters act exactly like variables inside the function.

### Local Variables and Scope

Variables declared inside a function exist only inside that function.

```csharp
int AddOne(int x)
{
    int temp = x + 1;
    return temp;
}

Write(temp); // Error: temp is not visible here.
```

This isolation helps prevent unintended side effects.

#### Overwriting Global Variables

> Functions may read global variables.
>
> Functions may modify global variables.

### Recursion

MShell fully supports recursive calls.

```csharp
int CustomFactorial(int n)
{
    if (n <= 1)
    {
        return 1;
    }

    return n * CustomFactorial(n - 1);
}
```

Calling:

```csharp
Write(CustomFactorial(5));    // 120
```

Recursion is proper for algorithms such as:

- Factorials - Although MShell has a built-in `Factorial` function.
- Fibonacci sequences.
- Tree traversal.
- Divide-and-conquer strategies.

### Functions as Expressions

Non-void functions can be used wherever a value is expected:

```csharp
int[] values = [1, 2, 3];
int doubled = Add(values[1], values[1]);   // Add returns int
```

Functions that return `void` cannot appear inside expressions.

---

## Input & Output

Input and output are essential for interacting with users, displaying results, debugging programs, and building dynamic behavior.
MShell provides two simple but powerful built-in functions for this purpose:

- `Write`: Outputs values to the console.
- `Read`: Reads a line of input from the user.

These functions make it easy to display information and capture user-provided data in MShell scripts and REPL sessions.

### Write

The `Write` function prints one or more arguments to the console and adds a newline at the end.

```csharp
Write(value1, value2, ...);
```

You can pass any number of arguments:

```csharp
Write("Hello");
Write("Result is: ", 42);
Write("Coordinates: ", x, ", ", y);
```

#### How Write Works

- All arguments are converted to their string representation.
- They are concatenated in order.
- A newline (`\n`) is appended automatically.

Example:

```csharp
int a = 5;
int b = 7;

Write("Sum = ", a + b);
```

Output: `Sum = 12`

### Read

The `Read` function pauses execution and waits for the user to enter text, returning the parsed value.

```csharp
var input = Read();
```

#### How Read Parses Input

MShell attempts to interpret the input in this order:

- `decimal`.
- `double`.
- `int`.
- `bool` (`true` or `false`).
- List literal (like `[1, 2, 3]`).
- Otherwise → returns a `string`.

Examples:

```csharp
Write("Enter a number: ");
var n = Read();
Write("You typed: ", n);
```

- If the user types: `42`, then `n` becomes an `int`.
- If the user types: `3.14`, then `n` becomes a `double`.
- If they type: `[1, 2, 3]`, then `n` becomes a `list`.
- If they type: `hello`, then `n` becomes a `string`.

### Handling Input Safely

Because `Read` tries multiple parses, be aware of ambiguous cases.

For example:

```csharp
var x = Read();
```

- If the user types: `true`, `True`, `TRUE` or `tRUE`, then `x` is a `bool` with a `true` value.
- If the user types: `false`, `False`, `FALSE` or `fALSE`, then `x` is a `bool` with a `false` value.

#### Example

Reading Numbers Until `0`

```csharp
int sum = 0;
int n;

do
{
    Write("Enter a number (0 to stop): ");
    n = Read();
    sum += n;
}
while (n != 0);

Write("Final sum: ", sum);
```

---

## Math Library

MShell includes a compact yet powerful math library containing commonly used mathematical functions.
These functions are designed to:

- Work with numeric types (`int`, `double`, `decimal`), and `list` of numeric types.
- The `Add` function supports `string` types.
- Validate argument count and types.
- Provide predictable, well-defined behavior.

In MShell, all math functions use PascalCase, just like user-defined functions.

### Sin

Returns the sine of a given angle (in radians).

Syntax:

```csharp
Sin(value)
```

Example:

```csharp
Write(Sin(3.14));
```

### Cos

Returns the cosine of an angle (in radians).

Syntax:

```csharp
Cos(value)
```

Example:

```csharp
Write(Cos(0));
```

### Abs

Returns the absolute value of a number.

Syntax:

```csharp
Abs(value)
```

Example:

```csharp
Write(Abs(-5));     // 5
```

### Sqrt

Returns the square root of a number.

Syntax:

```csharp
Sqrt(value)
```

Example:

```csharp
Write(Sqrt(16));    // 4
```

### Max

Returns the greater of two numbers.

Syntax:

```csharp
Max(a, b)
```

Example:

```csharp
Write(Max(3, 7));   // 7
```

### Min

Returns the smaller of two numbers.

Syntax:

```csharp
Min(a, b)
```

Example:

```csharp
Write(Min(3, 7));   // 3
```

### Pow

Raises a base to the given exponent.

Syntax:

```csharp
Pow(base, exponent)
```

Example:

```csharp
Write(Pow(2, 5));   // 32
```

### Add

Returns the sum of two numbers.

Syntax:

```csharp
Add(a, b)
```

Example:

```csharp
Write(Add(10, 5));   // 15
```

### Subtract

Subtracts the second value from the first.

Syntax:

```csharp
Subtract(a, b)
```

Example:

```csharp
Write(Subtract(10, 3));   // 7
```

### Multiply

Multiplies two values.

Syntax:

```csharp
Multiply(a, b)
```

Example:

```csharp
Write(Multiply(6, 7));   // 42
```

### Divide

Divides the first value by the second.

Syntax:

```csharp
Divide(a, b)
```

Example:

```csharp
Write(Divide(10, 2));   // 5
```

> ⚠️ Division by zero triggers an error.

### Sum

Computes the total of all elements in a numeric sequence.

Syntax:

```csharp
Sum(sequence)
```

Example:

```csharp
Write(Sum([1, 2, 3]));    // 6
OR
Write(Sum(1, 2, 3));      // 6
OR
WRite(Sum([1, 2], 3));    // 6
```

### Factorial

Computes the factorial of an integer (n!).

Syntax:

```csharp
Factorial(n)
```

Example:

```csharp
Write(Factorial(5));   // 120
```

---

## Scoping Rules

Scoping defines where variables and functions exist, where they can be accessed, and when they cease to exist.
MShell uses a simple, predictable model of lexical scoping (also known as block scoping), similar to many modern languages.

A firm understanding of scope is essential for writing reliable programs and avoiding subtle bugs.

### What Is Scope?

Scope determines:

- Where an identifier (variable or function) is visible.
- Where it can be read or assigned.
- When it is created and destroyed.

MShell has three main kinds of scope:

- Global scope
- Function scope
- Block scope (`{ ... }`)

Each is explained below.

### Global Scope

Everything declared at the top level of a script is in global scope.

Example:

```csharp
int x = 10;

Write(x);     // OK: x is global
```

> ⚠️ Global variables are visible everywhere except inside functions that redeclare a variable of the same name.
>
> If a local variable has the same name as a global variable, the local variable takes precedence.


### Function Scope

Variables declared inside a function exist only inside that function.

```csharp
int AddOne(int value)
{
    int temp = value + 1;
    return temp;
}

Write(temp);   // Error: not visible outside the function.
```

Key rules:

- Function-local variables cannot be accessed externally.
- Parameters behave like local variables.
- Returning a value does not make the variable accessible globally.

> ⚠️ Each call creates its own set of parameters and locals.

### Block Scope

Any `{ ... }` block creates a new scope.

Blocks appear in:

- `if`
- `else`
- `while`
- `do...while`
- `for`
- Functions
- Arbitrary code blocks

Example:

```csharp
int x = 1;

if (true)
{
    int y = 2;
    Write(x + y);   // OK
}

Write(y);          // Error: y only exists inside the block.
```

Even nested blocks behave predictably:

```csharp
{
    int a = 10;
    {
        int b = 20;
        Write(a + b);   // OK
    }
    Write(b);           // Error
}
```

### Loop Scope (for Variables)

The loop variable in a for loop belongs to the loop’s block:

```csharp
for (int i = 0; i < 3; i++)
{
    Write(i);
}

Write(i);     // Error: i is not defined here.
```

This behavior prevents accidental reuse of loop indices outside the loop.

### Scope and return

A `return` statement may only be used inside a function body.

> ⚠️ The global scope accepts a `return` statement because it implements C#-like top-level statements by wrapping the code in the global scope within a virtual `Main` function.

### No Hoisting

MShell does not perform JavaScript-style “hoisting.”

This means variables must be declared before use:

```csharp
Write(x);   // Error: x not declared yet
int x = 10;
```

This rule ensures predictable execution.

### Lifetime of Variables

- Global variables live for the entire execution of the script.
- Function variables live until the function returns.
- Block variables live until exiting the block.

> ⚠️ There is no garbage collection to manage — once out of scope, a variable simply ceases to exist in the execution environment.

### Summary of Scoping Behavior

| Construct        | Scope Created? | Access Outside Block? |
| ---------------- | -------------- | --------------------- |
| Script top level | Global scope   | Yes                   |
| `{ ... }` block  | Yes            | No                    |
| `if / else`      | Yes            | No                    |
| `while`          | Yes            | No                    |
| `do...while`     | Yes            | No                    |
| `for`            | Yes (loop var) | No                    |
| Function body    | Yes            | No                    |

---

## Errors & Limitations

MShell is intentionally small, strict, and predictable.
To achieve this, the language enforces several rules at both parse time and runtime. When these rules are violated, MShell produces clear error messages and stops execution immediately.

Understanding these errors helps you write safer programs and debug issues quickly.

Below are the most common situations where errors occur, along with explanations of why they happen.

### Type Mismatch Errors

MShell does not perform implicit conversions, except for numeric types.

Examples of invalid operations:

```csharp
int x = 10;
x = "hello";            // Error: cannot assign string to int

int[] nums = [1, 2];
nums.Add("bad");        // Error: list expects int elements

bool x = 2;             // Error: must assign a bool
```

Whenever a value’s type does not match the expected type, MShell raises a type mismatch error.

### Invalid Index Errors

All list indexing is bounds-checked.

Examples:

```csharp
int[] values = [1, 2, 3];
Write(values[10]);      // Error: index out of range

double[][] grid = [[1.0], [2.0]];
grid[1][5];             // Error: nested index out of range
```

This prevents accidental memory access and ensures predictable behavior.

### Unsupported Operations

Some operations are not allowed.

Examples:

```csharp
"Hello" - "World";      // Error: unsupported operator on strings

[1, 2, 3] + 5;          // Error: only scalar + scalar is supported
```

> ⚠️ If an operator is not defined for a combination of types, the interpreter will report it.

### Invalid Conditions in Control Flow

Conditions in `if`, `while`, `do...while`, and `for` must be booleans.

Invalid:

```csharp
if (10)                 // Error: condition must be a bool.

while ("text")          // Error

for (int i = 0; i < "wrong"; i++)    // Error
```

This ensures all control-flow logic remains readable and explicit.

### Return Misuse

Functions with non-void return types must return a value:

```csharp
int Test()
{
    // Error: missing return
}
```

### Duplicate Declarations

Variables cannot be redeclared in the same scope.

```csharp
int x = 5;
int x = 10;        // Error: redeclared variable
```

Depending on your MShell version, shadowing in nested blocks may or may not be allowed.

### Empty List Literal Inference Error

MShell cannot infer a type from an empty list literal:

```csharp
var x = [];        // Error: cannot infer type of empty list
```

Instead, declare an explicitly typed empty list:

```csharp
int[] values;
```

### Argument Count Mismatch

Calling a function or list method with the wrong number of arguments is an error.

```csharp
Add(1);            // Error: expected 2 arguments
nums.Insert(5);    // Error: expected index and value
```

### Numeric Overflow (64-bit Integer)

MShell uses a 64-bit signed integer as its default `int` type.

This means the valid range for `int`, `double`, and `decimal` values is:

| MShell type/keyword |                   Min                    |                   Max                   |
|:--------------------|:----------------------------------------:|:---------------------------------------:|
| `int`               | $-9{,}223{,}372{,}036{,}854{,}775{,}808$ | $9{,}223{,}372{,}036{,}854{,}775{,}807$ |
| `double`            |  $-1.7976931348623157 \times 10^{308}$   |  $1.7976931348623157 \times 10^{308}$   |
| `decimal`           |         $-7.9228 \times 10^{28}$         |         $7.9228 \times 10^{28}$         |


If a calculation produces a value outside this range, an overflow error may occur depending on your MShell implementation.

Examples:

```csharp
int max = 9223372036854775807;
int result = max + 1;      // Overflows.

int min = -9223372036854775808;
int test = min - 1;        // Overflows.
```

---

## Full Example

This section presents a complete MShell program that showcases the main features of the language working together:

- Variables
- Lists
- Expressions
- Loops
- Conditions
- Functions
- Math operations
- String interpolation
- Input and output

The goal is to demonstrate how a full script flows from start to finish using clean, idiomatic MShell.

### Scenario

In this example, we will:

- Ask the user to enter a series of numbers.
- Square each number.
- Filter out numbers less than or equal to 10.
- Compute the sum of the remaining values.
- Print all intermediate results and the final total.

This exercise covers nearly all the central concepts introduced in the guide.

### Script

```csharp
Write("How many numbers will you enter? ");
int count = Read();

int[] values;

for (int i = 0; i < count; i++)
{
    Write($"Enter value #{i + 1}: ");
    var n = Read();
    values.Add(n);
}

Write("Original list: ", values);

// Square each value
for (int i = 0; i < values.Size(); i++)
{
    values[i] = values[i] ^ 2;
}

Write("Squared values: ", values);

// Function to sum only values greater than 10
int SumGreaterThanTen(int[] items)
{
    int total = 0;

    for (int j = 0; j < items.Size(); j++)
    {
        if (items[j] > 10)
        {
            total += items[j];
        }
    }

    return total;
}

int result = SumGreaterThanTen(values);

Write("Sum of squared values > 10: ", result);
```