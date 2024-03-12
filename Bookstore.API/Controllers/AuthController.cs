using Application.Auth.Model.Request;
using Application.Auth.Service;
using Application.User.Model.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] CreateUserRequestModel request)
        {

            if (ModelState.IsValid)
            {
                var response = await _authService.Register(request);
                return StatusCode(response.StatusCode, response);
            }
            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel request)
        {

            if (ModelState.IsValid)
            {
                var response = await _authService.Login(request);
                return StatusCode(response.StatusCode, response);
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize]
        public IActionResult RefreshToken([FromBody] RefreshRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = _authService.RefreshToken(request);
                return StatusCode(response.StatusCode, response);
            }
            return BadRequest();
        }
    }
}
