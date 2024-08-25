using Bogus;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder().AddJsonFile($"{Environment.CurrentDirectory}\\appsettings.json")
                                       .Build();
List<Model> models = new List<Model>();
int id = 1;
var faker = new Faker<Model>().RuleFor(p => p.Id,
                                       f => id++)
                              .RuleFor(p => p.Name,
                                       Faker.NameFaker.FirstName);
models.AddRange(faker.Generate(1000));

string json = JsonSerializer.Serialize(models);
using (FileStream fs = File.Create(config["OutputFile"]!))
{
    using (StreamWriter sw = new StreamWriter(fs))
    {
        sw.Write(json);
    }
}