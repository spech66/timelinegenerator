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
            }

            return outputFile;
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
            var sectionCount = timeline.Events.Select(e => e.Category).Where(s => s != string.Empty).Distinct().Count();
            if (sectionCount > 1)
            {
                var categories = timeline.Events.Select(e => e.Category).Distinct().Order();
                foreach (var category in categories)
                {
                    var section = category != string.Empty ? category : "Empty";
                    writer.WriteLine($"    section {section}");
                    WriteEvents(timeline, writer, 2, category);
                }
            }
            else
            {
                WriteEvents(timeline, writer, 1, null);
            }

            writer.WriteLine("```");
        }

        private void WriteEvents(YamlTimeline timeline, StreamWriter writer, int ident, string? category = null)
        {
            var timelineEvents = timeline.Events;
            var events = timelineEvents.AsQueryable();

            if (category != null)
            {
                events = events.Where(e => e.Category == category);
            }
            events = events.OrderBy(e => e.DateRange);

            var lastDateRange = "";
            foreach (var yamlEvent in events)
            {
                WriteEvent(writer, yamlEvent, ident, lastDateRange);
                lastDateRange = yamlEvent.DateRange;
            }
        }

        private void WriteEvent(StreamWriter writer, YamlEvent yamlEvent, int ident, string lastDateRange)
        {                            
            var dateRange = yamlEvent.DateRange;

            // If the date range is the same as the last one, we group the events.
            if(yamlEvent.DateRange == lastDateRange)
            {
                ident++;
                dateRange = "";
            }

            var identation = new string(' ', ident * 4);

            writer.WriteLine($"{identation}{dateRange} : {Escape(yamlEvent.Title)}");
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
