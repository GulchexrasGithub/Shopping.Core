// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System.Linq;
using Shopping.Core.Models.Users;
using Shopping.Core.Services.Foundations.Users;

namespace Shopping.Core.Services.Processings
{
    public class UserProcessingService : IUserProcessingService
    {
        private readonly IUserService userService;

        public UserProcessingService(IUserService userService) =>
            this.userService = userService;

        public User RetriveUserByCredentials(string email, string password)
        {
            IQueryable<User> allUser = this.userService.RetrieveAllUsers();

            return allUser
                        .FirstOrDefault(retrievedUser =>
                            retrievedUser.Email == email && retrievedUser.Password == password);
        }
    }
}
