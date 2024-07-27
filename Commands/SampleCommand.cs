using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace TimelineGenerator.Commands
{
    internal class SampleCommand : Command<SampleCommand.Settings>
    {
        public sealed class Settings : CommandSettings
        {
            [Description("Path to write a sample file. Defaults to test.yml.")]
            [CommandArgument(0, "[outputPath]")]
            [DefaultValue("test.yml")]
            public string OutputPath { get; init; }
        }

        public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
        {
            var outputPath = settings.OutputPath ?? Path.Combine(Directory.GetCurrentDirectory(), "test.yml");

            var sampleFile = Properties.Resources.sample;
            File.WriteAllBytes(outputPath, sampleFile);

            return 0;
        }
    }
}
