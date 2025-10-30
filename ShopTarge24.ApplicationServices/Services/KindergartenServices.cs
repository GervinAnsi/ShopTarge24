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

        public KindergartenServices
            (
                ShopTarge24Context context
            )
        {
            _context = context;
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

            await _context.Kindergartens.AddAsync(Kindergarten);
            await _context.SaveChangesAsync(); 

            return Kindergarten;
        }

        public async Task<Kindergarten> Update(KindergartenDto dto)
        {
            //vaja leida doamini objekt, mida saaks mappida dto-ga
            var kindergarten = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (kindergarten == null)
            {
                throw new Exception("Ei leia kindergartenit");
            }

            
            kindergarten.GroupName = dto.GroupName;
            kindergarten.ChildrenCount = dto.ChildrenCount;
            kindergarten.KindergartenName = dto.KindergartenName;
            kindergarten.TeacherName = dto.TeacherName;
            kindergarten.CreatedAt = DateTime.Now;
            kindergarten.UpdatedAt = DateTime.Now;

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

        public async Task<Kindergarten?> Delete(Guid id)
        {

            var result = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.Kindergartens.Remove(result);
            await _context.SaveChangesAsync();

            if (result == null)
                return null;

            var images = await _context.FileToApis
                .Where (x=> x.KindergartenId == id)
                .ToListAsync();

            if (images.Count > 0)
            {
                _context.FileToApis.RemoveRange(images);
            }

            _context.Kindergartens.Remove(result);
            await _context.SaveChangesAsync();
            return result;
        }


    }
}
