using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.DAL.Entities;

namespace TideOfDestiniy.BLL.Interfaces
{
    public interface IR2StorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType);
        string GeneratePreSignedUrl(string bucket, string key, TimeSpan expiry);
        Task<GameFile?> GetFileInfoAsync(int id);
        Task<Stream> DownloadFileAsync(string fileName);
        Task<Stream> DownloadFileByKeyAsync(string key);
        Task<(Stream FileStream, string FileName, string ContentType)> DownloadLatestFileAsync();
    }
}
