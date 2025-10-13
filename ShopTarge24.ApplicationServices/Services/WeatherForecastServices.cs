using System.Text.Json;
using ShopTarge24.Core.Dto;
using ShopTarge24.Core.ServiceInterface;

namespace ShopTarge24.ApplicationServices.Services
{
    //tallinn = 127964
    public class WeatherForecastServices : IWeatherForecastServices
    {
        public async Task<AccuLocationWeatherResultDto> AccuWeatherResult(AccuLocationWeatherResultDto dto)
        {
            var response = $"https://api.weatherapi.com/v1/current.json";
            
            using (var client= new HttpClient())
            {
                var httpResponse = await client.GetAsync(response);
                string json = await httpResponse.Content.ReadAsStringAsync();

                List<AccuLocationRootDto> weatherData = 
                    JsonSerializer.Deserialize<List<AccuLocationRootDto>>(json);

            }

            return dto;
        }
    }
}
