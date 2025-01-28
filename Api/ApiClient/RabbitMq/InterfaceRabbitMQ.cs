using System;
using System.Collections.Generic;
using System.Text;

namespace ApiClient.RabbitMq
{
    public interface InterfaceRabbitMQ
    {
        void SendMessage(object obj);
        void SendMessage(string message);
        Task<string> SendMessageWithResponse(string message, string routingKey);
    }
}
