using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace KafkaProducer;
// See https://aka.ms/new-console-template for more information

public static class Program
{
    static string sourceLocation = "C:\\KafkaObjects";
    static string topic = "names";
    public static void Main(string[]? args)
    {
        string content = File.ReadAllText($"{sourceLocation}\\values.json");
        List<Model> models = JsonSerializer.Deserialize<List<Model>>(content);
        Producer producer = new Producer();
        models.ForEach(item =>
        {
            var result = producer.SendMessage(topic, item.Id.ToString(), item).Result;
            Console.WriteLine(result);
        });
    }
}
