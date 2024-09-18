using FitSwipe.Shared.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FitSwipe.Mobile.Pages.SetupPages;

public partial class SetupProfilePage : ContentPage
{
	private string _mainColor1 = "LimeGreen";
	private string _mainColor2 = "Green";
    private Gender? _selectedGender;
    private DateTime? _dateOfBirth;
    private string? _job;
    private string? _city;
    private string? _district;
    private string? _ward;
    private string? _bio;

    public ObservableCollection<string> Jobs { get; set; } = new ObservableCollection<string>();
    public ObservableCollection<string> Cities { get; set; } = new ObservableCollection<string>();
    public ObservableCollection<string> Districts { get; set; } = new ObservableCollection<string>();
    public ObservableCollection<string> Wards { get; set; } = new ObservableCollection<string>();
    public Gender? SelectedGender
    {
        get { return _selectedGender; }
        set
        {
            if (_selectedGender != value)
            {
                _selectedGender = value;
                OnPropertyChanged(nameof(SelectedGender));
            }
        }
    }


    public DateTime? DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            _dateOfBirth = value;
            OnPropertyChanged();
        }
    }

    public string? Job
    {
        get => _job;
        set
        {
            _job = value;
            OnPropertyChanged();
        }
    }

    public string? City
    {
        get => _city;
        set
        {
            _city = value;
            OnPropertyChanged();
        }
    }

    public string? District
    {
        get => _district;
        set
        {
            _district = value;
            OnPropertyChanged();
        }
    }

    public string? Ward
    {
        get => _ward;
        set
        {
            _ward = value;
            OnPropertyChanged();
        }
    }

    public string? Bio
    {
        get => _bio;
        set
        {
            _bio = value;
            OnPropertyChanged();
        }
    }

    public string MainColor1 {
		get => _mainColor1;
		set
		{
			_mainColor1 = value;
			OnPropertyChanged(nameof(MainColor1));
		}	
	} 
	public string MainColor2 { 
		get => _mainColor2; 
		set
		{
			_mainColor2 = value;
			OnPropertyChanged(nameof(MainColor2));
		}
	} 
    public SetupProfilePage()
	{
		InitializeComponent();
        // Populate Jobs and Cities lists with data
        Jobs = new ObservableCollection<string> { "Sinh viên", "Kinh doanh", "Công nghệ thông tin", "Khác" };
        Cities = new ObservableCollection<string> { "TPHCM", "Hà Nội", "Đà Nẵng" };
        Districts = new ObservableCollection<string> { "Quận 1, Quận 2, Quận 3" };
        Wards = new ObservableCollection<string> { "Phường 1", "Phường 2", "Phưởng 3" };
        JobPicker.ItemsSource = Jobs;
        CityPicker.ItemsSource = Cities;
        WardPicker.ItemsSource = Wards;
        DistrictPicker.ItemsSource = Districts;
        // Initialize with default values
        DateOfBirth = DateTime.Now;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        DisplayAlert("Review",$"Your current info : Bio : {Bio}, Job : {Job}, City : {City}, District : {District}, Ward : {Ward}, Gender : {SelectedGender}","Ok baby");
    }
}