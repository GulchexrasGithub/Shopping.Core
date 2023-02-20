// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

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
        public async Task ShouldAddProductAsync()
        {
            // given
            Product randomProduct = CreateRandomProduct();
            Product inputProduct = randomProduct;
            Product storageProduct = inputProduct;
            Product expectedProduct = storageProduct.DeepClone();

            this.storageBrokerMock.Setup(broker =>
                broker.InsertProductAsync(inputProduct))
                    .ReturnsAsync(storageProduct);

            // when
            Product actualProduct =
                await this.productService.AddProductAsync(inputProduct);

            // then
            actualProduct.Should().BeEquivalentTo(expectedProduct);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertProductAsync(inputProduct),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}