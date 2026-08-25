using IdentityEmailApp.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.ViewComponents.NewViewComponents
{
    public class _NewWeatherComponent : ViewComponent
    {
        private readonly IWeatherService _weatherService;

        public _NewWeatherComponent(
            IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public async Task<IViewComponentResult> InvokeAsync(
            double latitude = 41.0082,
            double longitude = 28.9784)
        {
            var weather = await _weatherService
                .GetWeatherByLocationAsync(
                    latitude,
                    longitude);

            return View(weather);
        }
    }
}