using FitSwipe.Mobile.ViewModels;
using FitSwipe.Shared.Dtos.Subscription;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace FitSwipe.Mobile.Pages.SubscriptionPages;

public partial class SubscriptionPaymentView : ContentPage
{
  public SubscriptionPaymentView (GetSubscriptionItemDto selectedSubscription, ObservableCollection<GetPaymentOptionDto>? paymentOptions)
  {
    InitializeComponent();
    BindingContext = new SubscriptionViewModel();
    if (selectedSubscription != null)
    {
      ((SubscriptionViewModel)BindingContext).SelectedSubscription = selectedSubscription;
    }
    if (paymentOptions != null)
    {
      ((SubscriptionViewModel)BindingContext).PaymentOptions = paymentOptions;
    }
  }
}