using Associated.Persistence.Factory;
using Associated.Persistence.Enum;
using Persistence.Context;
using Microsoft.AspNetCore.Builder;

namespace Persistence.DependencyInjection
{
    public static class DbContextDI
    {
        public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
        {
            builder.CreateDbContext<RelationalContext>(DbType.MySql);

            return builder;
        }
    }
}