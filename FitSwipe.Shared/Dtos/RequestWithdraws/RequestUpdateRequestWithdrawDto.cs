using FitSwipe.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitSwipe.Shared.Dtos.RequestWithdraw
{
    public class RequestUpdateRequestWithdrawDto
    {
        [Required]public required Guid Id { get; set; }
        [Required] public required string OperatorMessage { get; set; } = string.Empty;
        [Required] public required RequestStatus Status { get; set; }
    }
}
