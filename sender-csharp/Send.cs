using RabbitMQ.Client;
using Google.Protobuf;

namespace Rabbitmq;

class Send
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

            bool exit = false;

            while (!exit)
            {
                Console.WriteLine(" MENU:");
                Console.WriteLine("--------------------");
                Console.WriteLine(" 1 - Send a mesage with a person data");
                Console.WriteLine(" 2 - Exit");

                var result = Console.ReadKey();
                Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r\n");
                switch (result.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        SendMessage(channel);
                        break;
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine(" Unknown option. Press [enter] and try again...");
                        Console.ReadLine();
                        Console.Clear();
                        break;
                }
            }
        }
    }

    private static void SendMessage(IModel channel)
    {
        Console.Write("Name: ");
        var name = Console.ReadLine();
        Console.Write("City: ");
        var city = Console.ReadLine();
        Console.Write("Address: ");
        var address = Console.ReadLine();

        var rand = new Random();

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(address))
        {
            Console.WriteLine("Mandatory data is missing. Press [enter] and try again...");
            Console.ReadLine();
            Console.Clear();
            return;
        }


        Person person = new Person
        {
            Id = rand.Next(),
            Name = name,
            Address = new Address
            {
                Id = rand.Next(),
                AddressLine1 = address,
                City = city
            }
        };

        byte[] message = person.ToByteArray();

        channel.BasicPublish(exchange: "",
                             routingKey: "person",
                             basicProperties: null,
                             body: message);
        Console.WriteLine("");
        Console.WriteLine($"[x] Sent {person.Name}");
        Console.WriteLine("Press [enter] to go back to menu");
        Console.ReadLine();
        Console.Clear();
    }
}