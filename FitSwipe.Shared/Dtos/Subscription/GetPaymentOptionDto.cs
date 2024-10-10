using System.ComponentModel;

namespace FitSwipe.Shared.Dtos.Subscription
{
  public class GetPaymentOptionDto : INotifyPropertyChanged
  {
    private bool _isSelected;

    public string Name { get; set; }
    public string Description { get; set; }
    public int Index { get; set; }

    public bool IsSelected
    {
      get => _isSelected;
      set
      {
        if (_isSelected != value)
        {
          _isSelected = value;
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
