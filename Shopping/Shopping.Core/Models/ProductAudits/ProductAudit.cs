// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations.Schema;
using Shopping.Core.Models.Products;
using Shopping.Core.Models.Users;

namespace Shopping.Core.Models.ProductAudits
{
    public class ProductAudit
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string FieldName { get; set; }
        public string Operation { get; set; }
        public Guid UpdateUserId { get; set; }
        [ForeignKey(nameof(UpdateUserId))]
        public User User { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}