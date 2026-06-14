using CarParkManagement.Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarParkManagement.Features.Parking.FetchParkingSpaces;

[ApiController]
public class FetchParkingSpacesEndpoint : ControllerBase
{
    private readonly IMediator _mediator;

    public FetchParkingSpacesEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("parking")]
    public async Task<ActionResult<AvailableSpacesInfo>> FetchAvailableParkingSpaces(
        [FromQuery] FetchParkingSpacesQuery request,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(request, cancellationToken));
    }
}