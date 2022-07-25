namespace FaceIO.Api.Controllers
{
    using FaceIO.Commands.PersonImage;
    using FaceIO.Contracts.Common;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/customers/{customerUid:guid}/persons/{personUid:guid}")]
    public class PersonImagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PersonImagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("image")]
        public async Task<IActionResult> UploadPersonImageAsync([FromRoute] Guid customerUid, [FromRoute] Guid personUid, IFormFile image)
        {
            var imageRequest = new UploadImageRequest
            {
                Name = image.Name,
                FileName = image.FileName,
                ContentType = image.ContentType,
                FileStream = image.OpenReadStream()
            };

            return Ok(await _mediator.Send(new AddPersonImageCommand(customerUid: customerUid, personUid: personUid, imageRequest: imageRequest)));
        }
    }
}
