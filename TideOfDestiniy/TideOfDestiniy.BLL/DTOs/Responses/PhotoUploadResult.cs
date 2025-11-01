using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TideOfDestiniy.BLL.DTOs.Responses
{
    public class PhotoUploadResult
    {
        public bool Succeeded { get; set; }
        public string? PublicId { get; set; }
        public string? Url { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
