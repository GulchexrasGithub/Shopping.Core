// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using Shopping.Core.Services.Orchestrations.OrchestrationModels;

namespace Shopping.Core.Services.Orchestrations
{
    public interface IUserSecurityOrchestrationService
    {
        public UserToken LoginUser(string email, string password);
    }
}