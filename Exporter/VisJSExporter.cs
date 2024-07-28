using Markdig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimelineGenerator.Exporter
{
    // Repo: https://github.com/visjs/vis-timeline
    // Docs: https://visjs.github.io/vis-timeline/
    internal class VisJSExporter : Exporter
    {
        private const string DefaultGroupId = "empty";

        public string Export(YamlTimeline timeline, string outputPath)
        {
            var outputFile = outputPath + ".html";

            using (var writer = new StreamWriter(outputFile))
            {
                WriteHtml(timeline, writer);
            }

            return outputFile;
        }

        private void WriteHtml(YamlTimeline timeline, StreamWriter writer)
        {
            writer.WriteLine("<!DOCTYPE html>");
            writer.WriteLine("<html lang=\"en\">");
            writer.WriteLine("  <head>");
            writer.WriteLine($"    <title>{Escape(timeline.Title)}</title>");
            writer.WriteLine("    <script type=\"text/javascript\" src=\"https://unpkg.com/vis-timeline@latest/standalone/umd/vis-timeline-graph2d.min.js\"></script>");
            writer.WriteLine("    <link href=\"https://unpkg.com/vis-timeline@latest/styles/vis-timeline-graph2d.min.css\" rel=\"stylesheet\" type=\"text/css\" />");
            writer.WriteLine("    <style type=\"text/css\">");
            writer.WriteLine("      #visualization {");
            writer.WriteLine("        border: 1px solid lightgray;");
            writer.WriteLine("      }");
            writer.WriteLine("    </style>");
            writer.WriteLine("  </head>");
            writer.WriteLine("  <body>");
            writer.WriteLine("    <div id=\"visualization\"></div>");
            writer.WriteLine("    <script type=\"text/javascript\">");
            WriteGroups(timeline, writer);
            WriteItems(timeline, writer);
            writer.WriteLine("      var container = document.getElementById('visualization');");
            writer.WriteLine("      var items = new vis.DataSet(data);");
            writer.WriteLine("      var options = { align: 'left', orientation: 'top', stack: 'false' };");
            writer.WriteLine("      var timeline = new vis.Timeline(container, items, groups, options);");
            writer.WriteLine("    </script>");
            writer.WriteLine("  </body>");
            writer.WriteLine("</html>");
            writer.WriteLine();
        }

        private void WriteGroups(YamlTimeline timeline, StreamWriter writer)
        {
            var categories = timeline.Events.Select(e => e.Category).Distinct().ToList().Order();
            writer.WriteLine("      var groups = [");
            foreach (var c in categories)
            {
                if(string.IsNullOrEmpty(c))
                {
                    writer.WriteLine($"        {{ id: '{DefaultGroupId}', content: 'Default' }},");
                } else
                {
                    writer.WriteLine($"        {{ id: '{c}', content: '{c}' }},");
                }
            }
            writer.WriteLine("      ];");
        }

        private void WriteItems(YamlTimeline timeline, StreamWriter writer)
        {
            writer.WriteLine("      var data = [");
            foreach (var e in timeline.Events)
            {
                writer.WriteLine("        {");
                writer.WriteLine($"          content: '{MarkdownToHtml(e)}',");
                writer.WriteLine($"          start: '{e.Start:yyyy-MM-dd}',");
                if (e.End.HasValue)
                {
                    writer.WriteLine($"          end: '{e.End.Value:yyyy-MM-dd}',");
                }
                if(!string.IsNullOrEmpty(e.Category))
                {
                    writer.WriteLine($"          group: '{e.Category}',");
                } else
                {
                    writer.WriteLine($"          group: '{DefaultGroupId}',");
                }
                writer.WriteLine("        },");
            }
            writer.WriteLine("      ];");
        }

        private object Escape(string title)
        {
            return title.Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;");
        }

        private object MarkdownToHtml(YamlEvent yamlEvent)
        {
            var combinedText = $"# {yamlEvent.Title}\n\n";
            combinedText += yamlEvent.Description;

            if (!string.IsNullOrEmpty(yamlEvent.Location))
            {
                combinedText += $"\n\nLocation: {yamlEvent.Location}";
            }

            if (yamlEvent.Tags.Count > 0)
            {
                combinedText += $"\n\nTags: {string.Join(", ", yamlEvent.Tags)}";
            }

            return Markdown.ToHtml(combinedText).Replace("\n", "<br>");
        }
    }
}
