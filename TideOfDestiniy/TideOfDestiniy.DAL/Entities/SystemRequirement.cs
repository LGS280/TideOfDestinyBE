using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TideOfDestiniy.DAL.Entities
{
    public enum RequirementType
    {
        Minimum,
        Recommended
    }

    public class SystemRequirement
    {
        public int Id { get; set; }

        [Required]
        public RequirementType Type { get; set; }

        [Required]
        [StringLength(100)]
        public string OS { get; set; }

        [Required]
        [StringLength(150)]
        public string Processor { get; set; }

        [Required]
        [StringLength(50)]
        public string Memory { get; set; }

        [Required]
        [StringLength(150)]
        public string Graphics { get; set; }

        [Required]
        [StringLength(50)]
        public string Storage { get; set; }

        [StringLength(50)]
        public string? DirectXVersion { get; set; }
    }

}
