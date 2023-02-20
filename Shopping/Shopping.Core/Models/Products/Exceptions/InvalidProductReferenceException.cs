// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Shopping.Core.Models.Products.Exceptions
{
    public class InvalidProductReferenceException : Xeption
    {
        public InvalidProductReferenceException(Exception innerException)
            : base(message: "Invalid product reference error occured.", innerException)
        { }
    }
}