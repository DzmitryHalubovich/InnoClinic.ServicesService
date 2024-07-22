using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts.Filtering;
using Services.Contracts.Specialization;
using Services.Services.Abstractions.Commands.Specializations;
using Services.Services.Abstractions.Queries.Specializations;

namespace Services.Presentation.Controllers;

[ApiController]
[Route("api/specializations")]
public class SpecializationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SpecializationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSpecializations([FromQuery] SpecializationsQueryParameters queryParameters)
    {
        var getSpecializationResponse = await _mediator.Send(new GetAllSpecializationsQuery(queryParameters));

        return getSpecializationResponse.Match<IActionResult>(Ok, notFound => NotFound());
    }

    [HttpGet("{id}", Name = "GetSpecializationById")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSpecializationById([FromRoute] int id)
    {
        var getSpecializationResponse = await _mediator.Send(new GetSpecializationByIdQuery(id));

        return getSpecializationResponse.Match<IActionResult>(Ok, notFound => NotFound());
    }

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateSpecialization([FromBody] SpecializationCreateDTO newSpecialization)
    {
        var specResponse = await _mediator.Send(new CreateSpecializationCommand(newSpecialization));

        return CreatedAtRoute("GetSpecializationById", new { id = specResponse.Id }, specResponse);
    }

    [HttpPut("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UpdateSpecialization([FromRoute] int id,
        [FromBody] SpecializationUpdateDTO editedSpecialization)
    {
        var updateSpecializationResult =
            await _mediator.Send(new UpdateSpecializationCommand(id, editedSpecialization));

        return updateSpecializationResult.Match<IActionResult>(success => NoContent(), notFound => NotFound());
    }

    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSpecialization([FromRoute] int id)
    {
        var specializationDeleteResult = await _mediator.Send(new DeleteSpecializationCommand(id));

        return specializationDeleteResult.Match<IActionResult>(success => NoContent(), notFound => NotFound());
    }

    [HttpPatch("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ChangeStatus([FromRoute] int id, [FromBody] SpecializationChangeStatusDTO editedSpecialization)
    {
        var changeStatusResult = 
            await _mediator.Send(new ChangeSpecializationStatusCommand(id, editedSpecialization));

        return changeStatusResult.Match<IActionResult>(success => NoContent(), notFound => NotFound());
    }
}
