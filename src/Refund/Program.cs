using System.Text;
using Core;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory
{
    HostName = Configs.HOST_NAME
};

using var connection = factory.CreateConnection();
var channel = connection.CreateModel();

// Creating queue
channel.QueueDeclare(queue: Queues.QUEUE_REFUND_SERVICE, exclusive: false, autoDelete: false);

channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, eventArgs) =>
{
    Console.WriteLine($"Started process message, tag {eventArgs.DeliveryTag}");

    var body = eventArgs.Body.ToArray();
    var messageJson = Encoding.UTF8.GetString(body);

    var refund = JsonConvert.DeserializeObject<RefundDto>(messageJson);

    if (refund is null)
    {
        Console.WriteLine("Refund not consumed, fail on parse message");
        return;
    }
    
    Console.WriteLine($"Refund consumed, Customer: {refund.Customer}");
};

channel.BasicConsume(queue: Queues.QUEUE_REFUND_SERVICE, autoAck: true, consumer: consumer);

Console.WriteLine($"Initialized");