using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using TideOfDestiniy.BLL.Interfaces;
using TideOfDestiniy.DAL.Entities;
using TideOfDestiniy.DAL.Interfaces;

namespace TideOfDestiniy.BLL.Services
{
    public class UploadService : IUploadService
    {
        private readonly IFileRepo _repo;
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly string _publicBaseUrl;

        public UploadService(IFileRepo repo, IConfiguration configuration)
        {
            _repo = repo;
            
            var r2Config = configuration.GetSection("R2Storage");
            var accessKey = r2Config["AccessKeyId"] ?? throw new InvalidOperationException("R2Storage:AccessKeyId is required");
            var secretKey = r2Config["SecretAccessKey"] ?? throw new InvalidOperationException("R2Storage:SecretAccessKey is required");
            var accountId = r2Config["AccountId"] ?? throw new InvalidOperationException("R2Storage:AccountId is required");
            _bucketName = r2Config["BucketName"] ?? throw new InvalidOperationException("R2Storage:BucketName is required");

            _publicBaseUrl = $"https://{_bucketName}.{accountId}.r2.cloudflarestorage.com";

            // Configure S3 client for R2 with proper settings
            _s3Client = new AmazonS3Client(
                accessKey,
                secretKey,
                new AmazonS3Config
                {
                    ServiceURL = $"https://{accountId}.r2.cloudflarestorage.com",
                    AuthenticationRegion = "auto",
                    UseHttp = false,
                    ForcePathStyle = true
                });
        }

        public async Task<GameFile> UploadToR2Async(Stream fileStream, string fileName, string contentType)
        {
            // Copy stream data to memory to avoid disposal issues
            var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            var fileSize = memoryStream.Length;
            var fileBytes = memoryStream.ToArray();
            memoryStream.Dispose();

            try
            {
                // Upload to R2 Cloudflare using AWS SDK
                var putRequest = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    InputStream = new MemoryStream(fileBytes),
                    ContentType = contentType,
                    DisablePayloadSigning = true
                    //ServerSideEncryptionMethod = ServerSideEncryptionMethod.None // Disable encryption for R2
                };

                await _s3Client.PutObjectAsync(putRequest);

                // Create public URL for R2Amazon.S3.AmazonS3Exception: 'The request signature we calculated does not match the signature you provided. Check your secret access key and signing method.'

                string downloadUrl = $"{_publicBaseUrl}/{fileName}";

                // Save metadata to DB
                var gameFile = new GameFile
                {
                    FileName = fileName,
                    DownloadUrl = downloadUrl,
                    Size = fileSize,
                    ContentType = contentType,
                    UploadedAt = DateTime.UtcNow
                };

                await _repo.AddAsync(gameFile);
                return gameFile;
            }
            catch (Exception ex)
            {
                // If R2 upload fails, throw the exception instead of falling back to local
                throw new Exception($"Failed to upload to R2 Cloudflare: {ex.Message}", ex);
            }
        }

        

        public void Dispose()
        {
            _s3Client?.Dispose();
        }

        public async Task<List<S3Object>> GetAllFilesAsync()
        {
            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName
            };

            var response = await _s3Client.ListObjectsV2Async(request);
            return response.S3Objects;
        }
    }
}