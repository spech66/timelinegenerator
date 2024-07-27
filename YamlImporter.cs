using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TimelineGenerator
{
    // Docs: https://github.com/aaubry/YamlDotNet
    internal class YamlImporter
    {
        public static YamlTimeline Import(string path)
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();

            var yaml = File.ReadAllText(path);
            return deserializer.Deserialize<YamlTimeline>(yaml);
        }
    }
}
