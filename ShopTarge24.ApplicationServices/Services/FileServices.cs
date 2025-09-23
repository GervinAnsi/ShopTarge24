using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using ShopTarge24.Core.Domain;
using ShopTarge24.Core.Dto;
using ShopTarge24.Core.ServiceInterface;

namespace ShopTarge24.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly IHostEnvironment _webHost;

        public FileServices
            (
                IHostEnvironment webHost
            )
        {
            _webHost = webHost;
        }

        public void FilesToApi(SpaceshipDto dto, Spaceships domain)
        {
            if (dto.Files != null && dto.Files.Count > 0)
            {
                if (!Directory.Exists(_webHost.ContentRootPath + "\\multiplteFileUpload\\"))
                {

                }

            }
        }
    }
}
