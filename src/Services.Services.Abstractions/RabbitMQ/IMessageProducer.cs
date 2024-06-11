using InnoClinic.SharedModels.MQMessages.Services;
using InnoClinic.SharedModels.MQMessages.Specializations;

namespace Services.Services.Abstractions.Contracts;

public interface IMessageProducer
{
    void SendSpecializationUpdatedMessage(SpecializationUpdatedMessage message);

    void SendSpecializationStatusChangedMessage(SpecializationStatusChangedMessage message);

    void SendServiceDeletedMessage(ServiceDeletedMessage message);

    void SendServiceStatusChangedToInactiveMessage(ServiceStatusChangedToInactiveMessage message);
}
