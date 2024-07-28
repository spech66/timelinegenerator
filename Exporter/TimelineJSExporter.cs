using Markdig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimelineGenerator.Exporter
{
    // Repo: https://github.com/NUKnightLab/TimelineJS3
    // Docs: https://timeline.knightlab.com/
    internal class TimelineJSExporter : Exporter
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
            writer.WriteLine($"    <meta name=\"description\" content=\"{Escape(timeline.Description)}\">");
            writer.WriteLine("    <meta charset=\"utf-8\">");
            writer.WriteLine("    <style>");
            writer.WriteLine("      html, body {");
            writer.WriteLine("      height:100%;");
            writer.WriteLine("      padding: 0px;");
            writer.WriteLine("      margin: 0px;");
            writer.WriteLine("      }");
            writer.WriteLine("    </style>");
            writer.WriteLine("  </head>");
            writer.WriteLine("  <body>");
            WriteEvents(timeline, writer);
            writer.WriteLine("  </body>");
            writer.WriteLine("</html>");
            writer.WriteLine();
        }

        private void WriteEvents(YamlTimeline timeline, StreamWriter writer)
        {
            writer.WriteLine("<div id=\"timeline-embed\">");
            writer.WriteLine("  <div id=\"timeline\"></div>");
            writer.WriteLine("</div>");
            writer.WriteLine();
            writer.WriteLine("<link itle=\"timeline-styles\" rel=\"stylesheet\" href=\"https://cdn.knightlab.com/libs/timeline3/latest/css/timeline.css\">");
            writer.WriteLine("<script src=\"https://cdn.knightlab.com/libs/timeline3/latest/js/timeline-min.js\"></script>");
            writer.WriteLine();
            writer.WriteLine("<script>");
            WriteJson(timeline, writer);
            writer.WriteLine("  $(document).ready(function() {");
            writer.WriteLine("    var embed = document.getElementById('timeline-embed');");
            writer.WriteLine("    embed.style.height = getComputedStyle(document.body).height;");
            writer.WriteLine($"    window.timeline = new TL.Timeline('timeline-embed', timelineJson, {{");
            writer.WriteLine("      hash_bookmark: false");
            writer.WriteLine("    });");
            writer.WriteLine("    window.addEventListener('resize', function() {");
            writer.WriteLine("      var embed = document.getElementById('timeline-embed');");
            writer.WriteLine("      embed.style.height = getComputedStyle(document.body).height;");
            writer.WriteLine("      timeline.updateDisplay();");
            writer.WriteLine("    })");
            writer.WriteLine("  });");
            writer.WriteLine("</script>");
        }

        // Docs: https://timeline.knightlab.com/docs/json-format.html
        private void WriteJson(YamlTimeline timeline, StreamWriter writer)
        {
            writer.WriteLine("var timelineJson = {");
            writer.WriteLine($"  title: {{ text: {{ headline: \"{Escape(timeline.Title)}\", text: \"{Escape(timeline.Description)}\" }} }}, ");
            writer.WriteLine("  events: [");
            var events = timeline.Events.OrderBy(e => e.DateRange);
            foreach (var e in events)
            {
                writer.WriteLine("    {");
                writer.WriteLine($"      start_date: {{ year: {e.Start.Year}, month: {e.Start.Month}, day: {e.Start.Day} }},");
                if (e.End.HasValue)
                {
                    writer.WriteLine($"      end_date: {{ year: {e.End.Value.Year}, month: {e.End.Value.Month}, day: {e.End.Value.Day} }},");
                }

                writer.WriteLine($"      text: {{ headline: \"{Escape(e.Title)}\", text: \"{MarkdownToHtml(e.Description)}\" }},");

                if (!string.IsNullOrEmpty(e.Image))
                {
                    writer.WriteLine($"      media: {{ url: \"{e.Image}\", caption: \"{Escape(e.Title)}\" }},");
                }
                if (!string.IsNullOrEmpty(e.Link))
                {
                    writer.WriteLine($"      media: {{ url: \"{e.Link}\", text: \"Read more\" }},");
                }
                if(!string.IsNullOrEmpty(e.Category))
                {
                    writer.WriteLine($"      group: \"{e.Category}\",");
                }
                writer.WriteLine("    },");
            }

            writer.WriteLine("]");
            writer.WriteLine("};");
        }

        private object Escape(string title)
        {
            return title.Replace("\"", "&quot;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;");
        }

        private object MarkdownToHtml(string text)
        {
            return Markdown.ToHtml(text).Replace("\n", "<br>");
        }
    }
}
