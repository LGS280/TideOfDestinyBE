using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.BLL.DTOs.Responses;

namespace TideOfDestiniy.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDTO> LoginWithGoogleAsync(GoogleLoginDTO googleLoginDto);

    }
}
