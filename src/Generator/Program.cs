using Faker;
using Bogus;
using System.Text.Json.Nodes;
using System.Text.Json;

List<Model> models = new List<Model>();
string outputLocation = "C:\\KafkaObjects";
int id = 1;
var faker = new Faker<Model>().RuleFor(p => p.Id, f => id++).RuleFor(p => p.Name, Faker.NameFaker.FirstName);
models.AddRange(faker.Generate(10000));
string json = JsonSerializer.Serialize(models);

var f = File.Create($"{outputLocation}\\values.json");
f.Close();
File.WriteAllText($"{outputLocation}\\values.json", json);