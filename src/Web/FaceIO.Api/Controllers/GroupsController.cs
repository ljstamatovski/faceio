namespace FaceIO.Api.Controllers
{
    using FaceIO.Commands.Group;
    using FaceIO.Contracts.Group;
    using FaceIO.Queries.Features.Group;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/customers/{customerUid:guid}/groups")]
    public class GroupsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GroupsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetGroupsAsync([FromRoute] Guid customerUid)
            => Ok(await _mediator.Send(new GetGroupsQuery(customerUid: customerUid)));

        [HttpGet]
        [Route("{groupUid:guid}")]
        public async Task<IActionResult> GetGroupAsync([FromRoute] Guid customerUid, [FromRoute] Guid groupUid)
            => Ok(await _mediator.Send(new GetGroupQuery(customerUid: customerUid, groupUid: groupUid)));

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateGroupAsync([FromRoute] Guid customerUid, [FromBody] CreateGroupRequest request)
            => Ok(await _mediator.Send(new AddGroupCommand(customerUid: customerUid, name: request.Name, description: request.Description)));
    }
}
