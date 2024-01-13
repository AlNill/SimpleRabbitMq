using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Producer main thread.");

var factory = new ConnectionFactory() { HostName = "192.168.194.128" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

string message = GetMessage(args);
var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(exchange: string.Empty,
                     routingKey: "hello",
                     basicProperties: null,
                     body: body);

Console.WriteLine($"Sent message - {message}");

static string GetMessage(string[] args)
{
    return args.Length > 0 ? string.Join(" ", args) : "Hello from producer";
}
