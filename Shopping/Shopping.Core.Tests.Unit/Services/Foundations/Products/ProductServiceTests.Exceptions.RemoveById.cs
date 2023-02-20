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
        public async Task ShouldThrowDependencyValidationOnRemoveIfDatabaseUpdateConcurrencyErrorOccursAndLogItAsync()
        {
            // given
            Guid productId = Guid.NewGuid();

            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();

            var lockedProductException =
                new LockedProductException(databaseUpdateConcurrencyException);

            var expectedProductDependencyValidationException =
                new ProductDependencyValidationException(lockedProductException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProductByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            // when
            ValueTask<Product> removeProductByIdTask =
                this.productService.RemoveProductByIdAsync(productId);

            ProductDependencyValidationException actualProductDependencyValidationException =
                await Assert.ThrowsAsync<ProductDependencyValidationException>(
                    removeProductByIdTask.AsTask);

            // then
            actualProductDependencyValidationException.Should().BeEquivalentTo(
                expectedProductDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProductByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProductDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProductAsync(It.IsAny<Product>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid productId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedProductStorageException =
                new FailedProductStorageException(sqlException);

            var expectedProductDependencyException =
                new ProductDependencyException(failedProductStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProductByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Product> deleteProductTask =
                this.productService.RemoveProductByIdAsync(productId);

            ProductDependencyException actualProductDependencyException =
                await Assert.ThrowsAsync<ProductDependencyException>(
                    deleteProductTask.AsTask);

            // then
            actualProductDependencyException.Should().BeEquivalentTo(
                expectedProductDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProductByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProductDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRemoveIfExceptionOccursAndLogItAsync()
        {
            // given
            Guid productId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedProductServiceException =
                new FailedProductServiceException(serviceException);

            var expectedProductServiceException =
                new ProductServiceException(failedProductServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProductByIdAsync(It.IsAny<Guid>()))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Product> removeProductByIdTask =
                this.productService.RemoveProductByIdAsync(productId);

            ProductServiceException actualProductServiceException =
                await Assert.ThrowsAsync<ProductServiceException>(
                    removeProductByIdTask.AsTask);

            // then
            actualProductServiceException.Should().BeEquivalentTo(
                expectedProductServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProductByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProductServiceException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}