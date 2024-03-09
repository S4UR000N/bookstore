using Microsoft.AspNetCore.Builder;
using System.Net;

namespace Associated.Application.DependencyInjection
{
    public static class HttpsDI
    {
        public static WebApplicationBuilder AddHttpsRedirection(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
                options.HttpsPort = 443;
            });

            return builder;
        }
    }
}
