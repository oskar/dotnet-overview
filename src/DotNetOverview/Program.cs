using DotNetOverview;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

var services = new ServiceCollection()
    .AddSingleton(AnsiConsole.Console)
    .BuildServiceProvider();

var app = new CommandLineApplication<OverviewCommand>();
app.Conventions.UseDefaultConventions();
app.Conventions.UseConstructorInjection(services);

return await app.ExecuteAsync(args);
