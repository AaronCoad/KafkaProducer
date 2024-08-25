using System.Reflection;
using System.Runtime;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
namespace KafkaProducer;

public class Producer
{
    ProducerConfig config { get; set; }
    SchemaRegistryConfig schemaRegistryConfig { get; set; }
    ProducerBuilder<string, Model> producerBuilder { get; set; }
    CachedSchemaRegistryClient cachedSchemaRegistryClient { get; set; }

    public Producer()
    {
        config = new ProducerConfig()
        {
            BootstrapServers = "localhost:9092"
        };

        schemaRegistryConfig = new SchemaRegistryConfig()
        {
            Url = "localhost:8085"
        };

        cachedSchemaRegistryClient = new CachedSchemaRegistryClient(schemaRegistryConfig);
        var serializer = new JsonSerializer<Model>(cachedSchemaRegistryClient);
        producerBuilder = new ProducerBuilder<string, Model>(config).SetValueSerializer(serializer).SetErrorHandler((producer, error) => { Console.WriteLine(error); });
    }

    public async Task<bool> SendMessage(string topic, string key, Model message)
    {
        try
        {
            var p = producerBuilder.Build();
            using (var producer = producerBuilder.Build())
            {
                var result = await producer.ProduceAsync(topic, new Message<string, Model>() { Key = key, Value = message });
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}