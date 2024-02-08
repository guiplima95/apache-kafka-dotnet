using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Domain.Events;

var schemaConfig = new SchemaRegistryConfig
{
    Url = "http://localhost:8081"
};

var schemaRegistry = new CachedSchemaRegistryClient(schemaConfig);

ConsumerConfig config = new()
{
    GroupId = "Vehicle.Dev.IO",
    BootstrapServers = "localhost:9092"
};

using var consumer = new ConsumerBuilder<string, Vehicle>(config)
    .SetValueDeserializer(new AvroDeserializer<Vehicle>(schemaRegistry).AsSyncOverAsync())
    .Build();

consumer.Subscribe("vehicle-dev");

while (true)
{
    var result = consumer.Consume();
    Console.WriteLine($"Message: {result.Message.Key} - Plate: {result.Message.Value.Plate}");
}