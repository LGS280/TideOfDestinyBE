using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TideOfDestiniy.BLL.DTOs
{
    public class NewsDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        //public string? ImageUrl { get; set; }
        //public string? Version { get; set; } // Phiên bản game liên quan
        public DateTime PublishedAt { get; set; }
        public Guid AuthorId { get; set; }
        public string Authorname { get; set; } // Thêm trường này để hiển thị tên tác giả
    }
}
