using FitSwipe.Shared.Models;

namespace FitSwipe.Mobile.Pages.HomePages;

public partial class SwipeMatchView : ContentPage
{
    public List<ViewItem> Items { get; set; } = new List<ViewItem>();
    public SwipeMatchView()
    {
        InitializeComponent();
        Items = new List<ViewItem>()
        {
            new ViewItem {Id = 1, Name = "Jason", Picture="Images/pt1.png", Rating = 4.8, Age = 40, AvatarUrl="Images/pt1.png", Bio="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim "},
            new ViewItem {Id = 2, Name = "Jack", Picture="Images/pt2.png", Rating = 4.8, Age = 40, AvatarUrl="Images/pt2.png", Bio="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim "},
            new ViewItem {Id = 3, Name = "Geogre", Picture="Images/pt3.png", Rating = 4.8, Age = 40, AvatarUrl="Images/pt3.png", Bio="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim "},
            new ViewItem {Id = 4, Name = "Will", Picture = "Images/pt4.png", Rating = 4.8, Age = 40, AvatarUrl = "Images/pt4.png", Bio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim "},
            new ViewItem {Id = 4, Name = "Hambargah", Picture = "Images/pt5.png", Rating = 4.8, Age = 40, AvatarUrl = "Images/pt5.png", Bio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim "},
        };
        BindingContext = this;
    }

    private void OnCurrentItemChanged(object? sender, CurrentItemChangedEventArgs e)
    {
        //matchView.CurrentItemChanged += OnCurrentItemChanged;
    }

    private void btnMatch_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        // Retrieve the bound object via CommandParameter
        var boundItem = button?.CommandParameter as ViewItem;

        if (boundItem != null)
        {
            // Display the name of the corresponding object
            DisplayAlert("Match", $"You matched with: {boundItem.Name}", "OK");
        }
    }

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}