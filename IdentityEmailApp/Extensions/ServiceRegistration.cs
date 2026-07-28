using IdentityEmailApp.Services.Abstract;
using IdentityEmailApp.Services.Concrete;

namespace IdentityEmailApp.Extensions
{
    public static class ServiceRegistration
    {
        public static void ConfigureService(this IServiceCollection services)
        {
            services.AddScoped<ISystemEventService, SystemEventService>();
            services.AddScoped<IAIGenerateResponse, AIGenerateResponse>();
            services.AddScoped<INewsService, NewsService>();
        }
    }
}
