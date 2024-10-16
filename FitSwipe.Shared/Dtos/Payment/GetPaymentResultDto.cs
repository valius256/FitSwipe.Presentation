using FitSwipe.Shared.Dtos.Subscription;
using System.Globalization;

namespace FitSwipe.Shared.Dtos.Payment
{
  public class GetPaymentResultDto
  {
    public GetSubscriptionItemDto subscriptionItem { get; set; }
    public GetPaymentOptionDto paymentOption { get; set; }
    public bool paymentSuccess { get; set; }
    public string paymentMessage { get; set; }
    public string paymentCode { get; set; }
    public DateTime paymentDate { get; set; }

    // Date format - monday, 12/12/2024 12am
    public string getPaymentDateFormatted
    {
      get
      {
        return paymentDate.ToString("dddd, dd/MM/yyyy hh:mm:ss tt", new CultureInfo("vi-VN"));
      }
    }
  }
}
