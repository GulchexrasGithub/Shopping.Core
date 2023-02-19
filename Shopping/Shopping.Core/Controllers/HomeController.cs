// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;

namespace Shopping.Core.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : RESTFulController
    {
        [HttpGet]
        public ActionResult<string> GetHomeMessage() => Ok("Shopping is running...");
    }
}