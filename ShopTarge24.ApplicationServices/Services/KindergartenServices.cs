using Microsoft.EntityFrameworkCore;
using ShopTarge24.Core.Domain;
using ShopTarge24.Core.Dto;
using ShopTarge24.Core.ServiceInterface;
using ShopTarge24.Data;
using ShopTARge24.ApplicationServices.Services;

namespace ShopTarge24.ApplicationServices.Services
{
    public class KindergartenServices : IKindergartenServices
    {
        private readonly ShopTarge24Context _context;
        private readonly IFileServices _fileServices;

        public KindergartenServices
            (
                ShopTarge24Context context
            ,   IFileServices fileServices
            )
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<Kindergarten> Create(KindergartenDto dto)
        {
            Kindergarten Kindergarten = new Kindergarten();

            Kindergarten.Id = Guid.NewGuid();
            Kindergarten.GroupName = dto.GroupName;
            Kindergarten.ChildrenCount = dto.ChildrenCount;
            Kindergarten.KindergartenName= dto.KindergartenName;
            Kindergarten.TeacherName = dto.TeacherName;
            Kindergarten.CreatedAt = DateTime.Now;
            Kindergarten.UpdatedAt = DateTime.Now;
            _fileServices.FilesToApi(dto, Kindergarten);

            await _context.Kindergartens.AddAsync(Kindergarten);
            await _context.SaveChangesAsync(); 

            return Kindergarten;
        }

        public async Task<Kindergarten> Update(KindergartenDto dto)
        {
            Kindergarten kindergarten = new Kindergarten();
            
            kindergarten.Id = dto.Id;
            kindergarten.GroupName = dto.GroupName;
            kindergarten.ChildrenCount = dto.ChildrenCount;
            kindergarten.KindergartenName = dto.KindergartenName;
            kindergarten.TeacherName = dto.TeacherName;
            kindergarten.CreatedAt = DateTime.Now;
            kindergarten.UpdatedAt = DateTime.Now;
            _fileServices.FilesToApi(dto, kindergarten);

            //tuleb db-s teha andmete uuendamine jauue oleku salvestamine
            _context.Kindergartens.Update(kindergarten);
            await _context.SaveChangesAsync();

            return kindergarten;
        }



        public async Task<Kindergarten> DetailAsync(Guid id)
        {
            var result = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }


        public async Task<Kindergarten> Delete(Guid id)
        {
            var result = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            var images = await _context.FileToApis
                .Where(x => x.KindergartenId == id)
                .Select(y => new FileToApiDto
                {
                    ImageId = y.Id,
                    KindergartenId = y.KindergartenId,
                    ExistingFilePath = y.ExistingFilePath,
                }).ToArrayAsync();

            await _fileServices.RemoveImagesFromApi(images);
            _context.Kindergartens.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }


    }
}
