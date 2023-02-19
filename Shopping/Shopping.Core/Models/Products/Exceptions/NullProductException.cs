// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using Xeptions;

namespace Shopping.Core.Models.Products.Exceptions
{
    public class NullProductException : Xeption
    {
        public NullProductException()
            : base(message: "Product is null. ")
        { }
    }
}