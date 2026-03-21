using Microsoft.AspNetCore.Mvc;
using UserService.Application.User.Commands;
using MediatR;

namespace UserService.API
{
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [EndpointName("GetWeatherForecast")]
        [Route("/weatherforecast")]
        //[ActionName("GetWeatherForecast")]
        public ActionResult GetWeatherForecast()
        {
            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };
            var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
            .ToArray();
            return Ok(forecast);
        }

        record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
        {
            public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        }

        [HttpPost]
        [EndpointName("RegisterUser")]
        [Route("/register")]
        public async Task<ActionResult> Register(RegisterUserCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost]
        [EndpointName("")]
        [Route("/")]
        public async Task<ActionResult> Register(RegisterUserCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}
