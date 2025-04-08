using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MobileShopInMVC.Models;
namespace MobileShopInMVC.Service
{
  public interface IPaymentService
    {
         Task<MerchantOrder> ProcessMerchantOrder(PaymentRequest payRequest);
         Task<string> CompleteOrderProcess(IHttpContextAccessor _httpContextAccessor);
    }
}
