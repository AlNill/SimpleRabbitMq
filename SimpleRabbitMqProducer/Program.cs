using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Producer main thread.");

var factory = new ConnectionFactory() { HostName = "192.168.194.128" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

string message = GetMessage(args);
var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(exchange: "logs", routingKey: string.Empty, null, body);
Console.WriteLine($"Sent logs - {message}");

Console.WriteLine($"Press [enter] to exit.");
Console.ReadLine();

static string GetMessage(string[] args)
{
    return args.Length > 0 ? string.Join(" ", args) : "Hello from producer";
}
