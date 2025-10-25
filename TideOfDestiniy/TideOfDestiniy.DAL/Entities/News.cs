using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TideOfDestiniy.DAL.Entities
{
    public class News
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string? ImageUrl { get; set; }

        [StringLength(20)]
        public string? Version { get; set; } // Phiên bản game liên quan

        [Required]
        public NewsCategory NewsCategory { get; set; }

        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

        // Foreign key to the User who authored this news
        public Guid AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public User Author { get; set; }

    }
}
