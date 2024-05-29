using MediatR;
using OneOf.Types;
using OneOf;
using Services.Contracts.Specialization;

namespace Services.Services.Abstractions.Queries.Specializations;

public record GetSpecializationByIdQuery(int Id) : IRequest<OneOf<SpecializationResponseDTO, NotFound>>
{ }
