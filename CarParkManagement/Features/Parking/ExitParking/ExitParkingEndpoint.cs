using CarParkManagement.Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarParkManagement.Features.Parking.ExitParking;

[ApiController]
public class ExitParkingEndpoint : ControllerBase
{
    private readonly IMediator _mediator;

    public ExitParkingEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("parking/exit")]
    public async Task<ActionResult<CarExitInfo>> ExitParking(
        [FromBody] ExitParkingCommand request,
        CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(request, cancellationToken));
    }
}