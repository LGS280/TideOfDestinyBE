using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TideOfDestiniy.BLL.DTOs
{
    public class GoogleLoginDTO
    {
        //[JsonPropertyName("idToken")]

        public string idToken { get; set; } = null!;

    }
}
