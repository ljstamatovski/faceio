namespace FaceIO.Api.Controllers
{
    using FaceIO.Commands.Face;
    using FaceIO.Contracts.Common;
    using FaceIO.Queries.Features.Face;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/customers/{customerUid:guid}/persons/{personUid:guid}/faces")]
    public class FacesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FacesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetPersonFacesAsync([FromRoute] Guid customerUid, [FromRoute] Guid personUid)
            => Ok(await _mediator.Send(new GetPersonFacesQuery(customerUid: customerUid, personUid: personUid)));

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> UploadPersonFaceAsync([FromRoute] Guid customerUid, [FromRoute] Guid personUid, IFormFile image)
            => Ok(await _mediator.Send(new AddFaceCommand(
                customerUid: customerUid,
                personUid: personUid,
                imageRequest: new UploadImageRequest { Name = image.Name, FileName = image.FileName, ContentType = image.ContentType, FileStream = image.OpenReadStream() })));

        [HttpDelete]
        [Route("{faceUid:guid}")]
        public async Task<IActionResult> DeletePersonFaceAsync([FromRoute] Guid customerUid, [FromRoute] Guid personUid, [FromRoute] Guid faceUid)
            => Ok(await _mediator.Send(new RemoveFaceCommand(customerUid: customerUid, personUid: personUid, faceUid: faceUid)));
    }
}
