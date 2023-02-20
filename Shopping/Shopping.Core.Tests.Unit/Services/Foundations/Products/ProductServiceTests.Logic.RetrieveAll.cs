// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using Shopping.Core.Models.Products;
using Xunit;

namespace Shopping.Core.Tests.Unit.Services.Foundations.Products
{
    public partial class ProductServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllProducts()
        {
            //given
            IQueryable<Product> randomProducts = CreateRandomProducts();
            IQueryable<Product> storageProducts = randomProducts;
            IQueryable<Product> expectedProducts = storageProducts;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllProducts()).Returns(storageProducts);

            //when
            IQueryable<Product> actualProducts = this.productService.RetrieveAllProducts();

            //then
            actualProducts.Should().BeEquivalentTo(expectedProducts);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllProducts(), Times.Once());

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}