using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Associated.Application.Cors.DependencyInjection
{
    public static class CorsDI
    {
        public static WebApplicationBuilder AddCors(this WebApplicationBuilder builder)
        {
            var allowedOrigins = builder.Configuration
                .GetSection("AllowedOrigins")
                .AsEnumerable()
                .Select(o => o.Value)
                .Where(v => !string.IsNullOrEmpty(v))
                .ToArray();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("DefaultCorsPolicy",
                    builder => builder.WithOrigins(allowedOrigins!)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            return builder;
        }
    }
}
