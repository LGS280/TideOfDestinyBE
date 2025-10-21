using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.DAL.Entities;

namespace TideOfDestiniy.BLL.Interfaces
{
    public interface IUploadService : IDisposable
    {
        Task<GameFile> UploadToR2Async(Stream fileStream, string fileName, string contentType);

        Task<List<S3Object>> GetAllFilesAsync();
    }
}
