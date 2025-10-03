using Amazon.S3.Model;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.BLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using TideOfDestiniy.DAL.Entities;
using TideOfDestiniy.DAL.Context;

namespace TideOfDestiniy.BLL.Services
{
    public class R2StorageService : IR2StorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly TideOfDestinyDbContext _dbContext;
        private readonly string _bucketName;

        public R2StorageService(IAmazonS3 s3Client, TideOfDestinyDbContext dbContext, IConfiguration config)
        {
            _s3Client = s3Client;
            _dbContext = dbContext;
            _bucketName = config["R2Storage:BucketName"]!;
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName,
                InputStream = fileStream,
                ContentType = contentType
            };

            await _s3Client.PutObjectAsync(request);

            // Trả về public URL
            return $"https://{_bucketName}.r2.cloudflarestorage.com/{fileName}";
        }
        public string GeneratePreSignedUrl(string bucket, string key, TimeSpan expiry)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = bucket,
                Key = key,
                Expires = DateTime.UtcNow.Add(expiry)
            };
            return _s3Client.GetPreSignedURL(request);
        }
        public async Task<GameFile?> GetFileInfoAsync(int id)
        {
            return await _dbContext.GameFiles.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = fileName
            };

            var response = await _s3Client.GetObjectAsync(request);
            return response.ResponseStream;
        }

        public async Task<Stream> DownloadFileByKeyAsync(string key)
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = key
            };

            var response = await _s3Client.GetObjectAsync(request);
            return response.ResponseStream;
        }
    }
}
