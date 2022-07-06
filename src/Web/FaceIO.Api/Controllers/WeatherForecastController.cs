namespace FaceIO.Api.Controllers
{
    using FaceIO.Contracts.Common.Database.Context;
    using FaceIO.Domain.Common.Location.Entities;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IFaceIODbContext _dbContext;

        public WeatherForecastController(IFaceIODbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> GetAsync()
        {
            var locations = await _dbContext.Set<Location>().ToListAsync();

            return Ok(locations);
        }
    }
}