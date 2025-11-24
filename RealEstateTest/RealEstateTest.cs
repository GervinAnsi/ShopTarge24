using Microsoft.AspNetCore.Http.HttpResults;
using ShopTarge24.Core.Dto;
using ShopTarge24.Core.ServiceInterface;
using ShopTarge24.RealEstateTest;


namespace ShopTARge24.RealEstateTest
{
    public class RealEstateTest : TestBase
    {
        [Fact]
        public async Task ShouldNot_AddEmptyRealEstate_WhenReturnResult()
        {
            // Arrange
            RealEstateDto dto = new()
            {
                Area = 120.5,
                Location = "Test Location",
                RoomNumber = 3,
                BuildingType = "Apartment",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };

            // Act
            var result = await Svc<IRealEstateServices>().Create(dto);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldNot_GetByIdRealestate_WhenReturnsNotEqual()
        {
            //arrange
            Guid wrongGuid = Guid.NewGuid();
            Guid guid = Guid.Parse("68ce7565-9105-4945-b428-b8e25ec061c6");

            //act
            await Svc<IRealEstateServices>().DetailAsync(guid);

            //assert
            Assert.NotEqual(wrongGuid, guid);
        }

        [Fact]
        public async Task Should_GetByIdRealestate_WhenReturnsEqual()
        {
            //arrange
            Guid databaseGuid = Guid.Parse("68ce7565-9105-4945-b428-b8e25ec061c6");
            Guid guid = Guid.Parse("68ce7565-9105-4945-b428-b8e25ec061c6");
            //act
            await Svc<IRealEstateServices>().DetailAsync(guid);

            //assert
            Assert.Equal(databaseGuid, guid);
        }

        [Fact]
        public async Task Should_DeleteByIdRealEstate_WhenDeleteRealEstate()
        {
            //arrange
            RealEstateDto dto = MockRealEstateDto();

            //act
            var addRealEstate = await Svc<IRealEstateServices>().Create(dto);
            var deleteRealEstate = await Svc<IRealEstateServices>().Delete((Guid)addRealEstate.Id);

            //assert
            Assert.Equal(addRealEstate.Id, deleteRealEstate.Id);
        }

        [Fact]
        public async Task ShouldNot_DeleteByIdRealEstate_WhenDidNotDeleteRealEstate()
        {
            //arrange
            var dto = MockRealEstateDto();

            //act
            var realEstate1 = await Svc<IRealEstateServices>().Create(dto);
            var realEstate2 = await Svc<IRealEstateServices>().Create(dto);

            var result = await Svc<IRealEstateServices>().Delete((Guid)realEstate2.Id);

            //assert
            Assert.NotEqual(realEstate1.Id, result.Id);
        }

        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateData()
        {
            //arrange
            var guid = new Guid("68ce7565-9105-4945-b428-b8e25ec061c6");

            RealEstateDto dto = MockRealEstateDto();

            RealEstateDto domain = new();

            domain.Id = Guid.Parse("68ce7565-9105-4945-b428-b8e25ec061c6");
            domain.Area = 200.0;
            domain.Location = "Updated Location";
            domain.RoomNumber = 5;
            domain.BuildingType = "Villa";
            domain.CreatedAt = DateTime.UtcNow;
            domain.ModifiedAt = DateTime.UtcNow;

            //act
            await Svc<IRealEstateServices>().Update(dto);

            //assert
            Assert.Equal(domain.Id, guid);
            Assert.NotEqual(dto.Area, domain.Area);
            Assert.NotEqual(dto.RoomNumber, domain.RoomNumber);
            //Võrrelda RoomNumbrit ja kasutada DoesNotMatch
            Assert.DoesNotMatch(dto.RoomNumber.ToString(), domain.RoomNumber.ToString());
            Assert.DoesNotMatch(dto.Location, domain.Location);
        }

        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateDataVersion2()
        {

            //lõpus kontrollime et andmed on erinevad
            //arrange and act
            //alguses andmed luuakse ja kasutame MockRealEstateDto meetodit
            RealEstateDto dto = MockRealEstateDto();
            var createRealEstate = await Svc<IRealEstateServices>().Create(dto);

            //andmed uuendatakse ja kasutame uut Mock meetodit(selle peab ise tegema)
            RealEstateDto updatedDto = MockUpdateRealEstateData();
            var result = await Svc<IRealEstateServices>().Update(updatedDto);

            //assert
            Assert.DoesNotMatch(createRealEstate.Location, result.Location);
            Assert.NotEqual(createRealEstate.ModifiedAt, result.ModifiedAt);
        }

        [Fact]
        public async Task ShouldNot_UpdateRealEstate_WhenDidNotUpdateData()
        {
            RealEstateDto dto = MockRealEstateData();
            var createRealEstate = await Svc<IRealEstateServices>().Create(dto);

            RealEstateDto nulldto = MockNullRealEstateData();
            var result = await Svc<IRealEstateServices>().Update(dto);

            Assert.NotEqual(createRealEstate.Id, result.Id);

        }

        //tuled valja moelda kolm erinevat xUnit testi
        //saate teha 2-3 meeskondades

        //ShouldUpdateModifiedAt_WhenUpdateData()
        //ShouldNotRenewCreateAt_WhenUpdateData()
        //ShouldCheckRealEstateIdIsUnique()

        //ShouldUpdateModifiedAt_WhenUpdateData();
        [Fact]
        public async Task ShouldUpdateModifiedAt_WhenUpdateData()
        {
            //arrange - loome meetod Create
            RealEstateDto dto = MockRealEstateDto();
            var create = await Svc<IRealEstateServices>().Create(dto);

            //act - uued MockUpdateRealEstateData andmed
            RealEstateDto update = MockUpdateRealEstateData();
            var result = await Svc<IRealEstateServices>().Update(update);

            //assert = Kontrollime, et ModifiedAt muutuks
            Assert.NotEqual(create.ModifiedAt, result.ModifiedAt);
        }


        //ShouldNotRenewCreateAt_WhenUpdateData();
        [Fact]
        public async Task ShouldNotRenewCreatedAt_WhenUpdateData()
        {
            //arrange
            // teeme muutuja CreatedAt originaaliks, mis peab jaama
            // loome CreatedAt
            RealEstateDto dto = MockRealEstateDto();
            var create = await Svc<IRealEstateServices>().Create(dto);
            var originalCreatedAt = create.CreatedAt;

            //act - uuendame MockUpdateRealEstateData andmeid
            RealEstateDto update = MockUpdateRealEstateData();
            var result = await Svc<IRealEstateServices>().Update(update);

            //assert - kontrollime, et uuendamisel ei uuendaks CreatedAt
            Assert.NotEqual(originalCreatedAt, update.CreatedAt);
        }

        //ShouldCheckRealEstateIdIsUnique()
        [Fact]
        public async Task ShouldCheckRealEstateIdIsUnique()
        {
            //arrange - loome kaks objekti
            RealEstateDto dto1 = MockRealEstateDto();
            RealEstateDto dto2 = MockRealEstateDto();

            //act - kasutame Id loomiseks
            var create1 = await Svc<IRealEstateServices>().Create(dto1);
            var create2 = await Svc<IRealEstateServices>().Create(dto2);

            //assert - kontrollib, et ID oleks erinev
            Assert.NotEqual(create1.Id, create2.Id);
        }

        private RealEstateDto MockRealEstateDto()
        {
            return new RealEstateDto
            {
                Area = 150.0,
                Location = "Sample Location",
                RoomNumber = 4,
                BuildingType = "House",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };
        }

        private RealEstateDto MockUpdateRealEstateData()
        {
            RealEstateDto realEstate = new()
            {
                Area = 100.0,
                Location = "Secret Location",
                RoomNumber = 7,
                BuildingType = "Hideout",
                CreatedAt = DateTime.Now.AddYears(1),
                ModifiedAt = DateTime.Now.AddYears(1)
            };

            return realEstate;
        }

        private RealEstateDto MockRealEstateData()
        {
            RealEstateDto realEstate = new()
            {
                Area = 100.0,
                Location = "Secret Location",
                RoomNumber = 7,
                BuildingType = "Hideout",
                CreatedAt = DateTime.Now.AddYears(1),
                ModifiedAt = DateTime.Now.AddYears(1)
            };

            return realEstate;
        }

        private RealEstateDto MockNullRealEstateData()
        {
            return new RealEstateDto
            {
                Area = 0,
                Location = "",
                RoomNumber = 0,
                BuildingType = "",
                CreatedAt = DateTime.Now.AddYears(1),
                ModifiedAt = DateTime.Now.AddYears(1)
            };
            

        }


    }
}