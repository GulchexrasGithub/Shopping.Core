// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using Shopping.Core.Brokers.Loggings;
using Shopping.Core.Brokers.Storages;
using Shopping.Core.Models.Products;
using Shopping.Core.Services.Foundations.Products;
using Tynamix.ObjectFiller;
using Xeptions;

namespace Shopping.Core.Tests.Unit.Services.Foundations.Products
{
    public partial class ProductServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IProductService productService;

        public ProductServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.productService = new ProductService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static int GetRandomNumber() =>
           new IntRange(min: 1, max: 10).GetValue();

        private static string GetRandomMessage() =>
           new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static Product CreateRandomProduct() =>
            CreateProductFiller(GetRandomDateTimeOffset()).Create();

        private static DateTimeOffset GetRandomDateTimeOffset() =>
            new DateTimeRange(earliestDate: DateTime.UnixEpoch).GetValue();

        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static Expression<Func<Xeption, bool>> SameExceptionAs(Xeption expectedException) =>
            actualException => actualException.SameExceptionAs(expectedException);


        private static Filler<Product> CreateProductFiller(DateTimeOffset dates)
        {
            var filler = new Filler<Product>();

            filler.Setup()
                .OnType<DateTimeOffset>().Use(dates);

            return filler;
        }
    }
}