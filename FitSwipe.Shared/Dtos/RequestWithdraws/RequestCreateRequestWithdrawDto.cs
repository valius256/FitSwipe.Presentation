
using FitSwipe.Shared.Enum;
using System.ComponentModel.DataAnnotations;

namespace FitSwipe.Shared.Dtos.RequestWithdraw
{
    public class RequestCreateRequestWithdrawDto
    {
        public string? Reason { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string ReceiverName { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
    }
}
