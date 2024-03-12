using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.DatabaseObject.Model.Entity;

namespace Persistence.DatabaseObject.Configuration.EntityFramework
{
    public class BookConfiguration : IEntityTypeConfiguration<BookModel>
    {
        public void Configure(EntityTypeBuilder<BookModel> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Title).HasMaxLength(20);
            builder.Property(e => e.Description).HasMaxLength(255);
            builder.Property(e => e.Author).HasMaxLength(30);
            builder.Property(x => x.Year);
        }
    }
}
