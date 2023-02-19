// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System;
using Shopping.Core.Models.Products;
using Shopping.Core.Models.Products.Exceptions;

namespace Shopping.Core.Services.Foundations.Products
{
    public partial class ProductService
    {
        private void ValidateProduct(Product product)
        {
            ValidateProductNotNull(product);

            Validate(
                (Rule: IsInvalid(product.Id), Parameter: nameof(Product.Id)),
                (Rule: IsInvalid(product.Title), Parameter: nameof(Product.Title)),
                (Rule: IsInvalid(product.Quantity), Parameter: nameof(Product.Quantity)),
                (Rule: IsInvalid(product.Price), Parameter: nameof(Product.Price)));
        }

        private void ValidateProductId(Guid productId) =>
            Validate((Rule: IsInvalid(productId), Parameter: nameof(Product.Id)));

        private void ValidateStorageProduct(Product maybeProduct, Guid productId)
        {
            if (maybeProduct is null)
            {
                throw new NotFoundProductException(productId);
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == default,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(int number) => new
        {
            Condition = number < 0,
            Message = "Quantity is required"
        };

        private static dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Title is required"
        };

        private static dynamic IsInvalid(decimal price) => new
        {
            Condition = price == default,
            Message = "Price is required"
        };

        private static void ValidateProductNotNull(Product product)
        {
            if (product is null)
            {
                throw new NullProductException();
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidProductException = new InvalidProductException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidProductException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidProductException.ThrowIfContainsErrors();
        }
    }
}