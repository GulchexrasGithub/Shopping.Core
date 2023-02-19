using Microsoft.EntityFrameworkCore;
using Shopping.Core.Models.Products;

namespace Shopping.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Product> Products { get; set; }
    }
}
