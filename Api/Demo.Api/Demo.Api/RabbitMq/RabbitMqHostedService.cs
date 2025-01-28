using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Api.RabbitMq
{
    public class RabbitMqHostedService : IHostedService
    {
        private readonly RabbitMqListener _rabbitMqListener;

        public RabbitMqHostedService(RabbitMqListener rabbitMqListener)
        {
            _rabbitMqListener = rabbitMqListener;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => _rabbitMqListener.StartListening(), cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
