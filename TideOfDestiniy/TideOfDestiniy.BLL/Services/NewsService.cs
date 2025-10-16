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
            //string? uploadedImageUrl = null;

            if (string.IsNullOrEmpty(newsDTO.Title) || string.IsNullOrEmpty(newsDTO.Content))
            {
                return new AuthResultDTO { Succeeded = false, Message = "Title and Content cannot be empty." };
            }

            if (string.IsNullOrEmpty(newsDTO.Title))
            {
                return new AuthResultDTO { Succeeded = false, Message = "Title cannot be empty." };
            }
            if(string.IsNullOrEmpty(newsDTO.Content))
            {
                return new AuthResultDTO { Succeeded = false, Message = "Content cannot be empty." };
            }

            var news = new News
            {
                Title = newsDTO.Title,
                Content = newsDTO.Content,
                //ImageUrl = uploadedImageUrl,
                NewsCategory = newsDTO.NewsCategory,
                //PublishedAt = newsDTO.PublishedAt,
                AuthorId = id,
            };

            if (newsDTO.ImageUrl != null && newsDTO.ImageUrl.Any())
            {
                foreach (var imageFile in newsDTO.ImageUrl)
                {
                    var uploadResult = await _photoService.AddPhotoAsync(imageFile); // Giả sử trả về { Succeeded, Url, PublicId }
                    if (uploadResult.Succeeded)
                    {
                        news.Images.Add(new Image
                        {
                            Url = uploadResult.Url,
                            PublicId = uploadResult.PublicId
                        });
                    }
                    else
                    {
                        // Xử lý lỗi: có thể bỏ qua ảnh này hoặc trả về lỗi ngay lập tức
                        return new AuthResultDTO { Succeeded = false, Message = $"Failed to upload image: {uploadResult.ErrorMessage}" };
                    }
                }
            }


            var result = await _newsRepo.CreateNewsAsync(news);
            return result
            ? new AuthResultDTO { Succeeded = true, Message = "News created successfully." }
            
            : new AuthResultDTO { Succeeded = false, Message = "Failed to create news." };

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
                ImageUrls = n.Images.Select(img => new ImageDTO
                {
                    Id = img.Id,
                    Url = img.Url
                }).ToList(),
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
                ImageUrls = news.Images.Select(img => new ImageDTO
                {
                    Id = img.Id,
                    Url = img.Url
                }).ToList(),
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

            // 1. Xóa các ảnh được yêu cầu
            if (newsDTO.DeletedImageIds != null && newsDTO.DeletedImageIds.Any())
            {
                // Lấy các ảnh cần xóa từ DB
                var imagesToDelete = news.Images
                                         .Where(img => newsDTO.DeletedImageIds.Contains(img.Id))
                                         .ToList();

                foreach (var image in imagesToDelete)
                {
                    await _photoService.DeletePhotoAsync(image.PublicId); // Xóa khỏi Cloudinary
                    news.Images.Remove(image); // Xóa khỏi collection (EF sẽ xóa khỏi DB khi SaveChanges)
                }
            }

            // 2. Thêm các ảnh mới
            if (newsDTO.ImageUrl != null && newsDTO.ImageUrl.Any())
            {
                foreach (var imageFile in newsDTO.ImageUrl)
                {
                    var uploadResult = await _photoService.AddPhotoAsync(imageFile);
                    if (uploadResult.Succeeded)
                    {
                        news.Images.Add(new Image { Url = uploadResult.Url, PublicId = uploadResult.PublicId });
                    }
                }
            }

            //// Trường hợp 2: Người dùng muốn xóa ảnh hiện tại (và không upload ảnh mới)
            //else if (newsDTO.RemoveCurrentImage && !string.IsNullOrEmpty(news.ImageUrl))
            //{
            //    var publicId = GetPublicIdFromUrl(news.ImageUrl);
            //    if (publicId != null)
            //    {
            //        await _photoService.DeletePhotoAsync(publicId);
            //    }
            //    finalImageUrl = null; // Xóa URL khỏi database
            //}

            if (string.IsNullOrEmpty(newsDTO.Title))
            {
                return new AuthResultDTO { Succeeded = false, Message = "Title cannot be empty." };
            }

            if(string.IsNullOrEmpty(newsDTO.Content))
            {
                return new AuthResultDTO { Succeeded = false, Message = "Content cannot be empty." };
            }

            news.Title = newsDTO.Title;
            news.Content = newsDTO.Content;
            //news.ImageUrl = finalImageUrl;
            news.NewsCategory = newsDTO.NewsCategory;

            var update = await _newsRepo.UpdateNewsAsync(news);
            return update
             ? new AuthResultDTO { Succeeded = true, Message = "News updated successfully." }
             : new AuthResultDTO { Succeeded = false, Message = "Failed to update news." };


        }

        private string? GetPublicIdFromUrl(string url)
        {
            try
            {
                // URL Cloudinary thường có dạng: http://res.cloudinary.com/<cloud_name>/image/upload/<version>/<public_id>.<format>
                var uri = new Uri(url);
                // Lấy phần cuối cùng của đường dẫn và loại bỏ phần mở rộng file
                var publicIdWithFormat = uri.Segments.LastOrDefault();
                if (publicIdWithFormat != null)
                {
                    return Path.GetFileNameWithoutExtension(publicIdWithFormat);
                }
                return null;
            }
            catch
            {
                return null; // URL không hợp lệ
            }
        }
    }
}
