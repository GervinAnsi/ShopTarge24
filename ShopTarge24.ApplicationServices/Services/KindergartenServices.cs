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
            Kindergarten Kindergarten = new Kindergarten();

            Kindergarten.Id = Guid.NewGuid();
            Kindergarten.GroupName = dto.GroupName;
            Kindergarten.ChildrenCount = dto.ChildrenCount;
            Kindergarten.KindergartenName = dto.KindergartenName;
            Kindergarten.TeacherName = dto.TeacherName;
            Kindergarten.CreatedAt = DateTime.Now;
            Kindergarten.UpdatedAt = DateTime.Now;

            //tuleb db-s teha andmete uuendamine jauue oleku salvestamine
            _context.Kindergartens.Update(Kindergarten);
            await _context.SaveChangesAsync();

            return Kindergarten;
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

            return result;
        }


    }
}
