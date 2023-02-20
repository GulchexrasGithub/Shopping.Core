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
        public async Task ShouldModifyProductAsync()
        {
            //given
            DateTimeOffset randomDate = GetRandomDateTimeOffset();
            Product randomProduct = CreateRandomModifyProduct(randomDate);
            Product inputProduct = randomProduct;
            Product storageProduct = inputProduct.DeepClone();
            Product updatedProduct = inputProduct;
            Product expectedProduct = updatedProduct.DeepClone();
            Guid inputProductId = inputProduct.Id;

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateProductAsync(inputProduct))
                    .ReturnsAsync(updatedProduct);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectProductByIdAsync(inputProductId))
                    .ReturnsAsync(storageProduct);

            //when
            Product actualProduct =
                await this.productService.ModifyProductAsync(inputProduct);

            //then
            actualProduct.Should().BeEquivalentTo(expectedProduct);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateProductAsync(inputProduct), Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectProductByIdAsync(inputProductId), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}