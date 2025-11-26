using Msh.CommandLineInterface.Terminals;
using Msh.Interpreter.Abstractions;

using Spectre.Console;

namespace Msh.CommandLineInterface.Extensions;

internal static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        internal IServiceCollection AddAnsiConsole()
        {
            return services.AddSingleton<IAnsiConsole>(_ => AnsiConsole.Console);
        }

        internal IServiceCollection AddConsoleTerminal()
        {
            services.AddSingleton<ITerminal, ConsoleTerminal>(provider => new ConsoleTerminal(provider.GetRequiredService<IAnsiConsole>()));

            services.AddSingleton<ConsoleTerminal>(provider => new ConsoleTerminal(provider.GetRequiredService<IAnsiConsole>()));

            return services;
        }
    }
}