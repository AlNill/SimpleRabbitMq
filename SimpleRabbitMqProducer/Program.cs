using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Producer main thread.");

var factory = new ConnectionFactory() { HostName = "192.168.194.128" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "task_queue",
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

string message = GetMessage(args);
var body = Encoding.UTF8.GetBytes(message);

var properties = channel.CreateBasicProperties();
properties.Persistent = true;

channel.BasicPublish(exchange: string.Empty,
                     routingKey: "task_queue",
                     basicProperties: properties,
                     body: body);

Console.WriteLine($"Sent message - {message}");

static string GetMessage(string[] args)
{
    return args.Length > 0 ? string.Join(" ", args) : "Hello from producer";
}
