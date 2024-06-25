using InnoClinic.SharedModels.MQMessages.Services;
using InnoClinic.SharedModels.MQMessages.Specializations;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Services.Services.Abstractions.Contracts;
using Services.Services.Abstractions.RabbitMQ;
using System.Text;

namespace Services.Infrastructure.RabbitMQ;

public class MessageProducer : IMessageProducer
{ 
    private readonly IRabbitMqConnection _connection;

    public MessageProducer(IRabbitMqConnection connection)
    {
        _connection = connection;
    }

    public void SendSpecializationUpdatedMessage(SpecializationUpdatedMessage message)
    {
        using var channel = _connection.Connection.CreateModel();

        var queueName = "specialization_updated";

        var exchangeName = "specialization_full_update";

        channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);

        channel.QueueBind(queue: queueName,
                          exchange: exchangeName,
                          routingKey: queueName);

        var json = JsonConvert.SerializeObject(message);

        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: exchangeName, 
                             routingKey: queueName, 
                             basicProperties: null,
                             body: body);
    }

    public void SendSpecializationStatusChangedMessage(SpecializationStatusChangedMessage message)
    {
        using var channel = _connection.Connection.CreateModel();

        var queueName = "specialization_status_changed";

        var exchangeName = "change_status";

        channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);

        channel.QueueBind(queue: queueName,
                          exchange: exchangeName,
                          routingKey: queueName);

        var json = JsonConvert.SerializeObject(message);

        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: exchangeName,
                             routingKey: queueName,
                             basicProperties: null,
                             body: body);
    }

    public void SendServiceDeletedMessage(ServiceDeletedMessage message)
    {
        using var channel = _connection.Connection.CreateModel();

        var queueName = "service_deleted_queue";
        var exchangeName = "service_deleted";

        channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);

        channel.QueueBind(queue: queueName,
                          exchange: exchangeName,
                          routingKey: queueName);

        var jsonMessage = JsonConvert.SerializeObject(message);

        var messageBodyBytes = Encoding.UTF8.GetBytes(jsonMessage);

        channel.BasicPublish(exchange: exchangeName,
                             routingKey: queueName,
                             basicProperties: null,
                             body: messageBodyBytes);
    }

    public void SendServiceStatusChangedToInactiveMessage(ServiceStatusChangedToInactiveMessage message)
    {
        using var channel = _connection.Connection.CreateModel();

        var queueName = "service_status_changed_to_inactive_queue";
        var exchangeName = "service_status_changed_to_inactive";

        channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);

        channel.QueueBind(queue: queueName,
                         exchange: exchangeName,
                         routingKey: queueName);

        var jsonMessage = JsonConvert.SerializeObject(message);

        var messageBodyBytes = Encoding.UTF8.GetBytes(jsonMessage);

        channel.BasicPublish(exchange: exchangeName,
                             routingKey: queueName,
                             basicProperties: null,
                             body: messageBodyBytes);
    }
}
