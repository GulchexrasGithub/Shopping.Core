// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shopping.Core.Models.Users;

namespace Shopping.Core.Brokers.Tokens
{
    public class TokenBroker
    {
        private readonly IConfiguration configuration;

        public TokenBroker(IConfiguration configuration) =>
            this.configuration = configuration;

        public string GenerateJWT(User user)
        {
            JWTOptionsModel jwtOptionsModel = this.configuration.GetSection("JWTOptionsModel").Get<JWTOptionsModel>();
            var securityTokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = Encoding.ASCII.GetBytes(jwtOptionsModel.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(jwtOptionsModel.ExpirationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtOptionsModel.Audience,
                Issuer = jwtOptionsModel.Issuer
            };
            var securityToken = securityTokenHandler.CreateToken(tokenDescriptor);
            return securityTokenHandler.WriteToken(securityToken);
        }
    }
}