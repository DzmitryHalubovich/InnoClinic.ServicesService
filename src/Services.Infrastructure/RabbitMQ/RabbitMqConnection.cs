using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using Services.Services.Abstractions.RabbitMQ;

namespace Services.Infrastructure.RabbitMQ;

public class RabbitMqConnection : IRabbitMqConnection, IDisposable
{
    private IConnection? _connection;
    private IConfiguration _configuration;

    public IConnection Connection => _connection!;

    public RabbitMqConnection(IConfiguration configuration)
    {
        _configuration = configuration;

        InitializeConnection();
    }

    private void InitializeConnection()
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration.GetSection("RabbitMQ:HostName").Value
        };

        _connection = factory.CreateConnection();

        EnsureQueuesExist(_connection);
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }

    private void EnsureQueuesExist(IConnection connection)
    {
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "specialization_updated",
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

        channel.QueueDeclare(queue: "specialization_status_changed",
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

        channel.QueueDeclare(queue: "service_deleted",
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

        channel.QueueDeclare(queue: "service_status_inactive_queue",
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
    }
}
