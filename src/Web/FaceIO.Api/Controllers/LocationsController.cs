namespace FaceIO.Api.Controllers
{
    using FaceIO.Queries.Features.Location;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/customers/{customerUid:guid}/locations")]
    public class LocationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LocationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetLocationsAsync([FromRoute] Guid customerUid)
        {
            return Ok(await _mediator.Send(new GetLocationsQuery(customerUid)));
        }
    }
}
