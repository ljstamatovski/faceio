namespace FaceIO.Api.Controllers
{
    using FaceIO.Commands.Person;
    using FaceIO.Contracts.Person;
    using FaceIO.Queries.Features.Person;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/customers/{customerUid:guid}/persons")]
    public class PersonsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PersonsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetPersonsAsync([FromRoute] Guid customerUid)
            => Ok(await _mediator.Send(new GetPersonsQuery(customerUid: customerUid)));

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreatePersonAsync([FromRoute] Guid customerUid, [FromBody] CreatePersonRequest request)
            => Ok(await _mediator.Send(new AddPersonCommand(customerUid: customerUid, name: request.Name)));

        [HttpDelete]
        [Route("{personUid:guid}")]
        public async Task<IActionResult> DeletePersonAsync([FromRoute] Guid customerUid, [FromRoute] Guid personUid)
            => Ok(await _mediator.Send(new RemovePersonCommand(customerUid: customerUid, personUid: personUid)));
    }
}