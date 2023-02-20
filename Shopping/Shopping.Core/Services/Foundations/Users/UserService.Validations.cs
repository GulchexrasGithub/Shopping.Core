// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System;
using Shopping.Core.Models.Users;
using Shopping.Core.Models.Users.Exceptions;

namespace Shopping.Core.Services.Foundations.Users
{
    public partial class UserService
    {
        private void ValidateUserOnAdd(User user)
        {
            ValidateUserNotNull(user);

            Validate(
                (Rule: IsInvalid(user.Id), Parameter: nameof(User.Id)),
                (Rule: IsInvalid(user.FirstName), Parameter: nameof(User.FirstName)),
                (Rule: IsInvalid(user.LastName), Parameter: nameof(User.LastName)),
                (Rule: IsInvalid(user.Email), Parameter: nameof(User.Email)),
                (Rule: IsInvalid(user.Password), Parameter: nameof(User.Password)));
        }

        private void ValidateUserId(Guid userId) =>
            Validate((Rule: IsInvalid(userId), Parameter: nameof(User.Id)));

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private static void ValidateUserNotNull(User user)
        {
            if (user is null)
            {
                throw new NullUserException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidUserException = new InvalidUserException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidUserException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidUserException.ThrowIfContainsErrors();
        }
    }
}