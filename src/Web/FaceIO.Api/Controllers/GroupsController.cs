namespace FaceIO.Api.Controllers
{
    using FaceIO.Commands.Group;
    using FaceIO.Contracts.Group;
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

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateGroupAsync([FromRoute] Guid customerUid, [FromBody] CreateGroupRequest request)
            => Ok(await _mediator.Send(new AddGroupCommand(customerUid: customerUid, name: request.Name, description: request.Description)));
    }
}
