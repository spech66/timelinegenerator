using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace TimelineGenerator.Commands
{
    internal class SampleCommand : Command<SampleCommand.Settings>
    {
        public sealed class Settings : CommandSettings
        {
            [Description("File to generate sample in. Defaults to sample.yml.")]
            [CommandArgument(0, "[outputPath]")]
            [DefaultValue("sample.yml")]
            public string OutputFile { get; init; }
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            var outputFile = settings.OutputFile ?? Path.Combine(Directory.GetCurrentDirectory(), "sample.yml");

            var sampleFile = Properties.Resources.sample;
            File.WriteAllBytes(outputFile, sampleFile);

            AnsiConsole.MarkupLine($"[green]Success:[/] Sample file generated at [bold]{outputFile}[/].");
            return 0;
        }
    }
}
