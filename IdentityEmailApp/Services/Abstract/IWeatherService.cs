using IdentityEmailApp.DTOs.WeatherDtos;

namespace IdentityEmailApp.Services.Abstract
{
    public interface IWeatherService
    {
        Task<GetCityWeatherDto?> GetWeatherByLocationAsync(double latitude = 41.0082,double longitude = 28.9784);
    }
}