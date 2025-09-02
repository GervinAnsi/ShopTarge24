using Microsoft.EntityFrameworkCore;
using ShopTarge24.Core.Domain;

namespace ShopTarge24.Data
{
    public class ShopTarge24Context : DbContext
    {



        public DbSet<Spaceships> Spaceships { get; set; }
    }
}
