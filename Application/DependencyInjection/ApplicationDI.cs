using Application.Auth.Service;
using Application.Jwt.Service;
using Application.User.Service;
using Associated.Jwt.Interface.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection
{
    public static class ApplicationDI
    {
        public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<IJwtService, JwtService>();

            return builder;
        }
    }
}
