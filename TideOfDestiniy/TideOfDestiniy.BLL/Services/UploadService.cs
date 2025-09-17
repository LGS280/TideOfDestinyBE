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
    public class UploadService : IUploadService
    {
        private readonly IFileRepo _fileRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UploadService(IFileRepo fileRepo, IWebHostEnvironment webHostEnvironment)
        {
            _fileRepo = fileRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        public IEnumerable<GameFile> GetFiles()
        {
            return _fileRepo.GetAll();
        }

        public async Task<GameFile> SaveFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("No file uploaded");

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "downloads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var gameFile = new GameFile
            {
                FileName = file.FileName,
                FilePath = $"/downloads/{file.FileName}"
            };

            _fileRepo.Save(gameFile);

            return gameFile;
        }
    }
}
