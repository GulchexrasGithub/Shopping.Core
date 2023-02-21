// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using Shopping.Core.Models.Users;

namespace Shopping.Core.Services.Processings
{
    public interface IUserProcessingService
    {
        User RetriveUserByCredentials(string email, string password);
    }
}