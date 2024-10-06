using FitSwipe.Mobile.Controls;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.UploadAvatar;

public partial class TraineeUploadImageVideo : ContentPage
{

    // Declare ImageItems as a class-level property
    public ObservableCollection<ImageItem> ImageItems { get; set; }


    public TraineeUploadImageVideo()
	{
		InitializeComponent();
        // Initialize the ImageItems collection
        ImageItems = new ObservableCollection<ImageItem>();

        // Bind the ImageItems collection to the CollectionView
        imageCollectionView.ItemsSource = ImageItems;
    }

    private async void NextPageAvtar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TraineeUploadAvatar());
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {

        try
        {
            var result = await MediaPicker.PickPhotoAsync();
            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                var imageSource = ImageSource.FromStream(() => stream);

                // Add the new image to the collection
                ImageItems.Add(new ImageItem { ImageSource = imageSource });
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., display an alert)
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void cameraPhotoButton_Clicked(object sender, EventArgs e)
    {

        try
        {
            var result = await MediaPicker.CapturePhotoAsync();
            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                var imageSource = ImageSource.FromStream(() => stream);

                // Add the new image to the collection
                ImageItems.Add(new ImageItem { ImageSource = imageSource });
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions (e.g., display an alert)
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void done_Clicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new UserUploadAvatar());
    }

    public class ImageItem
    {
        public ImageSource ImageSource { get; set; }
    }
}


