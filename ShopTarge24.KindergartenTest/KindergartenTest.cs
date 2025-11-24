using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopTarge24.Core.Dto;
using ShopTarge24.Core.ServiceInterface;

namespace ShopTarge24.KindergartenTest
{
    public class KindergartenTest : TestBase
    {
        [Fact]
        public async Task ShouldCheck_KindergartenIdIsUnique()
        {
            KindergartenDto dto1 = MockKindergartenData();
            KindergartenDto dto2 = MockKindergartenData();

            var create1 = await Svc<IKindergartenServices>().Create(dto1);
            var create2 = await Svc<IKindergartenServices>().Create(dto2);

            Assert.NotEqual(create1.Id, create2.Id);

        }

        [Fact]
        public async Task ShouldNotRenewCreatedAt_WhenUpdateData()
        {
            var createDto = MockKindergartenData();
            var created = await Svc<IKindergartenServices>().Create(createDto);
            var originalCreatedAt = created.CreatedAt;

            var updateDto = MockUpdateKindergartenData();
            updateDto.Id = created.Id;

            var updated = await Svc<IKindergartenServices>().Update(updateDto);

            Assert.Equal(originalCreatedAt, updated.CreatedAt);
        }

        [Fact]
        public async Task ShouldDeleteByIdKindergarten_WhenDelete()
        {
            var dto = MockKindergartenData();
            var created = await Svc<IKindergartenServices>().Create(dto);

            var deleted = await Svc<IKindergartenServices>().Delete(created.Id.Value);

            Assert.Equal(created.Id, deleted.Id);
        }

        [Fact]
        public async Task ShouldNotDeleteWrongKindergarten_WhenDelete()
        {
            var dto1 = MockKindergartenData();
            var dto2 = MockKindergartenData();

            var create1 = await Svc<IKindergartenServices>().Create(dto1);
            var create2 = await Svc<IKindergartenServices>().Create(dto2);

            var deleted = await Svc<IKindergartenServices>().Delete(create2.Id.Value);

            Assert.NotEqual(create1.Id, deleted.Id);
        }

        [Fact]
        public async Task Should_CreateKindergartenSuccessfully()
        {
            var dto = MockKindergartenData();

            var created = await Svc<IKindergartenServices>().Create(dto);

            Assert.NotNull(created);
            Assert.True(created.Id.HasValue);
        }

        [Fact]
        public async Task Should_DeleteKindergartenSucessfully()
        {
            var dto = MockKindergartenData();

            var created = await Svc<IKindergartenServices>().Create(dto);

            var deleted = await Svc<IKindergartenServices>().Delete(created.Id.Value);

            Assert.NotNull(deleted);
            Assert.Equal(created.Id, deleted.Id);
        }



        private KindergartenDto MockKindergartenData()
        {
            KindergartenDto kindergarten = new()
            {
                GroupName = "TestGrupinimi",
                ChildrenCount = 20,
                KindergartenName = "TestKindergartenNimi",
                TeacherName = "TestTeacherNimi",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            return kindergarten;
        }

        private KindergartenDto MockUpdateKindergartenData()
        {
            KindergartenDto kindergarten = new()
            {
                GroupName = "TestGrupinimeke",
                ChildrenCount = 2,
                KindergartenName = "TestKindergartenNimeke",
                TeacherName = "TestTeacherNimeke",
                CreatedAt = DateTime.Now.AddYears(1),
                UpdatedAt = DateTime.Now.AddYears(1)
            };

            return kindergarten;
        }
    }
}

