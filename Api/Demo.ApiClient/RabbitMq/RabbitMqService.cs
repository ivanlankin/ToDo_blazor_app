using Demo.ApiClient.RabbitMq;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

public class RabbitMqService : InerfaceRabbitMQ
{
    public void SendMessage(object obj)
    {
        var message = JsonSerializer.Serialize(obj);
        SendMessage(message);
    }

    public void SendMessage(string message)
    {
        var factory = new ConnectionFactory() { Uri = new Uri("amqps://sbzegsub:zgsmo_sE4cRML6ZiqTpISfWZFLy8gzDa@rattlesnake.rmq.cloudamqp.com/sbzegsub") };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "MyQueue",
                           durable: false,
                           exclusive: false,
                           autoDelete: false,
                           arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                           routingKey: "MyQueue",
                           basicProperties: null,
                           body: body);
        }
    }
}