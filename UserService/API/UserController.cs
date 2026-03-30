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

        /// <summary>
        /// Зарегистрировать нового пользователя
        /// </summary>
        /// <remarks>
        /// Для регистрации пользователя необходимы ник (6 - 20 символов, допустимы буквы, цифры, и символ '_')
        /// и пароль (10 - 20 символов, допустимы буквы, цифры, и набор спецсимволов)
        /// </remarks>
        [HttpPost]
        [Route("/register")]
        public async Task<ActionResult> Register(RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Message);
        }

        /// <summary>
        /// Пройти аутентификацию
        /// </summary>
        /// <remarks>
        /// Для успешного логина необходимы логин и пароль пользователя, результат
        /// успешной аутентификации - JWT Bearer токен со сроком жизни 4 часа
        /// </remarks>
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

        /// <summary>
        /// Отобразить текущего пользователя
        /// </summary>
        [HttpGet]
        [Route("/whoami")]
        [Authorize]
        public async Task<ActionResult> GetUserLogin()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok($"Вы вошли, как {userName}");
        }
    }
}
