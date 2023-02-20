// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shopping.Core.Models.Products;
using Shopping.Core.Models.Products.Exceptions;
using Xunit;

namespace Shopping.Core.Tests.Unit.Services.Foundations.Products
{
    public partial class ProductServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnModifyIfSqlErrorOccursAndLogItAsync()
        {
            // given
            DateTimeOffset someDateTime = GetRandomDateTimeOffset();
            Product randomProduct = CreateRandomProduct(someDateTime);
            Product someProduct = randomProduct;
            Guid productId = someProduct.Id;
            SqlException sqlException = GetSqlException();

            var failedProductStorageException =
                new FailedProductStorageException(sqlException);

            var expectedProductDependencyException =
                new ProductDependencyException(failedProductStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProductByIdAsync(productId))
                    .Throws(sqlException);

            // when
            ValueTask<Product> modifyProductTask =
                this.productService.ModifyProductAsync(someProduct);

            ProductDependencyException actualProductDependencyException =
                await Assert.ThrowsAsync<ProductDependencyException>(
                    modifyProductTask.AsTask);

            // then
            actualProductDependencyException.Should().BeEquivalentTo(
                expectedProductDependencyException);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProductDependencyException))), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProductByIdAsync(productId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProductAsync(someProduct), Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDatabaseUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int minutesInPast = GetRandomNegativeNumber();
            DateTimeOffset randomDateTime = GetRandomDateTimeOffset();
            Product randomProduct = CreateRandomProduct(randomDateTime);
            Product someProduct = randomProduct;
            Guid productId = someProduct.Id;
            var databaseUpdateException = new DbUpdateException();

            var failedProductException =
                new FailedProductStorageException(databaseUpdateException);

            var expectedProductDependencyException =
                new ProductDependencyException(failedProductException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProductByIdAsync(productId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Product> modifyProductTask =
                this.productService.ModifyProductAsync(someProduct);

            ProductDependencyException actualProductDependencyException =
                await Assert.ThrowsAsync<ProductDependencyException>(
                    modifyProductTask.AsTask);

            // then
            actualProductDependencyException.Should().BeEquivalentTo(
                expectedProductDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProductByIdAsync(productId), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProductDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}