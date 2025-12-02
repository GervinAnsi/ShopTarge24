using ShopTarge24.Core.Dto;

namespace ShopTarge24.Core.ServiceInterface
{
    public interface IEmailServices
    {
        void SendEmail(EmailDto dto);
    }
}
