using FitSwipe.Shared.Models;
using Syncfusion.Maui.Inputs;

namespace FitSwipe.Mobile.Pages.HomePages;

public partial class PTList : ContentPage
{
    public List<ViewItem> Items { get; set; } = new List<ViewItem>();
    public PTList()
	{
        InitializeComponent();
        Items = new List<ViewItem>()
        {
            new ViewItem {Id = 1, Name = "Jason", Picture="Images/pt1.png", Rating = 4.8, Age = 40, AvatarUrl="Images/pt1.png", Bio="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim "},
            new ViewItem {Id = 2, Name = "Jack", Picture="Images/pt2.png", Rating = 3, Age = 40, AvatarUrl="Images/pt2.png", Bio="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim "},
            new ViewItem {Id = 3, Name = "Geogre", Picture="Images/pt3.png", Rating = 4.8, Age = 40, AvatarUrl="Images/pt3.png", Bio="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim "},
            new ViewItem {Id = 4, Name = "Will", Picture = "Images/pt4.png", Rating = 4.8, Age = 40, AvatarUrl = "Images/pt4.png", Bio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim "},
            new ViewItem {Id = 4, Name = "Hambargah", Picture = "Images/pt5.png", Rating = 4.8, Age = 40, AvatarUrl = "Images/pt5.png", Bio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim "},
            new ViewItem {Id = 1, Name = "Jason", Picture="Images/pt1.png", Rating = 2, Age = 40, AvatarUrl="Images/pt1.png", Bio="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim "},
            new ViewItem {Id = 2, Name = "Jack", Picture="Images/pt2.png", Rating = 4.8, Age = 40, AvatarUrl="Images/pt2.png", Bio="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim "},
            new ViewItem {Id = 3, Name = "Geogre", Picture="Images/pt3.png", Rating = 4.8, Age = 40, AvatarUrl="Images/pt3.png", Bio="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim "},
            new ViewItem {Id = 4, Name = "Will", Picture = "Images/pt4.png", Rating = 1, Age = 40, AvatarUrl = "Images/pt4.png", Bio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim "},
            new ViewItem {Id = 4, Name = "Hambargah", Picture = "Images/pt5.png", Rating = 4.8, Age = 40, AvatarUrl = "Images/pt5.png", Bio = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim "},

        };
        BindingContext = this;
    }

    private void btnSwipeMatch_Clicked(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new SwipeMatchView());
        //await Shell.Current.GoToAsync("//SwipeMatchView");
    }

    private void btnMatch_Clicked(object sender, EventArgs e)
    {

    }
}