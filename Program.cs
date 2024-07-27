using TimelineGenerator;

Console.WriteLine("Timeline Generator CLI");

var sampleFile = TimelineGenerator.Properties.Resources.sample;
// Write file to test.yml
File.WriteAllBytes("test.yml", sampleFile);

// Parse test.yml
var timeline = YamlImporter.Import("test.yml");
Console.WriteLine(timeline.Version);
Console.WriteLine(timeline.Events.Count);
