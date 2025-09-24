using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.BLL.DTOs;
using TideOfDestiniy.DAL.Entities;

namespace TideOfDestiniy.BLL.Interfaces
{
    public interface INewsService
    {
        Task<List<NewsDTO>> GetAllNewsAsync(NewsCategory? category = null);
        Task<AuthResultDTO> CreateNewsAsync(CreateNewsDTO newsDTO, Guid id);
        Task<AuthResultDTO> UpdateNewsAsync(UpdateNewsDTO newsDTO, Guid id);

        Task<AuthResultDTO> DeleteNewsAsync(Guid id);

        Task<NewsDTO> GetNewsById(Guid id);
    }
}
