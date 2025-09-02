using Microsoft.AspNetCore.Mvc;

namespace ShopTarge24.Controllers
{
    public class SpaceshipsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
