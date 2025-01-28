using BlazorWebApp.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BlazorWebApp.RabbitMq
{
    public interface IRabbitMqService : IDisposable
    {
        Task<string> SendMessage(string message, CancellationToken cancellationToken = default);
    }
}
