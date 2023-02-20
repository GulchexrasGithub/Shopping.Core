// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using Shopping.Core.Models.Users;
using Shopping.Core.Services.Foundations;

namespace Shopping.Core.Services.Processings
{
    public class UserSecurityService
    {
        private readonly SecurityService securityService;

        public UserSecurityService(SecurityService securityService) =>
            this.securityService = securityService;
        public string CreateToken(User user) =>
            securityService.CreateToken(user);
    }
}