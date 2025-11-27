Write("Number Insights");
Write("Enter integers (0 to finish). Negative numbers are allowed.");

int SumAll(int[] values)
{
    int total = 0;

    for (int i = 0; i < values.Size(); i++)
    {
        total += values[i];
    }

    return total;
}

double Average(int[] values)
{
    if (values.Size() == 0)
    {
        return 0;
    }

    var total = SumAll(values);
    var count = values.Size();

    return (total + 0.0) / count;
}

string DescribeParity(int value)
{
    return value % 2 == 0 ? "even" : "odd";
}

int[] numbers;
int input;

do
{
    Write("Number: ");
    input = Read();

    if (input != 0)
    {
        numbers.Add(input);
    }
}
while (input != 0);

if (numbers.Size() == 0)
{
    Write("No numbers provided.");
    return;
}

var size = numbers.Size();

Write("Captured {size} number(s).");

var smallest = numbers[0];
var largest = numbers[0];

for (int i = 1; i < numbers.Size(); i++)
{
    if (numbers[i] < smallest)
    {
        smallest = numbers[i];
    }

    if (numbers[i] > largest)
    {
        largest = numbers[i];
    }
}

var totalSum = SumAll(numbers);
var average = Average(numbers);

Write("Total: {totalSum}");
Write("Average: {average:2}");
Write("Min: {smallest}");
Write("Max: {largest}");

Write("Listing values with parity:");

for (int i = 0; i < numbers.Size(); i++)
{
    var parity = DescribeParity(numbers[i]);
    Write("#{i}: ", numbers[i], " is ", parity);
}

var firstValue = numbers[0];

if (firstValue >= 0)
{
    var factorial = Factorial(firstValue);
    Write("Factorial of first value: {factorial}");
}
else
{
    Write("Factorial skipped for negative first value.");
}

numbers.Insert(0, numbers[0]);
Write("Duplicated first value at the start to demo Insert.");

if (numbers.Size() > 3)
{
    var lastIndex = numbers.Size() - 1;
    numbers.RemoveAt(lastIndex);
    Write("Removed last value to demo RemoveAt.");
}

Write("Clearing list...");
numbers.Clear();
size = numbers.Size();
Write("List size after clear: {size}");
