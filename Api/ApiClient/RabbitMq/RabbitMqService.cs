using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ApiClient.RabbitMq
{
    public class RabbitMqService : InterfaceRabbitMQ
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly Dictionary<string, TaskCompletionSource<object>> _waitingTasks;

        public RabbitMQService(string hostName)
        {
            _factory = new ConnectionFactory() { HostName = hostName };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _waitingTasks = new Dictionary<string, TaskCompletionSource<object>>();
        }

        public async Task<TResponse> SendMessageAndWaitForResponse<TResponse>(string queueName, object message)
        {
            var correlationId = Guid.NewGuid().ToString();
            var tcs = new TaskCompletionSource<object>();
            _waitingTasks.Add(correlationId, tcs);

            var props = _channel.CreateBasicProperties();
            props.CorrelationId = correlationId;
            props.ReplyTo = _channel.QueueDeclare().QueueName;

            var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            _channel.BasicPublish(exchange: "",
                                  routingKey: queueName,
                                  basicProperties: props,
                                  body: messageBytes);

            var response = await tcs.Task;
            _waitingTasks.Remove(correlationId);

            return JsonConvert.DeserializeObject<TResponse>(Encoding.UTF8.GetString((byte[])response));
        }

        public void StartListening(string queueName, Action<object> messageHandler)
        {
            _channel.QueueDeclare(queue: queueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = messageHandler(body);

                _channel.BasicPublish(exchange: "",
                                      routingKey: ea.BasicProperties.ReplyTo,
                                      basicProperties: null,
                                      body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response)));
            };

            _channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
