// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System.Security.Authentication;
using Shopping.Core.Models.Users;
using Shopping.Core.Services.Orchestrations.OrchestrationModels;
using Shopping.Core.Services.Processings;

namespace Shopping.Core.Services.Orchestrations
{
    public class UserSecurityOrchestrationService : IUserSecurityOrchestrationService
    {
        private readonly UserProcessingService userProcessingService;
        private readonly UserSecurityService userSecurityService;

        public UserSecurityOrchestrationService(
            UserProcessingService userProcessingService,
            UserSecurityService userSecurityService)
        {
            this.userProcessingService =
                userProcessingService;

            this.userSecurityService =
                userSecurityService;
        }
        public UserToken LoginUser(string email, string password)
        {
            User user =
                userProcessingService.RetriveUserByCredentials(email, password);

            if (user is null)
                throw new InvalidCredentialException("Email or password incorrect. Please try again.");

            UserToken userToken = new UserToken();
            userToken.UserId = user.Id;
            userToken.Token = userSecurityService.CreateToken(user);

            return userToken;
        }
    }
}