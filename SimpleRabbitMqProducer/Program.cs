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

string message = "Hello from producer";
var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(exchange: string.Empty,
                     routingKey: "hello",
                     basicProperties: null,
                     body: body);

Console.WriteLine($"Sent message - {message}");
