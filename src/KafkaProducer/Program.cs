using System.Text.Json;
using Microsoft.Extensions.Configuration;
using KafkaProducer;

var config = new ConfigurationBuilder().AddJsonFile($"{Environment.CurrentDirectory}\\appsettings.json").Build();
string content = File.ReadAllText(config["DataSourceFile"]!);
List<Model>? models = JsonSerializer.Deserialize<List<Model>>(content);

if (models == null)
{
    Console.WriteLine("Unable to generate list");
    System.Environment.Exit(1);
}

using (Producer producer = new Producer(config.GetSection("KafkaProducer")))
{
    models.ForEach(item =>
    {
        var result = producer.SendMessage(config["Topic"], item.Id.ToString(), item).Result;
        Console.WriteLine(result);
    });
}
