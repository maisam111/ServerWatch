using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ServerAuth.Database;
using ServerAuth.Models;
using ServerAuth.Request;
using ServerAuth.Response;
using ServerAuth.Services;
using System;


public class AuthenticationError: Exception {
    public AuthenticationError() {}

    public AuthenticationError(string message)
        : base(message) { }
    
    public AuthenticationError(string message, Exception error)
        : base(message, error) { }
    
}


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
                var validCredentials = await _userService.IsValidCredintialsAsync(userAuth.Email, userAuth.Password);
                if (validCredentials is not true)
                    return BadRequest("Incorrenct email or password");
                
                return Ok(await _authenticationService.AuthenticateAsync(
                    userAuth.Email,
                    userAuth.Password));
                
            }
            catch (Exception e) // TODO replace Exception
            {
                return BadRequest(e.Message);
            }

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
