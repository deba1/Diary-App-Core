using Application.Managers;
using Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationManager authenticationManager;

        public AuthenticationController(IAuthenticationManager _authenticationManager)
        {
            authenticationManager = _authenticationManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthCredential credential)
        {
            UserViewModel userViewModel = await authenticationManager.ValidateCredential(credential);
            if (userViewModel != null)
            {
                return Ok(new {
                    id = userViewModel.Id,
                    username = userViewModel.Username,
                    roles = userViewModel.Roles,
                    email = userViewModel.Email,
                    token = await authenticationManager.CreateToken()
                });
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterViewModel user)
        {
            var result = await authenticationManager.RegisterUser(user);
            if (result.Succeeded)
                return NoContent();
            else
                return BadRequest(result.Errors);
        }
    }
}
