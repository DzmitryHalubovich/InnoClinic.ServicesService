﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts.Service;
using Services.Services.Abstractions.Commands.Services;
using Services.Services.Abstractions.Queries.Services;

namespace Services.Presentation.Controllers;

[ApiController]
[Route("api/services")]
public class ServicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllServices(CancellationToken cancellationToken = default)
    {
        var getServicesResult = await _mediator.Send(new GetAllServicesQuery(), cancellationToken);

        return getServicesResult.Match<IActionResult>(Ok, notFound => NotFound());
    }

    [AllowAnonymous]
    [HttpGet("{id}", Name = "GetServiceById")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetServiceById([FromRoute] int id, 
        CancellationToken cancellationToken = default)
    {
        var getServiceByIdResult = await _mediator.Send(new GetServiceByIdQuery(id), 
            cancellationToken);

        return getServiceByIdResult.Match<IActionResult>(Ok, notFound => NotFound());
    }

    [Authorize(Roles = "Receptionist")]
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateService([FromBody] ServiceCreateDTO newService, 
        CancellationToken cancellationToken = default)
    {
        var createServiceResult = await _mediator.Send(new CreateServiceCommand(newService), 
            cancellationToken);

        return createServiceResult.Match<IActionResult>(
            createdService => CreatedAtRoute("GetServiceById", new { id = createdService.Id }, 
            createdService), notFound => NotFound());
    }

    [Authorize(Roles = "Receptionist")]
    [HttpPut("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UpdateService([FromRoute]int id, [FromBody] ServiceUpdateDTO editedService, 
        CancellationToken cancellationToken = default)
    {
        var updateServiceResult = await _mediator.Send(new UpdateServiceCommand(id, editedService), 
            cancellationToken);

        return updateServiceResult.Match<IActionResult>(success => NoContent(), notFound => NotFound());
    }

    [Authorize(Roles = "Receptionist")]
    [HttpDelete("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteService([FromRoute] int id, CancellationToken cancellationToken)
    {
        var deleteServiceResult = await _mediator.Send(new DeleteServiceCommand(id), cancellationToken);

        return deleteServiceResult.Match<IActionResult>(success => NoContent(), notFound => NotFound());
    }

    [Authorize(Roles = "Receptionist")]
    [HttpPatch("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeStatus([FromRoute] int id,[FromBody] int status)
    {
        var changeStatusResult = await _mediator.Send(new ChangeStatusServiceCommand(id, status));

        return changeStatusResult.Match<IActionResult>(success => NoContent(), notFound => NotFound());
    }
}
