namespace FaceIO.Api.Controllers
{
    using FaceIO.Commands.Person;
    using FaceIO.Contracts.Common;
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

        [HttpGet]
        [Route("{personUid:guid}")]
        public async Task<IActionResult> GetPersonAsync([FromRoute] Guid customerUid, [FromRoute] Guid personUid)
            => Ok(await _mediator.Send(new GetPersonQuery(customerUid: customerUid, personUid: personUid)));

        [HttpPost]
        [Route("{personUid:guid}")]
        public async Task<IActionResult> AddPersonFaceAsync([FromRoute] Guid customerUid, [FromRoute] Guid personUid, IFormFile image)
            => Ok(await _mediator.Send(new AddPersonFaceCommand(
                customerUid: customerUid,
                personUid: personUid,
                new UploadImageRequest { Name = image.Name, FileName = image.FileName, ContentType = image.ContentType, FileStream = image.OpenReadStream() })));

        [HttpPatch]
        [Route("{personUid:guid}")]
        public async Task<IActionResult> UpdatePersonAsync([FromRoute] Guid customerUid, [FromRoute] Guid personUid, [FromBody] UpdatePersonRequest request)
            => Ok(await _mediator.Send(new UpdatePersonCommand(customerUid: customerUid, personUid: personUid, name: request.Name)));

        [HttpDelete]
        [Route("{personUid:guid}")]
        public async Task<IActionResult> DeletePersonAsync([FromRoute] Guid customerUid, [FromRoute] Guid personUid)
            => Ok(await _mediator.Send(new RemovePersonCommand(customerUid: customerUid, personUid: personUid)));
    }
}