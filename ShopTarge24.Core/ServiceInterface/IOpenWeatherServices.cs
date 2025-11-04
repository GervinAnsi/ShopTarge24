using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopTarge24.Core.Dto;

namespace ShopTarge24.Core.ServiceInterface
{
    public interface IOpenWeatherRootDto
    {
        Task<OpenWeatherRootDto> GetCityWeather(string cityName);
    }
}
