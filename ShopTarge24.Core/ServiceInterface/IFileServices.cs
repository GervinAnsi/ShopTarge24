using System.Xml;
using ShopTarge24.Core.Domain;
using ShopTarge24.Core.Dto;

namespace ShopTarge24.Core.ServiceInterface
{
    public interface IFileServices
    {
        void FilesToApi(SpaceshipDto dto, Spaceships domain);
        void FilesToApi(KindergartenDto dto, Kindergarten domain);
        Task<FileToApi> RemoveImageFromApi(FileToApiDto dto);
        Task<List<FileToApi>> RemoveImagesFromApi(FileToApiDto[] dtos);
        void UploadFilesToDatabase(RealEstateDto dto, RealEstate domain);
    }
}
