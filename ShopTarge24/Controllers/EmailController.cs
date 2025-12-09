using Microsoft.AspNetCore.Mvc;
using ShopTarge24.ApplicationServices.Services;
using ShopTarge24.Core.ServiceInterface;
using ShopTarge24.Models.Email;

namespace ShopTarge24.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailServices _emailServices;

        public EmailController
            (
            IEmailServices emailServices
            )
        {
            _emailServices = emailServices;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendEmail(EmailViewModel vm)
        {
            var files = Request.Form.Files.Any() ? Request.Form.Files.ToList() : new List<IFormFile>();

            var emailDto = new Core.Dto.EmailDto()
            {
                To = vm.To,
                Subject = vm.Subject,
                Body = vm.Body,
                Attachment = files
            };
            _emailServices.SendEmail(emailDto);
            return RedirectToAction(nameof(Index));
        }
    }
}
