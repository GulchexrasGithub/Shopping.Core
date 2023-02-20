// ---------------------------------------------------------------
// Copyright (c) Gulchekhra Burkhonova
// Free To Use To Manage The Shopping In Markets 
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping.Core.Models.Users;

namespace Shopping.Core.Brokers.Storages
{
    public partial class StorageBroker
    {
        public void ConfigureUserEmail(EntityTypeBuilder<User> builder)
        {
            builder
                .HasIndex(user => user.Email)
                .IsUnique();
        }
    }
}