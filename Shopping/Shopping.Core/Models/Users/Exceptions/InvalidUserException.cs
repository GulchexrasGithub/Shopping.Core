// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using Xeptions;

namespace Shopping.Core.Models.Users.Exceptions
{
    public class InvalidUserException : Xeption
    {
        public InvalidUserException()
          : base(message: "User is invalid.")
        { }
    }
}