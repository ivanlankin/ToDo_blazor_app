using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.ApiClient.RabbitMq
{
    public interface InerfaceRabbitMQ
    {
        void SendMessage(object obj);
        void SendMessage(string message);
    }
}
