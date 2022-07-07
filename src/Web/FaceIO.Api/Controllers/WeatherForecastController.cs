namespace FaceIO.Api.Controllers
{
    using FaceIO.Contracts.Common.Database.Context;
    using FaceIO.Domain.Customer.Entities;
    using FaceIO.Domain.Location.Entities;
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
            var customer = await _dbContext.Set<Customer>().Include(x => x.Locations).SingleOrDefaultAsync();

            var locations = await _dbContext.Set<Location>().ToListAsync();

            return Ok(locations);
        }
    }
}