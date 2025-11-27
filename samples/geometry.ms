Write("Welcome to the Geometry Helper!");

bool running = true;
double pi = 3.14159d;

do
{
    Write("---- Geometry Menu ----");
    Write("1) Circle area and perimeter");
    Write("2) Right triangle: missing side (Cos)");
    Write("3) Sine wave sample table");
    Write("4) Exit");
    Write("Select an option (1-4): ");

    int option = Read();

    if (option == 1)
    {
        Write("Enter radius: ");
        var radius = Read();

        var area = pi * radius * radius;
        var perimeter = 2 * pi * radius;

        Write("Area: {area:2}");
        Write("Perimeter: {perimeter:2}");
    }
    else if (option == 2)
    {
        Write("Given hypotenuse and angle (degrees), calculate adjacent side.");
        Write("Enter hypotenuse: ");
        var hypotenuse = Read();

        Write("Enter angle in degrees: ");
        var angleDeg = Read();

        var angleRad = angleDeg * pi / 180;
        var adjacent = hypotenuse * Cos(angleRad);

        Write("Adjacent side: {adjacent:2}");
    }
    else if (option == 3)
    {
        Write("Enter amplitude: ");
        var amplitude = Read();

        Write("Enter step (degrees): ");
        var step = Read();

        Write("Sine samples:");

        for (int deg = 0; deg <= 360; deg += step)
        {
            var rad = deg * pi / 180;
            var value = amplitude * Sin(rad);
            Write("deg {deg} => {value:2}");
        }
    }
    else if (option == 4)
    {
        running = false;
        Write("Goodbye!");
    }
    else
    {
        Write("Invalid option.");
    }
}
while (running);
