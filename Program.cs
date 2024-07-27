using Spectre.Console.Cli;
using TimelineGenerator.Commands;

var app = new CommandApp();
app.Configure(config =>
{
    config.AddCommand<SampleCommand>("sample");
});
return app.Run(args);

/*
Console.WriteLine("Timeline Generator CLI");

// Parse test.yml
var timeline = YamlImporter.Import("test.yml");
Console.WriteLine(timeline.Version);
Console.WriteLine(timeline.Events.Count);
*/
