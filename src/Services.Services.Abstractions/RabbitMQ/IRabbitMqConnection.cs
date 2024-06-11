using RabbitMQ.Client;

namespace Services.Services.Abstractions.RabbitMQ;

public interface IRabbitMqConnection
{
    IConnection Connection { get; }
}
