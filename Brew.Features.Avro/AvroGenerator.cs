using Avro;
using Microsoft.Extensions.Logging;

namespace Brew.Features.Avro;

public class AvroGenerator(ILogger<AvroGenerator> logger)
{
    public void Execute()
    {
        var schemas = new List<string>()
        {
            @"
	            {
	              ""type"": ""enum"",
	              ""name"": ""Suit"",
	              ""namespace"": ""datadomain"",
	              ""symbols"" : [""SPADES"", ""HEARTS"", ""DIAMONDS"", ""CLUBS""]
	            }
	            ",
            @"
	            {
	                  ""type"": ""record"",
	                  ""name"": ""Person"",
		              ""namespace"": ""datadomain"",
	                  ""fields"" : [
	      	            { ""name"": ""Name"", ""type"": ""string""},
	                    { ""name"": ""Age"", ""type"": ""long"" }
	                  ]
	            }
	            "
        };

        CodeGen gen = new ();
        schemas.ForEach(json => gen.AddSchema(Schema.Parse(json)));
        _ = gen.GenerateCode();
            
        var baseDirectory = AppContext.BaseDirectory;
        var projectDirectory = Directory.GetParent(baseDirectory).Parent.Parent.Parent.Parent.FullName;
        var filePath = Path.Combine(projectDirectory, "Brew.Features.Avro.Models", "datadomain.cs");

        logger.LogInformation("Writing C# Models to: {Path}", filePath);
        gen.WriteCompileUnit(filePath);
    }
}