using Markdig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimelineGenerator.Exporter
{
    internal class BootstrapExporter: Exporter
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
            writer.WriteLine("<!doctype html>");
            writer.WriteLine("<html lang=\"en\">");
            writer.WriteLine("  <head>");
            writer.WriteLine("    <meta charset=\"utf-8\">");
            writer.WriteLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
            writer.WriteLine($"   <title>{Escape(timeline.Title)}</title>");
            writer.WriteLine("    <link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css\" rel=\"stylesheet\" integrity=\"sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN\" crossorigin=\"anonymous\">");
            writer.WriteLine("  </head>");
            writer.WriteLine("  <body>");
            writer.WriteLine("     <div class=\"container my-5\">");
            writer.WriteLine("       <div class=\"col-lg-6 px-0\">");
            writer.WriteLine($"        <h1>{Escape(timeline.Title)}</h1>");
            writer.WriteLine($"        <p class=\"lead mb-4\">{Escape(timeline.Description)}</p>");
            writer.WriteLine("       </div>");
            writer.WriteLine("       <div class=\"col-lg-8 px-0\">");
            WriteEvents(timeline, writer);
            writer.WriteLine("       </div>");
            writer.WriteLine("     </div>");
            writer.WriteLine("    <script src=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js\" integrity=\"sha384-C6RzsynM9kWDrMNeT87bh95OGNyZPhcTNXj1NW7RuBCsyN/o0jlpcV8Qyq46cDfL\" crossorigin=\"anonymous\"></script>");
            writer.WriteLine("  </body>");
            writer.WriteLine("</html>");
            writer.WriteLine();
        }

        private void WriteEvents(YamlTimeline timeline, StreamWriter writer)
        {
            var yamlEvents = timeline.Events.OrderBy(e => e.Start).ToList();

            foreach (var yamlEvent in yamlEvents)
            {
                writer.WriteLine("       <div class=\"card mb-3\">");
                writer.WriteLine("         <div class=\"card-body\">");
                writer.WriteLine($"          <h5 class=\"card-title\">{Escape(yamlEvent.Title)}</h5>");
                writer.WriteLine($"          <h6 class=\"card-subtitle mb-2 text-muted\">{Escape(yamlEvent.DateRange)}</h6>");
                writer.WriteLine($"          <p class=\"card-text\">{MarkdownToHtml(yamlEvent.Description)}</p>");
                writer.WriteLine("         </div>");
                WriteCardFooter(writer, yamlEvent);
                writer.WriteLine("       </div>");
                writer.WriteLine();
            }

        }

        private void WriteCardFooter(StreamWriter writer, YamlEvent? yamlEvent)
        {
            // Tags, Location, Category as badged in footer
            writer.WriteLine("           <div class=\"card-footer\">");
            if (!string.IsNullOrEmpty(yamlEvent.Category))
            {
                writer.WriteLine($"                 <span class=\"badge bg-info me-1\">{Escape(yamlEvent.Category)}</span>");
            }
            foreach (var tag in yamlEvent.Tags)
            {
                writer.WriteLine($"                 <span class=\"badge bg-secondary me-1\"># {Escape(tag)}</span>");
            }
            if (!string.IsNullOrEmpty(yamlEvent.Location))
            {
                writer.WriteLine($"                 <span class=\"badge bg-primary me-1\">@ {Escape(yamlEvent.Location)}</span>");
            }
            writer.WriteLine("           </div>");
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
