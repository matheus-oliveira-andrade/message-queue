using System.Text;
using System.Text.Json;
using Core;
using RabbitMQ.Client;

var factory = new ConnectionFactory()
{
    HostName = Configs.HOST_NAME,
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// Creating queue
channel.QueueDeclare(queue: Queues.QUEUE_REFUND_SERVICE, exclusive: false, autoDelete: false);

var refund = new RefundDto
{
    Customer = "01234567890",
    OrderId = Guid.NewGuid(),
    Amount = 125.54f,
    CreatedAt = DateTime.Now
};

var messageJson = JsonSerializer.Serialize(refund);
var body = Encoding.UTF8.GetBytes(messageJson);

channel.BasicPublish(exchange: "", routingKey: Queues.QUEUE_REFUND_SERVICE, body: body);

Console.WriteLine($"Published message, customer: {refund.Customer}");