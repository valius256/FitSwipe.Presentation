using CommunityToolkit.Mvvm.ComponentModel;
using FitSwipe.Shared.Dtos.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitSwipe.Mobile.ViewModels
{
  public partial class SubscriptionPaymentResultViewModel : ObservableObject
  {
    [ObservableProperty]
    private GetPaymentResultDto paymentResult;

    public SubscriptionPaymentResultViewModel (GetPaymentResultDto paymentResult)
    {
      PaymentResult = paymentResult ?? throw new ArgumentNullException(nameof(paymentResult));
    }
  }
}
