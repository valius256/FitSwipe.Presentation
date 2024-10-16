using System.ComponentModel;

namespace FitSwipe.Shared.Dtos.Subscription
{
  public class GetPaymentOptionDto : INotifyPropertyChanged
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public int Index { get; set; }

    private bool isSelected;
    public bool IsSelected
    {
      get => isSelected;
      set
      {
        if (isSelected != value)
        {
          isSelected = value;
          OnPropertyChanged(nameof(IsSelected));
        }
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged (string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
