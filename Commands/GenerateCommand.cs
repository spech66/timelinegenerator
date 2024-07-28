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
            [Description("Exporter to use. timelinejs, visjs, mermaid, bootstrap")]
            public string Exporter { get; set; }

            [CommandOption("--start <DATE>")]
            [Description("Filter events starting on or after this date.")]
            public DateTime? StartDate { get; set; }

            [CommandOption("--end <DATE>")]
            [Description("Filter events ending on or before this date.")]
            public DateTime? EndDate { get; set; }

            [CommandOption("--tag <VALUES>")]
            [Description("Filter events with these tags. Can be specified multiple times.")]
            public string[] IncludedTags { get; set; }

            [CommandOption("--exclude-tag <VALUES>")]
            [Description("Exclude events with these tags. Can be specified multiple times.")]
            public string[] ExcludedTags { get; set; }

            [CommandOption("--category <VALUES>")]
            [Description("Filter events with these categories. Can be specified multiple times.")]
            public string[] IncludedCategories { get; set; }

            [CommandOption("--exclude-category <VALUES>")]
            [Description("Exclude events with these categories. Can be specified multiple times.")]
            public string[] ExcludedCategories { get; set; }
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
            AnsiConsole.MarkupLine($"[bold]Total Events:[/] {timeline.Events.Count}");

            FilterTimeline(settings, timeline);
            AnsiConsole.MarkupLine($"[bold]Filtered Events:[/] {timeline.Events.Count}");

            var exportEngine = ExporterFactory.Create(exporter);
            var filePath = exportEngine.Export(timeline, outputPath);

            AnsiConsole.MarkupLine($"[green]Success:[/] Timeline generated at [bold]{filePath}[/].");
            return 0;
        }

        private void FilterTimeline(Settings settings, YamlTimeline timeline)
        {
            var filter = timeline.Events.AsQueryable();

            if (settings.StartDate.HasValue)
            {
                filter = filter.Where(e => e.Start >= settings.StartDate);
            }
            if (settings.EndDate.HasValue)
            {
                filter = filter.Where(e => e.End <= settings.EndDate);
            }

            if (settings.IncludedTags?.Length > 0)
            {
                filter = filter.Where(e => e.Tags.Any(t => settings.IncludedTags.Contains(t)));
            }
            if (settings.ExcludedTags?.Length > 0)
            {
                filter = filter.Where(e => e.Tags.All(t => !settings.ExcludedTags.Contains(t)));
            }

            if (settings.IncludedCategories?.Length > 0)
            {
                filter = filter.Where(e => settings.IncludedCategories.Contains(e.Category));
            }
            if (settings.ExcludedCategories?.Length > 0)
            {
                filter = filter.Where(e => !settings.ExcludedCategories.Contains(e.Category));
            }

            timeline.Events = filter.ToList();
        }
    }
}
