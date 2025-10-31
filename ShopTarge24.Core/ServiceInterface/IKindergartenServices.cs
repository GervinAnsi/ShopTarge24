using ShopTarge24.Core.Domain;
using ShopTarge24.Core.Dto;

namespace ShopTarge24.Core.ServiceInterface
{
    public interface IKindergartenServices
    {
        Task<Kindergarten> Create(KindergartenDto dto);

        Task<Kindergarten> DetailAsync(Guid Id);

        Task<Kindergarten?> Delete(Guid id);

        Task<Kindergarten> Update(KindergartenDto dto);
    }
}
