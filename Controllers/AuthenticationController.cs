using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ServerAuth.Database;
using ServerAuth.Models;
using ServerAuth.Request;
using ServerAuth.Response;
using ServerAuth.Services;
using System;

namespace ServerAuth.Controllers
{

    [AllowAnonymous]
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private AuthenticationService _authenticationService;
        private UserService _userService;
        public AuthenticationController(UserService userService, AuthenticationService authenticationService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
        }
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Authentication(UserAuth userAuth)
        {
            // login
            try
            {
                if (await _userService.IsValidCredintialsAsync(userAuth.Email, userAuth.Password))
                {
                    var token = await _authenticationService.AuthenticateAsync(userAuth.Email, userAuth.Password);
                    return Ok(token);
                };
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return BadRequest("There is error");

        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Registeration(RegisterRequest request)
        {
            // add the user to db
            var user = new User()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password
            };

            var token = await _userService.AddAsync(user);

            return Ok(token);
        }

    }
}
