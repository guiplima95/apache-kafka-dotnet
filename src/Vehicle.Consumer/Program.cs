using Confluent.Kafka;

ConsumerConfig config = new()
{
    GroupId = "Vehicle.Dev.IO",
    BootstrapServers = "localhost:9092"
};

using var consumer = new ConsumerBuilder<string, string>(config).Build();

consumer.Subscribe("vehicle-dev");

while (true)
{
    var result = consumer.Consume();
    Console.WriteLine($"Message: {result.Message.Key}-{result.Message.Value}");
}