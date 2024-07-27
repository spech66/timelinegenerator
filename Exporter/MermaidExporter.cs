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
        public void Export(YamlTimeline timeline, string outputPath)
        {
            var outputFile = outputPath + ".md";

            using (var writer = new StreamWriter(outputFile))
            {
                writer.WriteLine($"# {timeline.Title}");
                writer.WriteLine();
                writer.WriteLine("```mermaid");
                writer.WriteLine("timeline");
                writer.WriteLine($"    title {timeline.Title}");

                // Only date and title are supported. Categories are used as sections if multiple are present.
                var sections = timeline.Events.Select(e => e.Category).Where(s => s != string.Empty).Distinct().Count();
                if (sections > 1)
                {
                    foreach (var category in timeline.Events.Select(e => e.Category).Distinct())
                    {
                        writer.WriteLine($"    section {category}");
                        foreach (var @event in timeline.Events.Where(e => e.Category == category).OrderBy(s => s.Start))
                        {
                            var end = @event.End.HasValue ? @event.End.Value.ToString("yyyy-MM-dd") : string.Empty;
                            writer.WriteLine($"        {@event.Start:yyyy-MM-dd} {end} : {Escape(@event.Title)}");
                        }
                    }
                } else
                {
                    foreach (var @event in timeline.Events.OrderBy(s => s.Start))
                    {
                        var end = @event.End.HasValue ? @event.End.Value.ToString("yyyy-MM-dd") : string.Empty;
                        writer.WriteLine($"        {@event.Start:yyyy-MM-dd} {end} : {Escape(@event.Title)}");
                    }
                }

                writer.WriteLine("```");
                writer.WriteLine();
                writer.WriteLine("Generated using [TimelineGenerator](https://github.com/spech66/timelinegenerator).");
            }
        }

        private object Escape(string title)
        {
            return title.Replace(":", "\\:");
        }
    }
}
