// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Shopping.Core.Brokers.Loggings;
using Shopping.Core.Brokers.Storages;
using Shopping.Core.Models.Products;

namespace Shopping.Core.Services.Foundations.Products
{
    public partial class ProductService : IProductService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public ProductService(IStorageBroker storageBroker, ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Product> AddProductAsync(Product product) =>
        TryCatch(async () =>
        {
            ValidateProduct(product);

            return await this.storageBroker.InsertProductAsync(product);
        });
        public IQueryable<Product> RetrieveAllProducts() =>
        TryCatch(() => this.storageBroker.SelectAllProducts());

        public ValueTask<Product> RetrieveProductByIdAsync(Guid productId) =>
        TryCatch(async () =>
        {
            ValidateProductId(productId);

            Product maybeProduct =
                await this.storageBroker.SelectProductByIdAsync(productId);

            ValidateStorageProduct(maybeProduct, productId);

            return maybeProduct;
        });

        public ValueTask<Product> ModifyProductAsync(Product product) =>
        TryCatch(async () =>
        {
            ValidateProduct(product);

            Product maybeProduct =
                await this.storageBroker.SelectProductByIdAsync(product.Id);

            ValidateStorageProduct(maybeProduct, product.Id);

            return await this.storageBroker.UpdateProductAsync(product);
        });

        public ValueTask<Product> RemoveProductByIdAsync(Guid productId) =>
        TryCatch(async () =>
        {
            ValidateProductId(productId);

            Product maybeProduct = await this.storageBroker.SelectProductByIdAsync(productId);

            ValidateStorageProduct(maybeProduct, productId);

            return await this.storageBroker.DeleteProductAsync(maybeProduct);
        });
    }
}