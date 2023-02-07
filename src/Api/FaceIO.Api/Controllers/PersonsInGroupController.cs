namespace FaceIO.Api.Controllers
{
    using FaceIO.Commands.PersonInGroup;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/customers/{customerUid:guid}/groups/{groupUid:guid}/persons/{personUid:guid}")]
    public class PersonsInGroupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PersonsInGroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddPersonInGroupAsync([FromRoute] Guid customerUid, [FromRoute] Guid groupUid, [FromRoute] Guid personUid)
            => Ok(await _mediator.Send(new AddPersonInGroupCommand(customerUid: customerUid, groupUid: groupUid, personUid: personUid)));

        [HttpDelete]
        [Route("")]
        public async Task<IActionResult> RemovePersonFromGroupAsync([FromRoute] Guid customerUid, [FromRoute] Guid groupUid, [FromRoute] Guid personUid)
            => Ok(await _mediator.Send(new RemovePersonFromGroupCommand(customerUid: customerUid, groupUid: groupUid, personUid: personUid)));
    }
}
