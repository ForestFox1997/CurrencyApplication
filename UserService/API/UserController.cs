using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MediatR;
using UserService.Application.User.Commands;

namespace UserService.API
{
    [ApiController]
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
        [Route("/register")]
        public async Task<ActionResult> Register(RegisterUserCommand command)
        {
            await _mediator.Send(command);
            //await Task.Delay(TimeSpan.FromMinutes(2));
            return Ok();
        }

        [HttpPost]
        [Route("/login")]
        public async Task<ActionResult> Login(LoginUserCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.Success)
            {
                return Unauthorized(new { result.Error });
            }

            return Ok(new { Result = result.Message });
        }

        [HttpGet]
        [Route("/test")]
        [Authorize]
        public async Task<IActionResult> TestAuthorize()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok("Authorization test passed.");
        }
    }
}
