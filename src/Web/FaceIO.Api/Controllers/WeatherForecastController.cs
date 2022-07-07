namespace FaceIO.Api.Controllers
{
    using FaceIO.Commands.Customer;
    using FaceIO.Queries.Features.Location;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WeatherForecastController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> GetAsync()
        {
            var results = await _mediator.Send(new GetAllLocationsQuery());

            var customer = await _mediator.Send(new AddCustomerCommand("FaceIO Second customer"));

            return Ok(results);
        }
    }
}