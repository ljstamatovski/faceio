namespace FaceIO.Api.Controllers
{
    using FaceIO.Commands.Customer;
    using FaceIO.Contracts.Customer;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] CreateCustomerRequest request)
            => Ok(await _mediator.Send(new AddCustomerCommand(request.Name)));
    }
}