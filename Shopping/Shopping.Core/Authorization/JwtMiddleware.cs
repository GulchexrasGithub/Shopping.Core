// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Shopping.Core.Services.Foundations.Users;

namespace Shopping.Core.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var email = jwtUtils.ValidateJwtToken(token);
            if (email != null)
            {
                context.Items["User"] = userService.GetByEmail(email);
            }

            await _next(context);
        }
    }
}