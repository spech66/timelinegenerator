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
            throw new NotImplementedException();
        }
    }
}
