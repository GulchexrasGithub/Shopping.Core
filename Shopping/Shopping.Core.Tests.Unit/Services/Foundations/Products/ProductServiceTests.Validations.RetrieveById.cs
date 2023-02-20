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
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidProductId = Guid.Empty;

            var invalidProductException = new InvalidProductException();

            invalidProductException.AddData(
                key: nameof(Product.Id),
                values: "Id is required");

            var expectedProductValidationException = new
                ProductValidationException(invalidProductException);

            // when
            ValueTask<Product> retrieveProductByIdTask =
                this.productService.RetrieveProductByIdAsync(invalidProductId);

            ProductValidationException actualProductValidationException =
                await Assert.ThrowsAsync<ProductValidationException>(
                    retrieveProductByIdTask.AsTask);

            // then
            actualProductValidationException.Should().BeEquivalentTo(expectedProductValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProductValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProductByIdAsync(invalidProductId),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRetrieveByIdIfProductIsNotFoundAndLogItAsync()
        {
            //given
            Guid someProductId = Guid.NewGuid();
            Product noProduct = null;

            var notFoundProductException =
                new NotFoundProductException(someProductId);

            var expectedProductValidationException =
                new ProductValidationException(notFoundProductException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProductByIdAsync(someProductId))
                    .ReturnsAsync(noProduct);

            //when
            ValueTask<Product> retrieveProductByIdTask =
                this.productService.RetrieveProductByIdAsync(someProductId);

            ProductValidationException actualProductValidationException =
                await Assert.ThrowsAsync<ProductValidationException>(
                    retrieveProductByIdTask.AsTask);

            // then
            actualProductValidationException.Should().BeEquivalentTo
                (expectedProductValidationException);

            this.storageBrokerMock.Verify(broker =>
               broker.SelectProductByIdAsync(someProductId),
                    Times.Once());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProductValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }

}