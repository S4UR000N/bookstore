using Microsoft.AspNetCore.Builder;

namespace Associated.Application.DependencyInjection
{
    public static class HstsDI
    {
        public static WebApplicationBuilder AddHsts(this WebApplicationBuilder builder)
        {
            builder.Services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });

            return builder;
        }
    }
}
