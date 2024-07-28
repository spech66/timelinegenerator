using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimelineGenerator.Exporter
{
    // Docs: https://mermaid.js.org/syntax/timeline.html
    internal class MermaidExporter : Exporter
    {
        public string Export(YamlTimeline timeline, string outputPath)
        {
            var outputFile = outputPath + ".md";

            using (var writer = new StreamWriter(outputFile))
            {
                WriteHeader(timeline, writer);
                WriteMermaid(timeline, writer);
                WriteFooter(writer);

                return outputFile;
            }
        }

        private static void WriteHeader(YamlTimeline timeline, StreamWriter writer)
        {
            writer.WriteLine($"# {timeline.Title}");
            writer.WriteLine();
        }

        private void WriteMermaid(YamlTimeline timeline, StreamWriter writer)
        {
            writer.WriteLine("```mermaid");
            writer.WriteLine("timeline");
            writer.WriteLine($"    title {timeline.Title}");

            // Only date and title are supported. Categories are used as sections if multiple are present.
            var sections = timeline.Events.Select(e => e.Category).Where(s => s != string.Empty).Distinct().Count();
            if (sections > 1)
            {
                foreach (var category in timeline.Events.Select(e => e.Category).Distinct().Order())
                {
                    var section = category != string.Empty ? category : "Empty";
                    writer.WriteLine($"    section {section}");
                    foreach (var @event in timeline.Events.Where(e => e.Category == category).OrderBy(s => s.Start))
                    {
                        WriteEvent(writer, @event, 2);
                    }
                }
            }
            else
            {
                foreach (var @event in timeline.Events.OrderBy(s => s.Start))
                {
                    WriteEvent(writer, @event, 1);
                }
            }

            writer.WriteLine("```");
        }

        private void WriteEvent(StreamWriter writer, YamlEvent yamlEvent, int ident)
        {
            var identation = new string(' ', ident * 4);

            var endDate = yamlEvent.End.HasValue ? yamlEvent.End.Value.ToString("yyyy-MM-dd") : string.Empty;
            var end = endDate != string.Empty ? $"- {endDate}" : string.Empty;

            writer.WriteLine($"{identation}{yamlEvent.Start:yyyy-MM-dd} {end} : {Escape(yamlEvent.Title)}");
        }

        private static void WriteFooter(StreamWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine("Generated using [TimelineGenerator](https://github.com/spech66/timelinegenerator).");
        }

        private object Escape(string title)
        {
            return title.Replace(":", "\\:");
        }
    }
}
