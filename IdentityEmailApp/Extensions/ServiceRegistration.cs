using IdentityEmailApp.Services.Abstract;
using IdentityEmailApp.Services.Concrete;

namespace IdentityEmailApp.Extensions
{
    public static class ServiceRegistration
    {
        public static void ConfigureService(this IServiceCollection services)
        {
            services.AddScoped<ISystemEventService, SystemEventService>();
            services.AddHttpClient<IAIGenerateResponse, AIGenerateResponse>();
            services.AddHttpClient<INewsService, NewsService>();
            services.AddHttpClient<ITranslateService, TranslateService>();
        }
    }
}
