// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Shopping.Core.Models.Products;
using Shopping.Core.Models.Products.Exceptions;
using Xunit;

namespace Shopping.Core.Tests.Unit.Services.Foundations.Products
{
    public partial class ProductServiceTests
    {
        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveByIdIfSqlErrorOccursAndLogItAsync()
        {
            //given
            Guid someProductId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var failedProductStorageException =
                new FailedProductStorageException(sqlException);

            var expectedProductDependencyException =
                new ProductDependencyException(failedProductStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProductByIdAsync(someProductId))
                    .ThrowsAsync(sqlException);

            //when
            ValueTask<Product> retrieveProductByIdTask =
                this.productService.RetrieveProductByIdAsync(someProductId);

            ProductDependencyException actualProductDependencyException =
                await Assert.ThrowsAsync<ProductDependencyException>(
                    retrieveProductByIdTask.AsTask);

            //then
            actualProductDependencyException.Should().BeEquivalentTo(
                   expectedProductDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProductByIdAsync((It.IsAny<Guid>())),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProductDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnRetrieveByIdIfServiceErrorOccursAndLogItAsync()
        {
            //given
            Guid someProductId = Guid.NewGuid();
            var serviceException = new Exception();

            var failedProductServiceException =
                new FailedProductServiceException(serviceException);

            var expectedProductServiceException =
                new ProductServiceException(failedProductServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProductByIdAsync(someProductId))
                    .ThrowsAsync(serviceException);

            //when
            ValueTask<Product> retrieveProductByIdTask =
                this.productService.RetrieveProductByIdAsync(someProductId);

            ProductServiceException actualProductServiceException =
                 await Assert.ThrowsAsync<ProductServiceException>(
                     retrieveProductByIdTask.AsTask);

            //then
            actualProductServiceException.Should().BeEquivalentTo(
                expectedProductServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProductByIdAsync((It.IsAny<Guid>())),
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