using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace Rabbitmq;
class Receive
{
    public static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "person",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var obj = Utils.ToObject<Person>(ea.Body.ToArray());
                var message = obj.Name;
                Console.WriteLine(" [x] Received {0}", message);
            };

            channel.BasicConsume(queue: "person",
                                 autoAck: true,
                                 consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}