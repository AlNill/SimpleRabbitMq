using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Consumer main thread.");


var factory = new ConnectionFactory() { HostName = "192.168.194.128" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic);

var queueName = channel.QueueDeclare().QueueName;

if (args.Length < 1)
{
    Console.Error.WriteLine("Usage: {0} [binding_key...]",
                            Environment.GetCommandLineArgs()[0]);
    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
    Environment.ExitCode = 1;
    return;
}

foreach (var bindingKey in args)
{
    channel.QueueBind(queue: queueName,
                      exchange: "topic_logs",
                      routingKey: bindingKey);
}

Console.WriteLine(" [*] Waiting for messages.");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
    Console.WriteLine($" [*] Received \"{message}\"");
};

channel.BasicConsume(queue: queueName,
                     autoAck: true,
                     consumer: consumer);

Console.WriteLine(" [*] Press enter to exit.");
Console.ReadLine();