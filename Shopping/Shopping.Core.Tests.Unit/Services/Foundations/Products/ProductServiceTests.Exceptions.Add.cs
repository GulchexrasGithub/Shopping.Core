// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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
        public async Task ShouldThrowCriticalDependencyExceptionOnAddIfSqlErrorOccursAndLogItAsync()
        {
            // given
            Product someProduct = CreateRandomProduct();
            SqlException sqlException = GetSqlException();

            var failedProductStorageException =
                new FailedProductStorageException(sqlException);

            var expectedProductDependencyException =
                new ProductDependencyException(failedProductStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProductAsync(It.IsAny<Product>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Product> addProductTask =
                this.productService.AddProductAsync(someProduct);

            // then
            await Assert.ThrowsAsync<ProductDependencyException>(() =>
                addProductTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProductAsync(It.IsAny<Product>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(
                    expectedProductDependencyException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnAddIfProductAlreadyExistsAndLogItAsync()
        {
            // given
            Product someProduct = CreateRandomProduct();
            string randomMessage = GetRandomMessage();

            var duplicateKeyException =
            new DuplicateKeyException(randomMessage);

            var alreadyExistsProductException =
                new AlreadyExistsProductException(duplicateKeyException);

            var expectedProductDependencyValidationException =
                new ProductDependencyValidationException(alreadyExistsProductException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProductAsync(It.IsAny<Product>()))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Product> addProductTask =
                this.productService.AddProductAsync(someProduct);

            ProductDependencyValidationException actualProductDependencyValidationException =
                await Assert.ThrowsAsync<ProductDependencyValidationException>(
                    addProductTask.AsTask);

            // then
            actualProductDependencyValidationException.Should().BeEquivalentTo(
                expectedProductDependencyValidationException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProductAsync(someProduct),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProductDependencyValidationException))),
                        Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddIfDatabaseUpdateErrorOccursAndLogItAsync()
        {
            //given
            Product someProduct = CreateRandomProduct();

            var databaseUpdateException =
                new DbUpdateException();
            var failedProductStorageException =
                new FailedProductStorageException(databaseUpdateException);
            var expectedProductDependencyException =
                new ProductDependencyException(failedProductStorageException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProductAsync(someProduct)).Throws(databaseUpdateException);

            //when
            ValueTask<Product> addProductTask =
                this.productService.AddProductAsync(someProduct);
            ProductDependencyException actualProductDependencyException
                = await Assert.ThrowsAsync<ProductDependencyException>(
                addProductTask.AsTask);

            //then
            actualProductDependencyException.Should()
                .BeEquivalentTo(expectedProductDependencyException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProductAsync
                    (It.IsAny<Product>()), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProductDependencyException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddIfServiceErrorOccursAndLogItAsync()
        {
            //given
            Product someProduct = CreateRandomProduct();
            var serviceException = new Exception();

            var failedProductServiceException =
                new FailedProductServiceException(serviceException);

            var expectedProductServiceException =
                new ProductServiceException(failedProductServiceException);

            this.storageBrokerMock.Setup(broker =>
               broker.InsertProductAsync(someProduct))
                   .Throws(serviceException);

            //when
            ValueTask<Product> addProductTask =
                this.productService.AddProductAsync(someProduct);

            ProductServiceException actualProductServiceException =
                await Assert.ThrowsAsync<ProductServiceException>(
                    addProductTask.AsTask);

            //then
            actualProductServiceException.Should().BeEquivalentTo(
                expectedProductServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProductAsync(It.IsAny<Product>()),
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