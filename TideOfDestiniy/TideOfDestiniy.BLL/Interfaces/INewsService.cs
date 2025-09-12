using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.BLL.DTOs;

namespace TideOfDestiniy.BLL.Interfaces
{
    public interface INewsService
    {
        Task<List<NewsDTO>> GetAllNewsAsync();
        Task<AuthResultDTO> CreateNewsAsync(CreateNewsDTO newsDTO, Guid id);
        Task<AuthResultDTO> UpdateNewsAsync(UpdateNewsDTO newsDTO, Guid id);

        Task<AuthResultDTO> DeleteNewsAsync(Guid id);

        Task<NewsDTO> GetNewsById(Guid id);
    }
}
