using System.Text.Json;
using ShopTarge24.Core.Dto;


namespace ShopTarge24.ApplicationServices.Services
{
    public class OpenWeatherServices
    {
        private readonly HttpClient _httpClient;

        public OpenWeatherServices()
        {
            _httpClient = new HttpClient();
        }

        public async Task<OpenWeatherRootDto> GetCityWeather(string cityName)
        {
            const string ApiKey = "707f2b54aafd3db7e43408772976a616";
            const string Url = $"https://api.openweathermap.org/data/2.5/weather";

            if (string.IsNullOrEmpty(cityName))
                return null;

            var url = $"{Url}?q={cityName}&appid={ApiKey}&units=metric";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            return new OpenWeatherRootDto
            {
                CityName = root.GetProperty("name").GetString() ?? "",
                TempValue = root.GetProperty("main").GetProperty("temp").GetDouble(),
                TempFeelsLike = root.GetProperty("main").GetProperty("feels_like").GetDouble(),
                Humidity = root.GetProperty("main").GetProperty("humidity").GetInt32(),
                Pressure = root.GetProperty("main").GetProperty("pressure").GetInt32(),
                WindSpeed = root.GetProperty("wind").GetProperty("speed").GetDouble(),
                WeatherCondition = root.GetProperty("weather")[0].GetProperty("main").GetString()
            };
        }
    }
}