
using ShopTarge24.Core.Domain;
using ShopTarge24.Core.Dto;

namespace ShopTarge24.Core.ServiceInterface
{
    public interface ISpaceshipServices
    {
        Task<Spaceships> Create(SpaceshipDto dto);

        Task<Spaceships> DetailAsync(Guid Id);

        Task<Spaceships?> Delete(Guid id);
    }
}
