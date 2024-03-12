using Microsoft.EntityFrameworkCore;
using Persistence.DatabaseObject.Model.Entity;

namespace Persistence.Context
{
    public class RelationalContext : DbContext
    {
        public RelationalContext(DbContextOptions<RelationalContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
    }
}
