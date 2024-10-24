using FitSwipe.Shared.Enums;

namespace FitSwipe.Shared.Dtos.Users
{
    public class GetUserSubscriptionDto
    {
        public string FireBaseId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime? SubscriptionPurchasedDate { get; set; }
        public int? SubscriptionLevel { get; set; }
        public PaymentStatus? SubscriptionPaymentStatus { get; set; }
    }
}
