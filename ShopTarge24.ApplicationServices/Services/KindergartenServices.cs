using Microsoft.EntityFrameworkCore;
using ShopTarge24.Core.Domain;
using ShopTarge24.Core.Dto;
using ShopTarge24.Core.ServiceInterface;
using ShopTarge24.Data;

namespace ShopTarge24.ApplicationServices.Services
{
    public class KindergartenServices : IKindergartenServices
    {
        private readonly ShopTarge24Context _context;
        private readonly IFileServices _fileServices;

        public KindergartenServices
        (
            ShopTarge24Context context,
            IFileServices fileServices
        )
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<Kindergarten> Create(KindergartenDto dto)
        {
            var kindergarten = new Kindergarten
            {
                Id = Guid.NewGuid(),
                GroupName = dto.GroupName,
                ChildrenCount = dto.ChildrenCount,
                KindergartenName = dto.KindergartenName,
                TeacherName = dto.TeacherName,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // API failid
            _fileServices.FilesToApi(dto, kindergarten);

            await _context.Kindergartens.AddAsync(kindergarten);
            await _context.SaveChangesAsync();

            // DATABASE failid
            if (dto.Files != null && dto.Files.Count > 0)
            {
                _fileServices.UploadFilesToDatabase(dto, kindergarten);
            }

            return kindergarten;
        }

        public async Task<Kindergarten> Update(KindergartenDto dto)
        {
            var kindergarten = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (kindergarten == null) return null;

            kindergarten.GroupName = dto.GroupName;
            kindergarten.ChildrenCount = dto.ChildrenCount;
            kindergarten.KindergartenName = dto.KindergartenName;
            kindergarten.TeacherName = dto.TeacherName;
            kindergarten.UpdatedAt = DateTime.Now;

            // API failid
            _fileServices.FilesToApi(dto, kindergarten);

            _context.Kindergartens.Update(kindergarten);
            await _context.SaveChangesAsync();

            // DATABASE failid
            if (dto.Files != null && dto.Files.Count > 0)
            {
                _fileServices.UploadFilesToDatabase(dto, kindergarten);
            }

            return kindergarten;
        }

        public async Task<Kindergarten> DetailAsync(Guid id)
        {
            return await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Kindergarten> Delete(Guid id)
        {
            var domain = await _context.Kindergartens.FirstOrDefaultAsync(x => x.Id == id);
            if (domain == null) return null;

            var images = await _context.FileToDatabases.Where(x => x.KindergartenId == id).ToArrayAsync();
            foreach (var img in images)
            {
                _context.FileToDatabases.Remove(img);
            }

            _context.Kindergartens.Remove(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<bool> RemoveImage(Guid imageId)
        {
            var file = await _context.FileToDatabases
                .FirstOrDefaultAsync(x => x.Id == imageId);

            if (file == null)
                return false;

            _context.FileToDatabases.Remove(file);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
