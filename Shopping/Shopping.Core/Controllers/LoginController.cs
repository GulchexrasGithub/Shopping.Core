// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Shopping.Core.Authorization;
using Shopping.Core.Models.Users;
using Shopping.Core.Services.Foundations.Users;

namespace Shopping.Core.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LoginController : RESTFulController
    {
        private IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(UserLogin userLogin)
        {
            var response = _userService.Login(userLogin);
            return Ok(response);
        }
    }
}