using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopTarge24.Core.Domain;

namespace ShopTarge24.Data
{
    public class ShopTarge24Context : IdentityDbContext<ApplicationUser>
    {

        public ShopTarge24Context(DbContextOptions<ShopTarge24Context> options)
            : base(options) { }


        public DbSet<Spaceships> Spaceships { get; set; }

        public DbSet<FileToApi> FileToApis { get; set; }
        public DbSet<RealEstate> RealEstates { get; set; }
        public DbSet<FileToDatabase> FileToDatabases { get; set; }
        public DbSet<Kindergarten> Kindergartens { get; set; }
        public DbSet<IdentityRole> IdentityRoles { get; set; }

    }
}
