using Microsoft.AspNetCore.Mvc;

namespace IdentityEmailApp.Controllers
{
    public class WeatherController : Controller
    {
        
        [HttpGet]
        public IActionResult GetByLocation(double latitude, double longitude)
        {
            return ViewComponent(
                "_NewWeatherComponent",
                new
                {
                    latitude,
                    longitude
                });
        }
    }
}