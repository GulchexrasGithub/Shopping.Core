using Microsoft.EntityFrameworkCore;
using Shopping.Core.Models.Users;

namespace Shopping.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<User> Users { get; set; }
    }
}