// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Shopping.Core.Models.Products;
using Shopping.Core.Models.Products.Exceptions;
using Shopping.Core.Services.Foundations.Products;

namespace Shopping.Core.Controllers
{
    public class ProductController : RESTFulController
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService) =>
            this.productService = productService;

        [HttpPost]
        public async ValueTask<ActionResult<Product>> PostProductAsync(Product product)
        {
            try
            {
                return await this.productService.AddProductAsync(product);
            }
            catch (ProductValidationException productValidationExpection)
            {
                return BadRequest(productValidationExpection.InnerException);
            }
            catch (ProductDependencyValidationException productDependencyValidationException)
                when (productDependencyValidationException.InnerException is AlreadyExistsProductException)
            {
                return Conflict(productDependencyValidationException.InnerException);
            }
            catch (ProductDependencyValidationException productDependencyValidationException)
            {
                return BadRequest(productDependencyValidationException.InnerException);
            }
            catch (ProductDependencyException productDependencyException)
            {
                return InternalServerError(productDependencyException.InnerException);
            }
            catch (ProductServiceException productServiceException)
            {
                return InternalServerError(productServiceException.InnerException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Product>> GetAllProducts()
        {
            try
            {
                IQueryable<Product> allProducts = this.productService.RetrieveAllProducts();

                return Ok(allProducts);
            }
            catch (ProductDependencyException productDependencyException)
            {
                return InternalServerError(productDependencyException.InnerException);
            }
            catch (ProductServiceException productServiceException)
            {
                return InternalServerError(productServiceException.InnerException);
            }
        }

        [HttpGet("{productId}")]
        public async ValueTask<ActionResult<Product>> GetProductByIdAsync(Guid productId)
        {
            try
            {
                return await this.productService.RetrieveProductByIdAsync(productId);
            }
            catch (ProductDependencyException productDependencyException)
            {
                return InternalServerError(productDependencyException.InnerException);
            }
            catch (ProductValidationException productValidationException)
                when (productValidationException.InnerException is InvalidProductException)
            {
                return BadRequest(productValidationException.InnerException);
            }
            catch (ProductValidationException productValidationException)
                when (productValidationException.InnerException is InvalidProductException)
            {
                return NotFound(productValidationException.InnerException);
            }
            catch (ProductServiceException productServiceException)
            {
                return InternalServerError(productServiceException.InnerException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Product>> PutProductAsync(Product product)
        {
            try
            {
                Product modifiedProduct =
                    await this.productService.ModifyProductAsync(product);

                return base.Ok(modifiedProduct);
            }
            catch (ProductValidationException productValidationException)
                when (productValidationException.InnerException is NotFoundProductException)
            {
                return NotFound(productValidationException.InnerException);
            }
            catch (ProductValidationException productValidationException)
            {
                return BadRequest(productValidationException.InnerException);
            }
            catch (ProductDependencyValidationException productDependencyValidationException)
                when (productDependencyValidationException.InnerException is AlreadyExistsProductException)
            {
                return Conflict(productDependencyValidationException.InnerException);
            }
            catch (ProductDependencyException productDependencyException)
            {
                return InternalServerError(productDependencyException.InnerException);
            }
            catch (ProductServiceException productServiceException)
            {
                return InternalServerError(productServiceException.InnerException);
            }
        }

        [HttpDelete("{productId}")]
        public async ValueTask<ActionResult<Product>> DeleteProductrByIdAsync(Guid productId)
        {
            try
            {
                Product deletedProduct =
                    await this.productService.RemoveProductByIdAsync(productId);

                return Ok(deletedProduct);
            }
            catch (ProductValidationException productValidationException)
                when (productValidationException.InnerException is NotFoundProductException)
            {
                return NotFound(productValidationException.InnerException);
            }
            catch (ProductValidationException productValidationException)
            {
                return BadRequest(productValidationException.InnerException);
            }
            catch (ProductDependencyValidationException productDependencyValidationException)
                when (productDependencyValidationException.InnerException is LockedProductException)
            {
                return Locked(productDependencyValidationException.InnerException);
            }
            catch (ProductDependencyValidationException productDependencyValidationException)
            {
                return BadRequest(productDependencyValidationException.InnerException);
            }
            catch (ProductDependencyException productDependencyException)
            {
                return InternalServerError(productDependencyException.InnerException);
            }
            catch (ProductServiceException productServiceException)
            {
                return InternalServerError(productServiceException.InnerException);
            }
        }
    }
}