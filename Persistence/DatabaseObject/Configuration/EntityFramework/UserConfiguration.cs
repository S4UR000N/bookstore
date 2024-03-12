using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.DatabaseObject.Model.Entity;

namespace Persistence.DatabaseObject.Configuration.EntityFramework
{
    public class UserConfiguration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.FirstName).HasMaxLength(20);
            builder.Property(e => e.LastName).HasMaxLength(20);
            builder.Property(e => e.Email).HasMaxLength(30);
                builder.HasIndex(x => x.Email).IsUnique();
            builder.Property(e => e.Password).HasMaxLength(100);
            builder
                .HasOne(e => e.Role)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.RoleId);
        }
    }
}
