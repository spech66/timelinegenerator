using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimelineGenerator.Exporter;

namespace TimelineGenerator.Commands
{
    internal class GenerateCommand : Command<GenerateCommand.Settings>
    {
        public sealed class Settings : CommandSettings
        {
            [Description("Timeline file to read. Defaults to timeline.yml")]
            [CommandArgument(0, "[inputPath]")]
            [DefaultValue("timeline.yml")]
            public string InputFile { get; init; }

            [Description("Path to generate timeline. Defaults to timeline. File format depends on exporter.")]
            [CommandArgument(1, "[outputPath]")]
            [DefaultValue("timeline")]
            public string OutputPath { get; init; }

            [CommandOption("-e|--exporter")]
            [DefaultValue("timelinejs")]
            [Description("Exporter to use. timelinejs, visjs, bootstrap")]
            public string Exporter { get; set; }
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            var inputFile = settings.InputFile ?? Path.Combine(Directory.GetCurrentDirectory(), "timeline.yml");
            var outputPath = settings.OutputPath ?? Path.Combine(Directory.GetCurrentDirectory(), "timeline");
            var exporter = settings.Exporter ?? "timelinejs";

            var timeline = YamlImporter.Import(inputFile);
            AnsiConsole.MarkupLine($"Input file: [bold]{inputFile}[/]");
            AnsiConsole.MarkupLine($"Output path: [bold]{outputPath}[/]");
            AnsiConsole.MarkupLine($"[bold]Version:[/] {timeline.Version}");
            AnsiConsole.MarkupLine($"[bold]Events:[/] {timeline.Events.Count}");

            var exportEngine = ExporterFactory.Create(exporter);
            exportEngine.Export(timeline, outputPath);

            return 0;
        }
    }
}
