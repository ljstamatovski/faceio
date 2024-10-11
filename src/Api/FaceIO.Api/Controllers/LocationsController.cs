namespace FaceIO.Api.Controllers
{
    using FaceIO.Commands.Location;
    using FaceIO.Contracts.Location;
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
            => Ok(await _mediator.Send(new GetLocationsQuery(customerUid)));

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateLocationAsync([FromRoute] Guid customerUid, [FromBody] CreateLocationRequest request)
            => Ok(await _mediator.Send(new AddLocationCommand(customerUid: customerUid, name: request.Name, description: request.Description)));

        [HttpPost]
        [Route("{locationUid:guid}/verify")]
        public async Task<IActionResult> VerifyAccessToLocationAsync([FromRoute] Guid customerUid, [FromRoute] Guid locationUid, IFormFile image)
        {
            var memoryStream = new MemoryStream();

            await using (Stream stream = image.OpenReadStream())
                stream.CopyTo(memoryStream);

            return Ok(await _mediator.Send(new VerifyAccessToLocationCommand(customerUid: customerUid, locationUid: locationUid, stream: memoryStream)));
        }

        [HttpGet]
        [Route("{locationUid:guid}")]
        public async Task<IActionResult> GetLocationAsync([FromRoute] Guid customerUid, [FromRoute] Guid locationUid)
            => Ok(await _mediator.Send(new GetLocationQuery(customerUid: customerUid, locationUid: locationUid)));

        [HttpPatch]
        [Route("{locationUid:guid}")]
        public async Task<IActionResult> UpdateLocationAsync([FromRoute] Guid customerUid, [FromRoute] Guid locationUid, [FromBody] UpdateLocationRequest request)
            => Ok(await _mediator.Send(new UpdateLocationCommand(customerUid: customerUid, locationUid: locationUid, name: request.Name, description: request.Description)));

        [HttpDelete]
        [Route("{locationUid:guid}")]
        public async Task<IActionResult> DeleteLocationAsync([FromRoute] Guid customerUid, [FromRoute] Guid locationUid)
            => Ok(await _mediator.Send(new RemoveLocationCommand(customerUid: customerUid, locationUid: locationUid)));
    }
}