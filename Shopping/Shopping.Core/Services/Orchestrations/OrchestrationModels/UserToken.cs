// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System;

namespace Shopping.Core.Services.Orchestrations.OrchestrationModels
{
    public class UserToken
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}