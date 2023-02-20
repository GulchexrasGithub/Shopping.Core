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
        public async Task ShouldRemoveProductByIdAsync()
        {
            // given
            Guid randomProductId = Guid.NewGuid();
            Guid inputProductId = randomProductId;
            Product randomProduct = CreateRandomProduct();
            Product storageProduct = randomProduct;
            Product expectedInputProduct = storageProduct;
            Product deletedProduct = expectedInputProduct;
            Product expectedProduct = deletedProduct.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProductByIdAsync(inputProductId))
                .ReturnsAsync(storageProduct);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteProductAsync(expectedInputProduct))
                    .ReturnsAsync(deletedProduct);

            // when
            Product actualProduct = await this.productService
                .RemoveProductByIdAsync(inputProductId);

            // then
            actualProduct.Should().BeEquivalentTo(expectedProduct);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProductByIdAsync(inputProductId), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteProductAsync(expectedInputProduct), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}