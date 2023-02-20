// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using Shopping.Core.Models.Products;
using Xunit;

namespace Shopping.Core.Tests.Unit.Services.Foundations.Products
{
    public partial class ProductServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveProductByIdAsync()
        {
            // given
            Guid randomProductId = Guid.NewGuid();
            Guid inputProductId = randomProductId;
            Product randomProduct = CreateRandomProduct();
            Product storageProduct = randomProduct;
            Product expectedProduct = storageProduct.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProductByIdAsync(inputProductId))
                .ReturnsAsync(storageProduct);

            // when
            Product actualProduct =
                await this.productService.RetrieveProductByIdAsync(
                    inputProductId);

            // then
            actualProduct.Should().BeEquivalentTo(expectedProduct);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProductByIdAsync(inputProductId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}