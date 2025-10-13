using Microsoft.AspNetCore.Mvc;

namespace ShopTarge24.Controllers
{
    public class AccuWeathersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
