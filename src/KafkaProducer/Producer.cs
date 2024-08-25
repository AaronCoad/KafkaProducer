using System.Reflection;
using System.Runtime;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Configuration;
namespace KafkaProducer;

public class Producer : IDisposable
{
    ProducerConfig config { get; set; }
    SchemaRegistryConfig schemaRegistryConfig { get; set; }
    ProducerBuilder<string, Model> producerBuilder { get; set; }
    CachedSchemaRegistryClient cachedSchemaRegistryClient { get; set; }

    public Producer(IConfigurationSection configurationSection)
    {
        config = new ProducerConfig()
        {
            BootstrapServers = configurationSection["Brokers"]
        };

        schemaRegistryConfig = new SchemaRegistryConfig()
        {
            Url = configurationSection["SchemaRegistry"]
        };

        cachedSchemaRegistryClient = new CachedSchemaRegistryClient(schemaRegistryConfig);
        var serializer = new JsonSerializer<Model>(cachedSchemaRegistryClient);
        producerBuilder = new ProducerBuilder<string, Model>(config).SetValueSerializer(serializer)
                                                                    .SetErrorHandler((producer, error) => { Console.WriteLine(error); });
    }

    public async Task<bool> SendMessage(string? topic, string key, Model message)
    {
        if (String.IsNullOrWhiteSpace(topic))
            throw new ArgumentNullException("Topic cannot be null");

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

    public void Dispose()
    {
    }
}