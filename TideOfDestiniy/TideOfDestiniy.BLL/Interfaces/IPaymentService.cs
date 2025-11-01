using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TideOfDestiniy.BLL.DTOs.Requests;

namespace TideOfDestiniy.BLL.Interfaces
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentLink(Guid userId, Guid productId, string returnUrl, string cancelUrl);

        Task HandleWebhook(WebhookData webhookData);
        Task<bool> ConfirmPayment(long orderCode);


    }
}
