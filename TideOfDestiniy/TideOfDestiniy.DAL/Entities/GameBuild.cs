using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TideOfDestiniy.DAL.Entities
{
    public enum PlatformType
    {
        Windows,
        MacOS,
        Linux
    }

    public class GameBuild
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Version { get; set; }

        [Required]
        public PlatformType Platform { get; set; }

        [Required]
        public string DownloadUrl { get; set; }

        public int FileSizeMB { get; set; }

        public DateTime ReleaseDate { get; set; }

        public bool IsLatest { get; set; } = false;
    }


}
