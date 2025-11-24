using Microsoft.AspNetCore.Mvc;
using ShopTarge24.Models.ChuckNorris;
using System.Net;
using System.Text.Json;

namespace ShopTARge24.Controllers
{
    public class ChuckNorrisController : Controller
    {
        public IActionResult Index()
        {
            ChuckNorrisViewModel Joke = GetRandomJoke();
            return View(Joke);
        }

        private ChuckNorrisViewModel GetRandomJoke()
        {
            string url = "https://api.chucknorris.io/jokes/random";
            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);
                var joke = JsonSerializer.Deserialize<ChuckNorrisViewModel>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return joke;
            }
        }
    }
}