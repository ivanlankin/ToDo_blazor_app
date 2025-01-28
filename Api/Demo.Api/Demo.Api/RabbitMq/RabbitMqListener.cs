using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using System.Text;
using System.Text.Json;
using System.Diagnostics;
using System;
using Demo.Api.Controllers;
using Demo.Api.Data;
using Demo.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

namespace Demo.Api.RabbitMq
{
    public class RabbitMqListener
    {
        private readonly DemoDbContext _demoDbContext;
        private readonly ConnectionFactory _factory;

        public RabbitMqListener(IConfiguration conf)
        {

            var rabbitMqConnectionString = conf.GetConnectionString("RabbitMq");

            _factory = new ConnectionFactory
            {
                Uri = new Uri(rabbitMqConnectionString)
            };
        }

        public void StartListening()
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "rpc_queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue: "rpc_queue",
                     autoAck: false,
                     consumer: consumer);
                consumer.Received += (model, ea) =>
                {
                    string response = string.Empty;

                    var body = ea.Body.ToArray();
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    //try
                    //{
                        Thread.Sleep(5000);
                        var message = Encoding.UTF8.GetString(body);
                        message = message.Replace("Action", "\"Action\"").Replace("Model", "\"Model\"").Replace("Id", "\"Id\"");
                        Console.WriteLine("Received message: {0}", message);
                        var command = JsonSerializer.Deserialize<TaskRequest>(message);
                        TaskToDoController ttd = new TaskToDoController(_demoDbContext);
                        if (command.Action == "GetTaskToDos")
                            response = JsonSerializer.Serialize(ttd.Get());
                        else if (command.Action == "GetById")
                            response = JsonSerializer.Serialize(ttd.GetById(command.Id));
                        else if (command.Action == "SaveTaskToDo")
                            response = JsonSerializer.Serialize(ttd.Create(command.Model));
                        else if (command.Action == "UpdateTaskToDo")
                            response = JsonSerializer.Serialize(ttd.Update(command.Model));
                        else if (command.Action == "DeleteTaskToDo")
                            response = JsonSerializer.Serialize(ttd.Delete(command.Id));
                    //}
                    //catch (Exception ex) 
                    //{
                    //    try
                    //    {
                    //        var message = Encoding.UTF8.GetString(body);
                    //        var command = JsonSerializer.Deserialize<UserRequest>(message);
                    //        UserAccountController ua = new UserAccountController(_demoDbContext);
                    //        if (command.Action == "SaveUser")
                    //            response = JsonSerializer.Serialize(ua.Create(command.Model));
                    //        else if (command.Action == "UpdateUser")
                    //            response = JsonSerializer.Serialize(ua.Update(command.Model));
                    //        else if (command.Action == "GetUser")
                    //            response = JsonSerializer.Serialize(ua.Get());
                    //        else if (command.Action == "DeleteUser")
                    //            response = JsonSerializer.Serialize(ua.Delete(command.Id));

                    //    }
                    //    catch (Exception ex2)
                    //    {
                    //        Console.WriteLine($" [.] {ex2.Message}");
                    //        response = string.Empty;
                    //    }
                    //}
                    //finally
                    //{
                        var responseBytes = Encoding.UTF8.GetBytes(response);
                        channel.BasicPublish(exchange: string.Empty,
                                             routingKey: props.ReplyTo,
                                             basicProperties: replyProps,
                                             body: responseBytes);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    //}

                };

                Console.WriteLine("Listener started. Press any key to exit.");
                Console.ReadLine();
            }
        }
    }
    public class  TaskRequest
    {
        public string Action { get; set; }
        public TaskToDo? Model { get; set; }
        public int Id { get; set; }
    }
    public class UserRequest
    {
        public string Action { get; set; }
        public UserAccount? Model { get; set; }
        public int Id { get; set; }
    }
}
