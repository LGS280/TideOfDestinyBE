using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.BLL.DTOs;
using TideOfDestiniy.BLL.Interfaces;
using TideOfDestiniy.DAL.Entities;
using TideOfDestiniy.DAL.Interfaces;
using TideOfDestiniy.DAL.Repositories;

namespace TideOfDestiniy.BLL.Services
{
    public class NewsService : INewsService
    {
        private INewsRepo _newsRepo;

        public NewsService(INewsRepo newsRepo)
        {
            _newsRepo = newsRepo;
        }
        public async Task<AuthResultDTO> CreateNewsAsync(CreateNewsDTO newsDTO, Guid id)
        {
            var news = new News
            {
                Title = newsDTO.Title,
                Content = newsDTO.Content,
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

        public async Task<List<NewsDTO>> GetAllNewsAsync()
        {
            var newsList = await _newsRepo.GetAllNewsAsync();
            return newsList.Select(n => new NewsDTO
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                PublishedAt = n.PublishedAt,
                AuthorId = n.AuthorId,
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
                Authorname = news.Author != null ? news.Author.Username : "Unknown"
            };
        }

        public async Task<AuthResultDTO> UpdateNewsAsync(UpdateNewsDTO newsDTO, Guid id)
        {
            var news = _newsRepo.GetNewsByIdAsync(id);
            if (news == null)
            {
                return new AuthResultDTO { Succeeded = false, Message = "News not found." };
            }

            await _newsRepo.UpdateNewsAsync(new News
            {
                Title = newsDTO.Title,
                Content = newsDTO.Content,
                PublishedAt = newsDTO.PublishedAt,
                AuthorId = newsDTO.AuthorId,
            });

            return new AuthResultDTO { Succeeded = true, Message = "News updated successfully." };
        }
    }
}
