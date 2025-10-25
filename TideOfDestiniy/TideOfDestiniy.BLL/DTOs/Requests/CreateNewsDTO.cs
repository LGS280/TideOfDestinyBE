using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.DAL.Entities;

namespace TideOfDestiniy.BLL.DTOs.Requests
{
    public class CreateNewsDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        //public Guid AuthorId { get; set; }
        public IFormFile? ImageUrl { get; set; }
        public NewsCategory NewsCategory { get; set; }
        //public string? Version { get; set; } // Phiên bản game liên quan
    }
}
