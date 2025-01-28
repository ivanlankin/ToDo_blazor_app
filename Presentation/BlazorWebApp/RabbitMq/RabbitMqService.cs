using BlazorWebApp.RabbitMq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using BlazorWebApp.Models.ApiModels;

public class RabbitMqService : IRabbitMqService
{
    private const string QUEUE_NAME = "rpc_queue";

    private readonly IConnection connection;
    private readonly IModel channel;
    private readonly string replyQueueName;
    private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> callbackMapper = new();
    
    public RabbitMqService()
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri("amqps://jhkbkkdx:Y405x9X59_eyPL78XtaUt661rEC7bA53@rattlesnake.rmq.cloudamqp.com/jhkbkkdx")
        };

        connection = factory.CreateConnection();
        channel = connection.CreateModel();
        // declare a server-named queue
        replyQueueName = channel.QueueDeclare(
            queue: "queue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
            ).QueueName;
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            if (!callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                return;
            var body = ea.Body.ToArray();
            var response = Encoding.UTF8.GetString(body);
            tcs.TrySetResult(response);
        };

        channel.BasicConsume(consumer: consumer,
                             queue: replyQueueName,
                             autoAck: true);
    }

    public Task<string> SendMessage(string message, CancellationToken cancellationToken = default)
    {   
        IBasicProperties props = channel.CreateBasicProperties();
        var correlationId = Guid.NewGuid().ToString();
        props.CorrelationId = correlationId;
        props.ReplyTo = replyQueueName;
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var tcs = new TaskCompletionSource<string>();
        try
        {
            callbackMapper.TryAdd(correlationId, tcs);

            channel.BasicPublish(exchange: "",
                                 routingKey: QUEUE_NAME,
                                 basicProperties: props,
                                 body: messageBytes);
            Console.WriteLine($"сообщение доставлено");
            cancellationToken.Register(() => callbackMapper.TryRemove(correlationId, out _));
            return tcs.Task;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"=================================something happend=================================");
            Console.WriteLine(ex.ToString());
            return tcs.Task;
        }

    }
    public void Dispose()
    {
        // closing a connection will also close all channels on it
        connection.Close();
    }
}