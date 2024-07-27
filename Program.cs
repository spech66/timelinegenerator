using Spectre.Console.Cli;
using TimelineGenerator.Commands;

var app = new CommandApp();
app.Configure(config =>
{
    config.AddCommand<GenerateCommand>("generate").WithDescription("Generate a timeline using an exporter.");
    config.AddCommand<SampleCommand>("sample").WithDescription("Generate a sample file.");
});
return app.Run(args);
