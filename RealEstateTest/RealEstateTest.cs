using ShopTarge24.Core.Dto;
using ShopTarge24.Core.ServiceInterface;
using System.Threading.Tasks;

namespace ShopTarge24.RealEstateTest
{
    public class RealEstateTest : TestBase
    {
        [Fact]
        public async Task ShouldNot_AddEmptyRealEstate_WhenReturnResult()
        {
            //arrange
            RealEstateDto dto = new()
            {
                Area = 120.5,
                Location = "Test Location",
                RoomNumber = 3,
                BuildingType = "Apartmanet",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };
        }
    }
}