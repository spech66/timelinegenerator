using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimelineGenerator.Exporter
{
    internal class ExporterFactory
    {
        public static Exporter Create(string exporter)
        {
            return exporter switch
            {
                "timelinejs" => new TimelineJSExporter(),
                "visjs" => new VisJSExporter(),
                "bootstrap" => new BootstrapExporter(),
                _ => throw new ArgumentException($"Unknown exporter: {exporter}"),
            };
        }
    }
}
