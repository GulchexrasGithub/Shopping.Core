// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Core.Brokers.Tokens
{
    public class JWTOptionsModel
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpirationInMinutes { get; set; }
        [NotMapped]
        public string SecretKey { get => Environment.GetEnvironmentVariable("SECRET_KEY"); }
    }
}