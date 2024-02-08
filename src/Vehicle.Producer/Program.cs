using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Domain.Events;

var schemaConfig = new SchemaRegistryConfig
{
    Url = "http://localhost:8081"
};

var schemaRegistry = new CachedSchemaRegistryClient(schemaConfig);

ProducerConfig config = new()
{
    BootstrapServers = "localhost:9092"
};

using var producer = new ProducerBuilder<string, Vehicle>(config)
    .SetValueSerializer(new AvroSerializer<Vehicle>(schemaRegistry))
    .Build();

var message = new Message<string, Vehicle>
{
    Key = Guid.NewGuid().ToString(),
    Value = new Vehicle
    {
        Id = Guid.NewGuid().ToString(),
        DateUpdateUtc = DateTime.UtcNow,
        Model = "HB 20 1.6 Confort Plus",
        ModelYear = 2019,
        Plate = "AAA4444"
    }
};

var result = await producer.ProduceAsync("vehicle-dev", message);

Console.WriteLine($"Offset: {result.Offset}, Partition: {result.Partition}");