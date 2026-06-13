using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarParkManagement.Features.Parking.ParkCar;

[ApiController]
public class ParkCarEndpoint : ControllerBase
{
    private readonly IMediator _mediator;

    public ParkCarEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("parking")]
    public async Task<ActionResult> ParkCar(ParkCarCommand request, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(request, cancellationToken));
    }
}