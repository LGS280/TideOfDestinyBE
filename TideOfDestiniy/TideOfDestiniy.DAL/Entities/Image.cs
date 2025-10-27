using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TideOfDestiniy.DAL.Entities
{
    public class Image
    {
        public Guid Id { get; set; }

        //[Required]
        public string Url { get; set; }
        public string PublicId { get; set; }


        public string? AltText { get; set; } 

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public Guid NewsId { get; set; }

        [ForeignKey("NewsId")]
        public News News { get; set; }
    }
}
