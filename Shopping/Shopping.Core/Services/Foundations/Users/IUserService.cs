// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using Shopping.Core.Models.Users;

namespace Shopping.Core.Services.Foundations.Users
{
    public interface IUserService
    {
        ValueTask<User> AddUserAsync(User user);
        IQueryable<User> RetrieveAllUsers();
    }
}