// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using System;

namespace Shopping.Core.Models.ProductAudits
{
    public class ProductAudit
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string FieldName { get; set; }
        public string Operation { get; set; }
        public Guid UpdateUserId { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
    }
}