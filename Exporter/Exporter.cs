﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimelineGenerator.Exporter
{
    internal interface Exporter
    {
        string Export(YamlTimeline timeline, string outputPath);
    }
}
