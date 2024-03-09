using Associated.Persistence.Enum;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Associated.Persistence.Factory
{
    public static class DbContextFactory
    {
        public static WebApplicationBuilder CreateDbContext<TContext>(this WebApplicationBuilder builder, DbType dbType, string conStrKey = "", ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where TContext : DbContext
        {
            if (builder is null) throw new ArgumentNullException(nameof(builder));

            var conStr = "";
            conStr = string.IsNullOrEmpty(conStrKey)
                ? builder.Configuration.GetConnectionString(dbType.ToString())
                : builder.Configuration.GetConnectionString(conStrKey);

            if (string.IsNullOrEmpty(conStr)) throw new ArgumentException($"Unfound connection string under they key of {conStrKey}");

            switch (dbType)
            {
                case DbType.MySql:
                    builder.Services.AddDbContext<TContext>(options => options.UseMySql(conStr, ServerVersion.AutoDetect(conStr)), serviceLifetime);
                    break;
                default:
                    // code block
                    break;
            }

            return builder;
        }
    }
}
