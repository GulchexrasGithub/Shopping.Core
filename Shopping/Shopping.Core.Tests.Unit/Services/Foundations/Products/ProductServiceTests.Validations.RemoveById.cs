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
        public async Task ShouldThrowValidationExceptionOnRemoveIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidProductId = Guid.Empty;

            var invalidProductException = new InvalidProductException();

            invalidProductException.AddData(
            key: nameof(Product.Id),
                values: "Id is required");

            var expectedProductValidationException =
                new ProductValidationException(invalidProductException);

            // when
            ValueTask<Product> removeProductByIdTask =
                this.productService.RemoveProductByIdAsync(invalidProductId);

            ProductValidationException actualProductValidationException =
                await Assert.ThrowsAsync<ProductValidationException>(
                    removeProductByIdTask.AsTask);

            // then
            actualProductValidationException.Should()
                .BeEquivalentTo(expectedProductValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProductValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProductAsync(It.IsAny<Product>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowNotFoundExceptionOnRemoveIfGroupPostIsNotFoundAndLogItAsync()
        {
            //given
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Product randomProduct = CreateRandomProduct(randomDateTime);
            Guid inputProductId = randomProduct.Id;
            Product nullStorageProduct = null;

            var notFoundProductException =
                new NotFoundProductException(inputProductId);

            var expectedProductValidationException =
                new ProductValidationException(notFoundProductException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProductByIdAsync(inputProductId))
                    .ReturnsAsync(nullStorageProduct);

            //when
            ValueTask<Product> removeProductTask =
                this.productService.RemoveProductByIdAsync(inputProductId);

            ProductValidationException actualProductValidationException =
                await Assert.ThrowsAsync<ProductValidationException>(
                    removeProductTask.AsTask);

            //then
            actualProductValidationException.Should().BeEquivalentTo(
                expectedProductValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProductByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProductValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProductAsync(It.IsAny<Product>()), Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}