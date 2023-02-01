namespace FaceIO.Api.Controllers
{
    using FaceIO.Commands.Location;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/customers/{customerUid:guid}/locations/{locationUid:guid}/groups/{groupUid:guid}")]
    public class LocationGroupsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LocationGroupsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddGroupToLocationAsync([FromRoute] Guid customerUid, [FromRoute] Guid groupUid, [FromRoute] Guid locationUid)
            => Ok(await _mediator.Send(new AddGroupToLocationCommand(customerUid: customerUid, groupUid: groupUid, locationUid: locationUid)));

        [HttpDelete]
        [Route("")]
        public async Task<IActionResult> RemoveGroupFromLocationAsync([FromRoute] Guid customerUid, [FromRoute] Guid groupUid, [FromRoute] Guid locationUid)
            => Ok(await _mediator.Send(new RemoveGroupFromLocationCommand(customerUid: customerUid, groupUid: groupUid, locationUid: locationUid)));
    }
}