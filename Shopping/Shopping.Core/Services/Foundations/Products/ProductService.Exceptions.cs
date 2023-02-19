// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shopping.Core.Models.Products;
using Shopping.Core.Models.Products.Exceptions;
using Xeptions;

namespace Shopping.Core.Services.Foundations.Products
{
    public partial class ProductService
    {
        private delegate IQueryable<Product> ReturningProductsFunction();
        private delegate ValueTask<Product> ReturningProductFunction();

        private async ValueTask<Product> TryCatch(ReturningProductFunction returningProductFunction)
        {
            try
            {
                return await returningProductFunction();
            }
            catch (NullProductException nullProductException)
            {
                throw CreateAndLogValidationException(nullProductException);
            }
            catch (InvalidProductException invalidProductException)
            {
                throw CreateAndLogValidationException(invalidProductException);
            }
            catch (NotFoundProductException notFoundProductException)
            {
                throw CreateAndLogValidationException(notFoundProductException);
            }
            catch (SqlException sqlException)
            {
                var failedProductStorageException = new FailedProductStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedProductStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsProductException =
                    new AlreadyExistsProductException(duplicateKeyException);

                throw CreateAndDependencyValidationException(alreadyExistsProductException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedProductException = new LockedProductException(dbUpdateConcurrencyException);

                throw CreateAndDependencyValidationException(lockedProductException);
            }
            catch (DbUpdateException databaseUpdateException)
            {
                var failedProductStorageException =
                    new FailedProductStorageException(databaseUpdateException);

                throw CreateAndLogDependencyException(failedProductStorageException);
            }
            catch (Exception exception)
            {
                var failedProductServiceException = new FailedProductServiceException(exception);

                throw CreateAndLogServiceException(failedProductServiceException);
            }
        }

        private IQueryable<Product> TryCatch(ReturningProductsFunction returningProductsFunction)
        {
            try
            {
                return returningProductsFunction();
            }
            catch (SqlException sqlException)
            {
                var failedProductServiceException = new FailedProductServiceException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedProductServiceException);
            }
            catch (Exception serviceException)
            {
                var failedProductServiceException = new FailedProductServiceException(serviceException);

                throw CreateAndLogServiceException(failedProductServiceException);
            }
        }

        private ProductValidationException CreateAndLogValidationException(Xeption exception)
        {
            var productValidationException = new ProductValidationException(exception);
            this.loggingBroker.LogError(productValidationException);

            return productValidationException;
        }

        private ProductDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var productDependencyException = new ProductDependencyException(exception);
            this.loggingBroker.LogCritical(productDependencyException);

            return productDependencyException;
        }

        private ProductDependencyValidationException CreateAndDependencyValidationException(Xeption exception)
        {
            var productDependencyValidationException =
                new ProductDependencyValidationException(exception);

            this.loggingBroker.LogError(productDependencyValidationException);

            return productDependencyValidationException;
        }
        private ProductServiceException CreateAndLogServiceException(Xeption exception)
        {
            var productServiceException = new ProductServiceException(exception);
            this.loggingBroker.LogError(productServiceException);

            return productServiceException;
        }

        private ProductDependencyException CreateAndLogDependencyException(Xeption exception)
        {
            var productDependencyException =
                new ProductDependencyException(exception);

            this.loggingBroker.LogError(productDependencyException);

            return productDependencyException;
        }
    }
}
