using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.DAL.Entities;

namespace TideOfDestiniy.BLL.DTOs.Requests
{
    public class UpdateNewsDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public IFormFile? ImageUrl { get; set; }
        public NewsCategory NewsCategory { get; set; }
        //public bool RemoveCurrentImage { get; set; } = false;

        //public string Version { get; set; } = string.Empty;
        //public DateTime PublishedAt { get; set; }
        //public Guid AuthorId { get; set; }
    }
}
