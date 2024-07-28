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
            WriteItems(timeline, writer);
            writer.WriteLine("      var container = document.getElementById('visualization');");
            writer.WriteLine("      var items = new vis.DataSet(data);");
            writer.WriteLine("      var options = {};");
            writer.WriteLine("      var timeline = new vis.Timeline(container, items, options);");
            writer.WriteLine("    </script>");
            writer.WriteLine("  </body>");
            writer.WriteLine("</html>");
            writer.WriteLine();
        }

        private void WriteItems(YamlTimeline timeline, StreamWriter writer)
        {
            writer.WriteLine("      var data = [");
            foreach (var e in timeline.Events)
            {
                writer.WriteLine("        {");
                writer.WriteLine($"          content: '{Escape(e.Title)}',");
                writer.WriteLine($"          start: '{e.Start:yyyy-MM-dd}',");
                if (e.End.HasValue)
                {
                    writer.WriteLine($"          end: '{e.End.Value:yyyy-MM-dd}',");
                }
                writer.WriteLine("        },");
            }
            writer.WriteLine("      ];");
        }

        private object Escape(string title)
        {
            return title.Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;");
        }
    }
}
