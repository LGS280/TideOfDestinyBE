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
        private readonly IFileRepo _repo;
        private readonly IWebHostEnvironment _env;

        public DownloadGameService(IFileRepo repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        public GameFile? GetFileById(int id)
        {
            return _repo.GetById(id);
        }

        public string GetPhysicalPath(GameFile file)
        {
            return Path.Combine(_env.WebRootPath, "downloads", file.FileName);
        }
    }
}
