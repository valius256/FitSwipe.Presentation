using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitSwipe.Mobile.MockData
{
  public partial class User : ObservableObject
  {
    [ObservableProperty]
    private string name;
    [ObservableProperty]
    private string occupation;
    [ObservableProperty]
    private int durationPerSection;
    [ObservableProperty]
    private string doB;
    [ObservableProperty]
    private int practiceTime;
  }
}
