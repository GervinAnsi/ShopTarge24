using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using ShopTarge24.Data;
using ShopTarge24.Models.Spaceships;

namespace ShopTarge24.Controllers
{
    public class SpaceshipsController : Controller
    {
        private readonly ShopTarge24Context _context;

        public SpaceshipsController
            (
                ShopTarge24Context context
            )
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var result = _context.Spaceships
                .Select(x => new SpaceshipIndexViewModel 
                { 
                    Id = x.Id,
                    Name = x.Name,
                    Classification = x.Classification,
                    BuiltDate = x.BuiltDate,
                    Crew = x.Crew
                });

            return View(result);
        }
    }
}
