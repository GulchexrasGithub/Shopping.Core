using Microsoft.EntityFrameworkCore;
using Shopping.Core.Models.ProductAudits;

namespace Shopping.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<ProductAudit> ProductAudits { get; set; }
    }
}