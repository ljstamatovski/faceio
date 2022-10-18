namespace FaceIO.Api.Controllers
{
    using FaceIO.Commands.GroupLocation;
    using FaceIO.Queries.Features.GroupLocation;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/customers/{customerUid:guid}/groups/{groupUid:guid}/locations/{locationUid:guid}/grouplocations")]
    public class GroupLocationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GroupLocationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetGroupLocationsAsync([FromRoute] Guid customerUid, [FromRoute] Guid groupUid, [FromRoute] Guid locationUid)
            => Ok(await _mediator.Send(new GetGroupLocationsQuery(customerUid, groupUid, locationUid)));

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> GreateGroupLocationAsync([FromRoute] Guid customerUid, [FromRoute] Guid groupUid, [FromRoute] Guid locationUid)
            => Ok(await _mediator.Send(new AddLocationToGroupCommand(customerUid: customerUid, groupUid: groupUid, locationUid: locationUid)));

        [HttpDelete]
        [Route("{groupLocationUid:guid}")]
        public async Task<IActionResult> DeleteGroupLocationAsync([FromRoute] Guid customerUid, [FromRoute] Guid groupUid, [FromRoute] Guid locationUid, [FromRoute] Guid groupLocationUid)
            => Ok(await _mediator.Send(new RemoveLocationFromGroupCommand(customerUid: customerUid, groupUid: groupUid, locationUid: locationUid, groupLocationUid: groupLocationUid)));
    }
}