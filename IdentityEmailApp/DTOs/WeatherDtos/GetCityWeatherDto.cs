using System.Text.Json.Serialization;

namespace IdentityEmailApp.DTOs.WeatherDtos
{
    public class GetCityWeatherDto
    {
        [JsonPropertyName("cod")]
        public string Cod { get; set; }

        [JsonPropertyName("message")]
        public int Message { get; set; }

        [JsonPropertyName("cnt")]
        public int Count { get; set; }

        [JsonPropertyName("list")]
        public List<WeatherForecastItemDto> Forecasts { get; set; }

        [JsonPropertyName("city")]
        public WeatherCityDto City { get; set; }
    }

    public class WeatherCityDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("coord")]
        public WeatherCoordinateDto Coordinate { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("population")]
        public int Population { get; set; }

        [JsonPropertyName("timezone")]
        public int Timezone { get; set; }

        [JsonPropertyName("sunrise")]
        public long Sunrise { get; set; }

        [JsonPropertyName("sunset")]
        public long Sunset { get; set; }
    }

    public class WeatherCoordinateDto
    {
        [JsonPropertyName("lat")]
        public double Latitude { get; set; }

        [JsonPropertyName("lon")]
        public double Longitude { get; set; }
    }

    public class WeatherForecastItemDto
    {
        [JsonPropertyName("dt")]
        public long DateTimeUnix { get; set; }

        [JsonPropertyName("main")]
        public WeatherMainDto Main { get; set; }

        [JsonPropertyName("weather")]
        public List<WeatherDescriptionDto> Weather { get; set; }

        [JsonPropertyName("clouds")]
        public WeatherCloudsDto Clouds { get; set; }

        [JsonPropertyName("wind")]
        public WeatherWindDto Wind { get; set; }

        [JsonPropertyName("visibility")]
        public int Visibility { get; set; }

        [JsonPropertyName("pop")]
        public double PrecipitationProbability { get; set; }

        [JsonPropertyName("sys")]
        public WeatherSystemDto System { get; set; }

        [JsonPropertyName("dt_txt")]
        public string DateText { get; set; }

        [JsonPropertyName("rain")]
        public WeatherRainDto Rain { get; set; }
    }

    public class WeatherMainDto
    {
        [JsonPropertyName("temp")]
        public double Temperature { get; set; }

        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; }

        [JsonPropertyName("temp_min")]
        public double MinimumTemperature { get; set; }

        [JsonPropertyName("temp_max")]
        public double MaximumTemperature { get; set; }

        [JsonPropertyName("pressure")]
        public int Pressure { get; set; }

        [JsonPropertyName("sea_level")]
        public int SeaLevel { get; set; }

        [JsonPropertyName("grnd_level")]
        public int GroundLevel { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }

        [JsonPropertyName("temp_kf")]
        public double TemperatureDifference { get; set; }
    }

    public class WeatherCloudsDto
    {
        [JsonPropertyName("all")]
        public int Cloudiness { get; set; }
    }

    public class WeatherWindDto
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }

        [JsonPropertyName("deg")]
        public int Degree { get; set; }

        [JsonPropertyName("gust")]
        public double Gust { get; set; }
    }

    public class WeatherSystemDto
    {
        [JsonPropertyName("pod")]
        public string PartOfDay { get; set; }
    }

    public class WeatherRainDto
    {
        [JsonPropertyName("3h")]
        public double LastThreeHours { get; set; }
    }

    public class WeatherDescriptionDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("main")]
        public string Main { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }
    }
}