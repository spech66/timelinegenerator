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
            [Description("Path to input file. Defaults to timeline.yml")]
            [CommandArgument(0, "[inputPath]")]
            [DefaultValue("timeline.yml")]
            public string InputPath { get; init; }

            [Description("Path to write timeline file. Defaults to timeline. File format depends on exporter.")]
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
            var inputPath = settings.InputPath ?? Path.Combine(Directory.GetCurrentDirectory(), "timeline.yml");
            var outputPath = settings.OutputPath ?? Path.Combine(Directory.GetCurrentDirectory(), "timeline");
            var exporter = settings.Exporter ?? "timelinejs";

            var timeline = YamlImporter.Import(inputPath);
            Console.WriteLine(timeline.Version);
            Console.WriteLine(timeline.Events.Count);

            var exportEngine = ExporterFactory.Create(exporter);
            exportEngine.Export(timeline, outputPath);

            return 0;
        }
    }
}
