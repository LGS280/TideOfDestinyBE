using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.BLL.DTOs.Responses;
using TideOfDestiniy.BLL.Interfaces;

namespace TideOfDestiniy.BLL.Services
{
    public class PhotoService : IPhotoService
    {
        private Cloudinary _cloudinary;

        public PhotoService(IConfiguration config, Cloudinary cloudinary)
        {
            // Đọc cấu hình từ appsettings.json và tạo tài khoản Cloudinary
            var account = new Account(
                config["CloudinarySettings:CloudName"],
                config["CloudinarySettings:ApiKey"],
                config["CloudinarySettings:ApiSecret"]
            );
            _cloudinary = new Cloudinary(account);
            _cloudinary = cloudinary;
        }

        public async Task<PhotoUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                // Mở một stream từ file được upload
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.FileName, stream),
                        // (Tùy chọn) Biến đổi ảnh, ví dụ: cắt thành hình vuông 500x500
                        Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                    };

                    // Thực hiện upload lên Cloudinary
                    uploadResult = await _cloudinary.UploadAsync(uploadParams);
                }
            }

            if (uploadResult.Error != null)
            {
                return new PhotoUploadResult { Succeeded = false, ErrorMessage = uploadResult.Error.Message };
            }

            return new PhotoUploadResult
            {
                Succeeded = true,
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.ToString() // Lấy URL an toàn (https)
            };
        }

        public async Task<bool> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            // Trả về true nếu kết quả là "ok"
            return result.Result?.ToLower() == "ok";
        }
    }
}
