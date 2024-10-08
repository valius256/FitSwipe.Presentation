using FitSwipe.Shared.Dtos.Tags;
using FitSwipe.Shared.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitSwipe.Mobile.ViewModels.UserProfile
{
  public class UserProfileCombinedViewModel
  {
    public GetUserDto User { get; set; }
    public ObservableCollection<GetTagDto> UserTags { get; set; }

    public UserProfileCombinedViewModel (GetUserDto user, ObservableCollection<GetTagDto> userTags)
    {
      User = user;
      UserTags = userTags;
    }
  }

}
