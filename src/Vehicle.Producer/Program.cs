using Confluent.Kafka;

ProducerConfig config = new()
{
    BootstrapServers = "localhost:9092"
};

using var producer = new ProducerBuilder<string, string>(config).Build();

var message = new Message<string, string>
{
    Key = Guid.NewGuid().ToString(),
    Value = $"Message test plate: LSN4444 date: {DateTime.UtcNow}"
};

var result = await producer.ProduceAsync("vehicle-dev", message);

Console.WriteLine($"Offset: {result.Offset}, Partition: {result.Partition}");