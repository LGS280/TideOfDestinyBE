using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TideOfDestiniy.BLL.DTOs.Requests
{
    public class PaymentRequest
    {
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}
