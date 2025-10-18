using DotNetOpen;
using Spectre.Console.Cli;

var app = new CommandApp<OpenSolutionCommand>();
return app.Run(args);
