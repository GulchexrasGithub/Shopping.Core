// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

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
        public async Task ShouldThrowValidationExceptionOnModifyIfProductIsNullAndLogItAsync()
        {
            // given
            Product nullProduct = null;
            var nullProductException = new NullProductException();

            var expectedProductValidationException =
                new ProductValidationException(nullProductException);

            // when
            ValueTask<Product> modifyProductTask =
                this.productService.ModifyProductAsync(nullProduct);

            ProductValidationException actualProductValidationException =
                await Assert.ThrowsAsync<ProductValidationException>(
                    modifyProductTask.AsTask);

            // then
            actualProductValidationException.Should().BeEquivalentTo(expectedProductValidationException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProductValidationException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProductAsync(It.IsAny<Product>()), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        /* [Fact]
         public async Task ShouldThrowValidationExceptionOnModifyIfProductIsInvalidAndLogItAsync()
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
                 values: "Title is required");

             invalidProductException.AddData(
                key: nameof(Product.Quantity),
                values: "Quantity is required");

             invalidProductException.AddData(
                key: nameof(Product.Price),
                values: "Price is required");

             var expectedProductValidationException =
                 new ProductValidationException(invalidProductException);

             // when
             ValueTask<Product> modifyProductTask =
                 this.productService.ModifyProductAsync(invalidProduct);

             ProductValidationException actualProductValidationException =
                 await Assert.ThrowsAsync<ProductValidationException>(
                     modifyProductTask.AsTask);

             //then
             actualProductValidationException.Should()
                 .BeEquivalentTo(expectedProductValidationException);

             this.loggingBrokerMock.Verify(broker =>
                 broker.LogError(It.Is(SameExceptionAs(
                     expectedProductValidationException))), Times.Once);

             this.storageBrokerMock.Verify(broker =>
                 broker.UpdateProductAsync(It.IsAny<Product>()), Times.Never);

             this.loggingBrokerMock.VerifyNoOtherCalls();
             this.storageBrokerMock.VerifyNoOtherCalls();
         }*/
    }
}