// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Shopping.Core.Authorization;
using Shopping.Core.Brokers.Storages;
using Shopping.Core.Models.Users;

namespace Shopping.Core.Services.Foundations.Users
{
    public interface IUserService
    {
        public string Login(UserLogin userLogin);
        User GetByEmail(string email);
    }

    public class UserService : IUserService
    {
        private StorageBroker storageBroker;
        private IJwtUtils _jwtUtils;

        public UserService(
            StorageBroker storageBroker,
            IJwtUtils jwtUtils)
        {
            storageBroker = storageBroker;
            _jwtUtils = jwtUtils;
        }


        public string Login(UserLogin userLogin)
        {
            var user = storageBroker.Users.SingleOrDefault(x => x.Email == userLogin.Email && x.Password == userLogin.Password);

            var jwtToken = _jwtUtils.GenerateJwtToken(user);

            return jwtToken;
        }

        public User GetByEmail(string email)
        {
            var user = storageBroker.Users.FirstOrDefault(u => u.Email.Equals(email));
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }
    }
}