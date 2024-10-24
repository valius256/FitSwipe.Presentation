using FitSwipe.Shared.Dtos.Subscription;
using System.Diagnostics;
using System.Globalization;

namespace FitSwipe.Shared.Dtos.Payment
{
  public class GetPaymentResultDto
  {
    public GetSubscriptionItemDto subscriptionItem { get; set; } = new();
    public string Method { get; set; } = string.Empty;
    //public string paymentCode { get; set; }
    public DateTime paymentDate { get; set; }

    // Date format - monday, 12/12/2024 12am
    public string BalanceLeftString { get; set; } = string.Empty;
    public string getPaymentDateFormatted
    {
      get
      {
        return paymentDate.ToString("dddd, dd/MM/yyyy hh:mm:ss tt", new CultureInfo("vi-VN"));
            }
        }
    }
}
