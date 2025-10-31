using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTarge24.ApplicationServices.Services;
using ShopTarge24.Core.Domain;
using ShopTarge24.Core.Dto;
using ShopTarge24.Core.ServiceInterface;
using ShopTarge24.Data;
using ShopTarge24.Models.Kindergarten;
using ShopTarge24.Models.Spaceships;

namespace ShopTarge24.Controllers
{
    public class KindergartenController : Controller
    {
        private readonly ShopTarge24Context _context;
        private readonly IKindergartenServices _kindergartenServices;
        private readonly IFileServices fileservices;

        public KindergartenController
            (
                ShopTarge24Context context,
                IKindergartenServices kindergartenservices,
                IFileServices fileServices
            )
        {
            _context = context;
            _kindergartenServices = kindergartenservices;
            this.fileservices = fileServices;
        }

        public IActionResult Index()
        {
            var result = _context.Kindergartens
                .Select(x => new KindergartenIndexViewModel
                { 
                    Id = x.Id,
                    GroupName = x.GroupName,
                    ChildrenCount = x.ChildrenCount,
                    KindergartenName = x.KindergartenName,
                    TeacherName = x.TeacherName
                });

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            KindergartenCreateUpdateViewModel result = new();

            return View("CreateUpdate", result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KindergartenCreateUpdateViewModel vm)
        {

            if (!ModelState.IsValid)
            {
                return View("CreateUpdate", vm);
            }

            var dto = new KindergartenDto()
            {
                Id = Guid.NewGuid(),
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = vm.UpdatedAt
            };

            var created = await _kindergartenServices.Create( dto );

            await fileservices.FilesToApi(dto, created ?? new Kindergarten { Id = dto.Id });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = dto.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.KindergartenId == kindergarten.Id)
                .Select(y => new ImageViewModel
                {
                    Filepath = y.ExistingFilePath,
                    ImageId = y.Id,
                    KindergartenId = y.KindergartenId,
                    ImageTitle = y.ImageTitle,
                    ImageData = y.ImageData,
                    Images = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(y.ImageData))
                })
                .ToListAsync();

            var vm = new KindergartenCreateUpdateViewModel
            {
                Id = kindergarten.Id,
                GroupName = kindergarten.GroupName,
                ChildrenCount = kindergarten.ChildrenCount,
                KindergartenName = kindergarten.KindergartenName,
                TeacherName = kindergarten.TeacherName,
                Images = images
            };

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(KindergartenCreateUpdateViewModel vm)
        {

            if (!ModelState.IsValid)
            {
                vm.Images = await _context.FileToApis
                    .Where(x => x.KindergartenId == vm.Id)
                    .Select(y => new ImageViewModel()
                    {
                        Filepath = y.ExistingFilePath,
                        ImageId = y.Id,
                        KindergartenId = y.KindergartenId,
                        ImageTitle = y.ImageTitle,
                        ImageData = y.ImageData,
                        Images = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(y.ImageData))
                    })
                    .ToListAsync();

                return View("CreateUpdate", vm);
            }


            var dto = new KindergartenDto()
            {
                Id = vm.Id ?? Guid.Empty,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                Files = vm.Files
            };

            var updated = await _kindergartenServices.Update(dto);

            await fileservices.FilesToApi(dto, updated ?? new Kindergarten { Id = dto.Id });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = dto.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> RemoveImage(Guid ImageId, Guid KindergartenId)
        {
            await fileservices.RemoveImageFromApi(new FileToApiDto { Id = ImageId });

            return RedirectToAction("Update", "Kindergarten", new { id = KindergartenId });
        }


        [HttpGet]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(Id);
            if (kindergarten == null) return NotFound();

            var images = await _context.FileToApis
                .Where(x => x.KindergartenId == Id)
                .Select(y => new ImageViewModel
                {
                    Filepath = y.ExistingFilePath,
                    ImageId = y.Id,
                    KindergartenId = y.KindergartenId,
                    ImageTitle = y.ImageTitle,
                    ImageData = y.ImageData,
                    Images = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(y.ImageData))
                })
                .ToListAsync();

            var vm = new KindergartenDeleteViewModel
            {
                Id = kindergarten.Id,
                GroupName = kindergarten.GroupName,
                KindergartenName = kindergarten.KindergartenName,
                TeacherName = kindergarten.TeacherName,
                Images = images
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            await _kindergartenServices.Delete(id);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(id);
            if (kindergarten == null) return NotFound();

            var images = await _context.FileToApis
                .Where(x => x.KindergartenId == kindergarten.Id)
                .Select(y => new ImageViewModel
                {
                    Filepath = y.ExistingFilePath,
                    ImageId = y.Id,
                    KindergartenId = y.KindergartenId,
                    ImageData = y.ImageData,
                    ImageTitle = y.ImageTitle,
                    Images = string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(y.ImageData))
                })
                .ToListAsync();

            //toimub viewModeliga mappimine
            var vm = new KindergartenDetailsViewModel
            {
                Id = kindergarten.Id,
                GroupName = kindergarten.GroupName,
                ChildrenCount = kindergarten.ChildrenCount,
                KindergartenName = kindergarten.KindergartenName,
                TeacherName = kindergarten.TeacherName,
                Images = images
            };

            return View(vm);
        }
    }
}
