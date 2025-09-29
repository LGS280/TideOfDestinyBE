using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.DAL.Entities;

namespace TideOfDestiniy.BLL.Interfaces
{
    public interface IUploadService
    {
        Task<GameFile> SaveFileAsync(IFormFile file);
    }
}
