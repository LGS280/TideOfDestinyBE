using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.DAL.Entities;

namespace TideOfDestiniy.BLL.Interfaces
{
    public interface IAuthorization
    {
        string CreateAccessToken(User user);
        string CreateRefreshToken();
    }
}
