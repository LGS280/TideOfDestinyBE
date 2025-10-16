
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace TideOfDestiniy.BLL.Hubs
{
    public class NewsHub: Hub
    {
        public override async Task OnConnectedAsync()
        {
            // Có thể log lại khi có client mới kết nối
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Log lại khi client ngắt kết nối
            await base.OnDisconnectedAsync(exception);
        }
    }
}
