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
        public string FileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public long Size { get; set; }  // dung lượng file (bytes)
        public string DownloadUrl { get; set; } = null!;
        public DateTime UploadedAt { get; set; }
    }
}
