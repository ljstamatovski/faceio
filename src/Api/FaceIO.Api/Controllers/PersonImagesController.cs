namespace FaceIO.Api.Controllers
{
    using FaceIO.Commands.PersonImage;
    using FaceIO.Contracts.Common;
    using FaceIO.Queries.Features.PersonImage;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/customers/{customerUid:guid}/persons/{personUid:guid}/images")]
    public class PersonImagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PersonImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetPersonImagesAsync([FromRoute] Guid customerUid, [FromRoute] Guid personUid)
            => Ok(await _mediator.Send(new GetPersonImagesQuery(customerUid: customerUid, personUid: personUid)));

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> UploadPersonImageAsync([FromRoute] Guid customerUid, [FromRoute] Guid personUid, IFormFile image)
            => Ok(await _mediator.Send(new AddPersonImageCommand(
                customerUid: customerUid,
                personUid: personUid,
                imageRequest: new UploadImageRequest { Name = image.Name, FileName = image.FileName, ContentType = image.ContentType, FileStream = image.OpenReadStream() })));

        [HttpDelete]
        [Route("{personImageUid:guid}")]
        public async Task<IActionResult> DeletePersonImageAsync([FromRoute] Guid customerUid, [FromRoute] Guid personUid, [FromRoute] Guid personImageUid)
            => Ok(await _mediator.Send(new RemovePersonImageCommand(customerUid: customerUid, personUid: personUid, personImageUid: personImageUid)));
    }
}
