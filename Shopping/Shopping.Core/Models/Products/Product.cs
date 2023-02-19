// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

namespace Shopping.Core.Models.Products
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}