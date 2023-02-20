// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Shopping.Core.Models.Products;
using Shopping.Core.Models.Products.Exceptions;
using Xunit;

namespace Shopping.Core.Tests.Unit.Services.Foundations.Products
{
    public partial class ProductServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfProductIsNullAndLogItAsync()
        {
            // given
            Product nullProduct = null;

            var nullProductException =
                new NullProductException();

            var expecteProductValidationException =
                new ProductValidationException(nullProductException);

            // when
            ValueTask<Product> addProductTask =
                this.productService.AddProductAsync(nullProduct);

            ProductValidationException actualProductValidationException =
                await Assert.ThrowsAsync<ProductValidationException>(
                    addProductTask.AsTask);

            // then
            actualProductValidationException.Should().BeEquivalentTo(
                expecteProductValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expecteProductValidationException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnAddIfProductIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidGuid = Guid.Empty;

            var invalidProduct = new Product
            {
                Id = invalidGuid
            };

            var invalidProductException =
                new InvalidProductException();

            invalidProductException.AddData(
                key: nameof(Product.Id),
                values: "Id is required");

            invalidProductException.AddData(
                key: nameof(Product.Title),
                values: "Text is required");

            invalidProductException.AddData(
               key: nameof(Product.Price),
               values: "Price is required");

            invalidProductException.AddData(
               key: nameof(Product.Quantity),
               values: "Quantity is required");

            var expectedProductValidationException =
                new ProductValidationException(invalidProductException);

            // when
            ValueTask<Product> addProductTask =
                this.productService.AddProductAsync(invalidProduct);

            ProductValidationException actualProductValidationException =
                await Assert.ThrowsAsync<ProductValidationException>(
                    addProductTask.AsTask);

            // then
            actualProductValidationException.Should().BeEquivalentTo(
                expectedProductValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProductAsync(invalidProduct),
                    Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProductValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}