// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace Shopping.Core.Models.Products.Exceptions
{
    public class LockedProductException : Xeption
    {
        public LockedProductException(Exception innerException)
           : base(message: "Product is locked, please try again.", innerException)
        { }
    }
}