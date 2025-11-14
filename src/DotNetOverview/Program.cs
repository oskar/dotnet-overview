using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace DotNetOverview;

public class Program
{
    private static Task<int> Main(string[] args)
    {
        var services = new ServiceCollection()
            .AddSingleton(AnsiConsole.Console)
            .BuildServiceProvider();

        var app = new CommandLineApplication<OverviewCommand>();
        app.Conventions.UseDefaultConventions();
        app.Conventions.UseConstructorInjection(services);

        return app.ExecuteAsync(args);
    }
}
