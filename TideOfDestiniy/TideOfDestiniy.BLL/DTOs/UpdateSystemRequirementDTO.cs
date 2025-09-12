using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TideOfDestiniy.BLL.DTOs
{
    public class UpdateSystemRequirementDTO
    {
        public string OS { get; set; }
        public string Processor { get; set; }
        public string Memory { get; set; }
        public string Graphics { get; set; }
        public string Storage { get; set; }

        public string DirectX { get; set; }

        public string Type { get; set; } // "Minimum" or "Recommended"
    }
}
