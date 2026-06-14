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
    public async Task<ActionResult> ParkCar([FromBody] ParkCarCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        if (result is null)
            return NotFound("No available parking spaces found");
        
        return Ok(result);
    }
}