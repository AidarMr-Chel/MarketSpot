using MarketSpot.Application.DTOs;
using MarketSpot.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace MarketSpot.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;  

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;  
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            _logger.LogInformation("Попытка регистрации пользователя с email: {Email}", request.Email);

            var result = await _authService.RegisterAsync(request);
            if (result.Succeeded)
            {
                _logger.LogInformation("Пользователь успешно зарегистрирован: {Email}", request.Email);
                await _authService.SignInAsync(request.Email, request.Password);
                return Ok(new { message = "User registered and logged in successfully" });
            }

            _logger.LogWarning("Ошибка регистрации пользователя {Email}: {Errors}", request.Email, result.Errors);
            return BadRequest(result.Errors);
        }

        [HttpPost("register-author")]
        public async Task<IActionResult> RegisterAuthor([FromBody] AuthorRegisterRequest request)
        {
            _logger.LogInformation("Попытка регистрации автора с email: {Email}", request.Email);

            var result = await _authService.RegisterAuthorAsync(request);
            if (result.Succeeded)
            {
                _logger.LogInformation("Автор успешно зарегистрирован: {Email}", request.Email);
                await _authService.SignInAsync(request.Email, request.Password);
                return Ok(new { message = "Author registered and logged in successfully" });
            }

            _logger.LogWarning("Ошибка регистрации автора {Email}: {Errors}", request.Email, result.Errors);
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("Попытка входа пользователя с email: {Email}", request.Email);

            await _authService.SignOutAsync(); 
            await _authService.SignInAsync(request.Email, request.Password);

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                _logger.LogInformation("Пользователь {Email} успешно вошел", request.Email);
                return Ok(new { message = "Login successful" });
            }
            else
            {
                _logger.LogWarning("Неудачная попытка входа для пользователя {Email}", request.Email);
                return Unauthorized(new { message = "Invalid login attempt" });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("Пользователь вышел из системы");
            await _authService.SignOutAsync();
            return Ok(new { message = "Logged out successfully" });
        }
    }
}
