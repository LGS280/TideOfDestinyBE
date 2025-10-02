using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.BLL.DTOs.Requests;
using TideOfDestiniy.BLL.DTOs.Responses;
using TideOfDestiniy.BLL.Interfaces;
using TideOfDestiniy.DAL.Entities;
using TideOfDestiniy.DAL.Interfaces;
using TideOfDestiniy.DAL.Repositories;

namespace TideOfDestiniy.BLL.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsRepo _newsRepo;
        private readonly IPhotoService _photoService;

        public NewsService(INewsRepo newsRepo, IPhotoService photoService)
        {
            _newsRepo = newsRepo;
            _photoService = photoService;
        }
        public async Task<AuthResultDTO> CreateNewsAsync(CreateNewsDTO newsDTO, Guid id)
        {
            string? uploadedImageUrl = null;

            if (newsDTO.ImageUrl != null && newsDTO.ImageUrl.Length > 0)
            {
                // Nếu có, upload nó lên Cloudinary
                var uploadResult = await _photoService.AddPhotoAsync(newsDTO.ImageUrl);

                // Nếu upload thành công, lấy URL
                if (uploadResult.Succeeded)
                {
                    uploadedImageUrl = uploadResult.Url;
                }
                else
                {
                    // (Tùy chọn) Xử lý lỗi: ném ra exception hoặc bỏ qua việc thêm ảnh
                    // Ở đây chúng ta sẽ bỏ qua và tiếp tục tạo bài viết không có ảnh
                    // logger.LogError("Cloudinary upload failed: {error}", uploadResult.ErrorMessage);
                }
            }

            var news = new News
            {
                Title = newsDTO.Title,
                Content = newsDTO.Content,
                ImageUrl = uploadedImageUrl,
                NewsCategory = newsDTO.NewsCategory,
                //PublishedAt = newsDTO.PublishedAt,
                AuthorId = id,
            };
            var result = await _newsRepo.CreateNewsAsync(news);
            if (result)
            {
                return new AuthResultDTO { Succeeded = true, Message = "News created successfully." };
            }
            return new AuthResultDTO { Succeeded = false, Message = "Failed to create news." };

        }

        public async Task<AuthResultDTO> DeleteNewsAsync(Guid id)
        {
            var result = await _newsRepo.DeleteNewsAsync(id);
            if (!result)
            {
                return new AuthResultDTO { Succeeded = false, Message = "News not found." };
            }

            return new AuthResultDTO { Succeeded = true, Message = "News deleted successfully." };
        }

        public async Task<List<NewsDTO>> GetAllNewsAsync(NewsCategory? category = null)
        {
            var newsList = await _newsRepo.GetAllNewsAsync(category);
            return newsList.Select(n => new NewsDTO
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                PublishedAt = n.PublishedAt,
                ImageUrl = n.ImageUrl,
                AuthorId = n.AuthorId,
                NewsCategory = n.NewsCategory,
                Authorname = n.Author != null ? n.Author.Username : "Unknown"
            }).ToList();
        }
        

        public async Task<NewsDTO> GetNewsById(Guid id)
        {
            var news = await _newsRepo.GetNewsByIdAsync(id);

            if (news == null) return null;
            return new NewsDTO
            {
                Id = news.Id,
                Title = news.Title,
                Content = news.Content,
                PublishedAt = news.PublishedAt,
                AuthorId = news.AuthorId,
                ImageUrl = news.ImageUrl,
                NewsCategory = news.NewsCategory,
                Authorname = news.Author != null ? news.Author.Username : "Unknown"
            };
        }

        public async Task<AuthResultDTO> UpdateNewsAsync(UpdateNewsDTO newsDTO, Guid id)
        {
            var news = await _newsRepo.GetNewsByIdAsync(id);
            if (news == null)
            {
                return new AuthResultDTO { Succeeded = false, Message = "News not found." };
            }

            news.Title = newsDTO.Title;
            news.Content = newsDTO.Content;
            news.ImageUrl = newsDTO.ImageUrl;
            news.NewsCategory = newsDTO.NewsCategory;

            var update = await _newsRepo.UpdateNewsAsync(news);
            if (!update)
            {
                return new AuthResultDTO { Succeeded = false, Message = "Failed to update news." };
            }

            return new AuthResultDTO { Succeeded = true, Message = "News updated successfully." };
        }
    }
}
