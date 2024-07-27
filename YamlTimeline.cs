namespace TimelineGenerator
{
    internal class YamlEvent
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        public string Category { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public string Link { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;
    }

    internal class YamlTimeline
    {
        public string Version { get; set; } = "1.0";

        public string Title { get; set; } = "Timeline";

        public List<YamlEvent> Events { get; set; } = new List<YamlEvent>();
    }
}
