using MediatR;
using Services.Contracts.Specialization;

namespace Services.Services.Abstractions.Commands.Specializations;

public record CreateSpecializationCommand(SpecializationCreateDTO NewSpecialization) : IRequest<SpecializationResponseDTO>
{ }
