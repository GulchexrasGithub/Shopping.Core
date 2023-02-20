// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using Xeptions;

namespace Shopping.Core.Models.Users.Exceptions
{
    public class UserValidationException : Xeption
    {
        public UserValidationException(Xeption innerExeption)
            : base(message: "User validation error occured, fix the errors and try again.", innerExeption)
        { }
    }
}