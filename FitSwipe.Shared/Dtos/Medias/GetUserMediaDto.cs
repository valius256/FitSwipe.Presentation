using FitSwipe.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitSwipe.Shared.Dtos.Medias
{
    public class GetUserMediaDto
    {
        public string UserId { get; set; } = string.Empty;
        public string MediaUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsVideo { get; set; } = false;
        public RequestStatus Status { get; set; }
    }
}
