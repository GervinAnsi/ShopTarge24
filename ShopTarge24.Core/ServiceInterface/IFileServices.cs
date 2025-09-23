using ShopTarge24.Core.Domain;
using ShopTarge24.Core.Dto;

namespace ShopTarge24.Core.ServiceInterface
{
    public interface IFileServices
    {
        void FilesToApi(SpaceshipDto dto, Spaceships domain);
    }
}