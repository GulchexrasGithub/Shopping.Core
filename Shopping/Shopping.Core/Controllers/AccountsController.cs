// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Shopping.Core.Services.Orchestrations;
using Shopping.Core.Services.Orchestrations.OrchestrationModels;

namespace Shopping.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : RESTFulController
    {
        private readonly IUserSecurityOrchestrationService userSecurityOrchestrationService;
        public AccountsController(IUserSecurityOrchestrationService userSecurityOrchestrationService) =>
            this.userSecurityOrchestrationService = userSecurityOrchestrationService;

        [HttpGet("login")]
        public async ValueTask<ActionResult<UserToken>> Login(string email, string password)
        {
            try
            {
                var user = userSecurityOrchestrationService.LoginUser(email, password);
                return Ok(user);
            }
            catch (InvalidCredentialException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}