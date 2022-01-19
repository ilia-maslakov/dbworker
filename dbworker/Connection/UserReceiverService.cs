using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using dbworker.Data.EF;
using dbworker.Controllers;
using Microsoft.Extensions.Logging;

namespace dbworker.Connection
{
    public class UserReceiverService : BackgroundService
    {
        private IServiceProvider _sp;
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;
        private readonly ILogger<UserReceiverService> _logger;

        public UserReceiverService(ILogger<UserReceiverService> logger, IServiceProvider sp)
        {
            _sp = sp;
            _factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "rabbit_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                _channel.Dispose();
                _connection.Dispose();

                return Task.CompletedTask;
            }

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} Received message from Rabbit({message})");

                Task.Run(() =>
                {
                    using (var scope = _sp.CreateScope())
                    {
                        var opts = message.Split(";");
                        if (opts.Length == 4)
                        {
                            var db = scope.ServiceProvider.GetRequiredService<IUserController>();
                            _logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()} try add User({opts[1]}, {opts[2]}, {opts[3]})");
                            var u = db.Add(opts[1], opts[2], opts[3]);
                        }

                    }
                });
            };

            _channel.BasicConsume(queue: "rabbit_queue", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
    }
}