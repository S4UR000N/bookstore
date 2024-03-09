using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.DatabaseObject.Model.Entity;

namespace Persistence.DatabaseObject.Configuration.EntityFramework
{
    public class UserConfiguration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.Property(e => e.Name).HasMaxLength(20);
            builder.Property(e => e.Password).HasMaxLength(30);
        }
    }
}
