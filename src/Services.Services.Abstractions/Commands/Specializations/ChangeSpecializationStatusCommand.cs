using MediatR;
using OneOf.Types;
using OneOf;
using Services.Contracts.Specialization;

namespace Services.Services.Abstractions.Commands.Specializations;

public record ChangeSpecializationStatusCommand(int Id, SpecializationChangeStatusDTO EditedSpecialization) : 
    IRequest<OneOf<Success, NotFound>>
{ }
