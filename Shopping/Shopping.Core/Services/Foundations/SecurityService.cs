// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using Shopping.Core.Brokers.Tokens;
using Shopping.Core.Models.Users;

namespace Shopping.Core.Services.Foundations
{
    public class SecurityService
    {
        private readonly TokenBroker tokenBroker;

        public SecurityService(TokenBroker tokenBroker) =>
            this.tokenBroker = tokenBroker;

        public string CreateToken(User user) =>
            tokenBroker.GenerateJWT(user);
    }
}