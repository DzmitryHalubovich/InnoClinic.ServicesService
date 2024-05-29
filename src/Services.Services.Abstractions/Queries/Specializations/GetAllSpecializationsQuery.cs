using MediatR;
using OneOf.Types;
using OneOf;
using Services.Contracts.Filtering;
using Services.Contracts.Specialization;

namespace Services.Services.Abstractions.Queries.Specializations;

public record GetAllSpecializationsQuery(SpecializationsQueryParameters QueryParameters) : 
    IRequest<OneOf<List<SpecializationResponseDTO>, NotFound>>
{ }
