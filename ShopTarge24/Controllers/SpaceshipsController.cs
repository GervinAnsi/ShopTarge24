using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using ShopTarge24.Core.Dto;
using ShopTarge24.Core.ServiceInterface;
using ShopTarge24.Data;
using ShopTarge24.Models.Spaceships;

namespace ShopTarge24.Controllers
{
    public class SpaceshipsController : Controller
    {
        private readonly ShopTarge24Context _context;
        private readonly ISpaceshipServices _spaceshipServices;

        public SpaceshipsController
            (
                ShopTarge24Context context,
                ISpaceshipServices spaceshipservices
            )
        {
            _context = context;
            _spaceshipServices = spaceshipservices;
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

        [HttpGet]
        public IActionResult Create()
        {
            SpaceshipCreateViewModel result = new();

            return View("Create", result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SpaceshipCreateViewModel vm)
        {
            var dto = new SpaceshipDto()
            {
                Id = vm.Id,
                Name = vm.Name,
                Classification = vm.Classification,
                BuiltDate = vm.BuiltDate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt
            };

            var result = await _spaceshipServices.Create( dto );

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var spaceship = await _spaceshipServices.DetailAsync(Id);

            if (spaceship == null)
            {
                return NotFound();
            }

            var vm = new SpaceshipDeleteViewModel();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Classification = spaceship.Classification;
            vm.BuiltDate = spaceship.BuiltDate;
            vm.Crew = spaceship.Crew;
            vm.EnginePower = spaceship.EnginePower;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;

            return View(vm);
        }


    }
}
