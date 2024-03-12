using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.DatabaseObject.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.DatabaseObject.Configuration.EntityFramework
{
    public class RoleConfiguration : IEntityTypeConfiguration<RoleModel>
    {
        public void Configure(EntityTypeBuilder<RoleModel> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Name).HasMaxLength(20);
            builder
                .HasMany(e => e.Users)
                .WithOne(e => e.Role);
        }
    }
}
