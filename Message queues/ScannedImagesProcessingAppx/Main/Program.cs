using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace MainProcessingService;

internal class Program
{
    private const string CentralizedControlExchange = "CentralizedControlExchange";
    private const string CentralizedControlQueue = "CentralizedControlQueue";
    private static IConnection? Connection { get; set; }
    private static IModel? Channel { get; set; }
    static void Main(string[] args)
    {
        ReceiveStateMessage();
        var sendThread = new Thread(() =>
        {
            while (true)
            {
                var newMessageSize = Console.ReadLine();
                SendMessage(newMessageSize);
            }
        });
        sendThread.IsBackground = false;
        sendThread.Start();
    }

    private static void SendMessage(string? newMessageSize)
    {
        if (!int.TryParse(newMessageSize, out int messageSizeInt))
            return;

        Channel.ExchangeDeclare(CentralizedControlExchange, ExchangeType.Direct, true, false);

        var headers = new Dictionary<string, object?>
                {
                    { "NewMessageSize", messageSizeInt }
                };
        var basicProperties = Channel?.CreateBasicProperties();
        basicProperties.Headers = headers;
        Channel.BasicPublish(CentralizedControlExchange, "centralized.control", basicProperties, null);
    }

    private static void ReceiveStateMessage()
    {
        var factory = new ConnectionFactory();
        factory.Uri = new Uri("amqp://mainprocessingservice:mainprocessingservice123@localhost:5672");

        Connection = factory.CreateConnection();
        Channel = Connection.CreateModel();

        AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
        {
            Connection.Close();
            Channel.Close();
        };

        Channel.QueueDeclare(CentralizedControlQueue, true, false, false);
        Channel.QueueBind(CentralizedControlQueue, CentralizedControlExchange, "status.update");

        var consumer = new EventingBasicConsumer(Channel);
        consumer.Received += (sender, args) =>
        {
            var headers = args.BasicProperties.Headers;
            var currentState = Encoding.UTF8.GetString((byte[])headers["CurrentState"]);
            var messageSize = Convert.ToInt32(headers["MessageSize"]);
            DisplayState(currentState, messageSize);
        };

        Channel.BasicConsume(CentralizedControlQueue, true, consumer);
    }

    private static void DisplayState(string currentState, int messageSize)
    {
        Console.Clear();
        Console.WriteLine(DateTime.Now);
        Console.WriteLine($"Current state: {currentState}");
        Console.WriteLine($"Current message size: {messageSize}");
        Console.WriteLine();
        Console.WriteLine("Set a new message size in MB:");
    }

}
