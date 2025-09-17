using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.BLL.Interfaces;
using TideOfDestiniy.DAL.Entities;
using TideOfDestiniy.DAL.Interfaces;
using TideOfDestiniy.DAL.Repositories;

namespace TideOfDestiniy.BLL.Services
{
    public class DownloadGameService : IDownloadGameService
    {
        private readonly IFileRepo _fileRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DownloadGameService(IFileRepo fileRepo, IWebHostEnvironment webHostEnvironment)
        {
            _fileRepo = fileRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        public GameFile? GetFileById(int id)
        {
            return _fileRepo.GetAll().FirstOrDefault(f => f.Id == id);
        }

        public string GetPhysicalPath(GameFile file)
        {
            return Path.Combine(_webHostEnvironment.WebRootPath, "downloads", file.FileName);
        }
    }
}
