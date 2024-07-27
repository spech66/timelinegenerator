Console.WriteLine("Timeline Generator CLI");

var sampleFile = TimelineGenerator.Properties.Resources.sample;
// Write file to test.yml
File.WriteAllBytes("test.yml", sampleFile);
