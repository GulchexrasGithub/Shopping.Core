// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------



using System;
using FluentAssertions;
using Moq;
using Shopping.Core.Models.Products.Exceptions;
using Xunit;

namespace Shopping.Core.Tests.Unit.Services.Foundations.Products
{
    public partial class ProductServiceTests
    {
        [Fact]
        public void ShouldThrowServiceExceptionOnRetrieveAllWhenAllServiceErrorOccursAndLogIt()
        {
            //given
            string exceptionMessage = GetRandomMessage();
            var serviceException = new Exception(exceptionMessage);

            var failedProductServiceException =
                new FailedProductServiceException(serviceException);

            var expectedProductServiceException =
                new ProductServiceException(failedProductServiceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllProducts()).Throws(serviceException);

            //when
            Action retrieveAllProductsAction = () =>
                this.productService.RetrieveAllProducts();

            ProductServiceException actualProductServiceException =
                Assert.Throws<ProductServiceException>(retrieveAllProductsAction);

            //then
            actualProductServiceException.Should().
                BeEquivalentTo(expectedProductServiceException);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProducts(), Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedProductServiceException))), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}