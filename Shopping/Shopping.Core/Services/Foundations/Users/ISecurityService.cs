// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using Shopping.Core.Models.Users;

namespace Shopping.Core.Services.Foundations.Users
{
    public interface ISecurityService
    {
        string CreateToken(User user);
    }
}
