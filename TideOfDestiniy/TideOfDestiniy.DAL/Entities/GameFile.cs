using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TideOfDestiniy.DAL.Entities
{
    public class GameFile
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty; // URL hoặc đường dẫn local
        public long FileSize { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
