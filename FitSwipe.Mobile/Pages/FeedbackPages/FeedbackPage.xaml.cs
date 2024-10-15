using FitSwipe.Mobile.Controls;
using FitSwipe.Mobile.Utils;
using FitSwipe.Mobile.ViewModels;
using FitSwipe.Shared.Dtos.Trainings;
using FitSwipe.Shared.Utils;
using System.Collections.ObjectModel;

namespace FitSwipe.Mobile.Pages.FeedbackPages;

public partial class FeeedbackPage : ContentPage
{
    private GetTrainingWithTraineeAndPTDto _trainingDetailDto = new();
    private MyPTListPageViewModel? _viewModel;
    private ObservableCollection<ImageItem> _imageItems = [];
    // Declare ImageItems as a class-level property
    public ObservableCollection<ImageItem> ImageItems { 
        get => _imageItems; 
        set
        {
            _imageItems = value;
            OnPropertyChanged(nameof(ImageItems));
        } 
    }
    private string _feedback = string.Empty;
    private int _rating = 0;
    public string Feedback
    {
        get => _feedback;
        set
        {
            _feedback = value;
            OnPropertyChanged(nameof(Feedback));
        }
    }
    public int Rating
    {
        get => _rating;
        set
        {
            _rating = value;
            OnPropertyChanged(nameof(Rating));
        }
    }
    public GetTrainingWithTraineeAndPTDto TrainingDetailDto
    {
        get => _trainingDetailDto;
        set
        {
            _trainingDetailDto = value;
            OnPropertyChanged(nameof(TrainingDetailDto));
        }
    }

    public FeeedbackPage(GetTrainingWithTraineeAndPTDto getTrainingDetailDto, MyPTListPageViewModel? myPTListPageViewModel = null)
    {
        InitializeComponent();
        TrainingDetailDto = getTrainingDetailDto;
        _viewModel = myPTListPageViewModel;
        // Bind the ImageItems collection to the CollectionView
        imageCollectionView.ItemsSource = ImageItems;
        BindingContext = this;

    }
    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        if (_viewModel != null)
        {
            await _viewModel.FetchData();
            await _viewModel.HandleSwitchTab();
        }

        while (Shell.Current.Navigation.ModalStack.Count > 0)
        {
            await Shell.Current.Navigation.PopModalAsync(true);
        }

    }
    private async void Button_Clicked(object sender, EventArgs e)
    {

        try
        {
            var result = await MediaPicker.PickPhotoAsync();
            if (result != null)
            {
                using (var stream = await result.OpenReadAsync())
                {
                    // Read the stream into a byte array
                    var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    var imageBytes = memoryStream.ToArray();

                    // Create a new ImageSource from the byte array
                    var imageSource = ImageSource.FromStream(() => new MemoryStream(imageBytes));

                    // Add the new image to the collection
                    ImageItems.Add(new ImageItem
                    {
                        ImageSource = imageSource,
                        FilePath = result.FullPath,
                    });
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Lỗi", ex.Message, "OK");
        }
    }
    private void starRating_StarChanged(object sender, EventArgs e)
    {
        var starControl = (StarRatingControl)sender;
        if (starControl != null)
        {
            Rating = (int)starControl.Rating;
        }
    }

    private void btnRemove_Clicked(object sender, EventArgs e)
    {
        var button = sender as ImageButton;
        if (button != null) 
        {
            var parameter = button.CommandParameter as ImageItem;
            if (parameter != null)
            {
                ImageItems.Remove(parameter);                
            }
        }
    }

    private async void btSubmit_Clicked(object sender, EventArgs e)
    {
        if (!loadingDialog.IsVisible)
        {
            return;
        }
        var answer = await DisplayAlert("Gửi đánh giá", "Bạn có chắc chắm về hành động không?", "Có","Không");
        if (answer)
        {
            loadingDialog.IsVisible = true;
            try
            {
                var token = await SecureStorage.GetAsync("auth_token");
                if (token == null)
                {
                    throw new Exception("Lỗi xác thực");
                }
                var iamgeUrls = new List<string>();
                foreach (var image in ImageItems)
                {
                    var uploadResult = await MauiUtils.UploadImageAsync(new FileResult(image.FilePath), token);
                    if (uploadResult != null)
                    {
                        iamgeUrls.Add(uploadResult.FileUrl);
                    }
                }
                await Fetcher.PostAsync("api/trainings/rating", new RequestFeedbackTrainingDto
                {
                    Feedback = Feedback,
                    Rating = Rating,
                    ImageUrls = iamgeUrls,
                    TrainingId = TrainingDetailDto.Id
                }, token);
                if (_viewModel != null)
                {
                    await _viewModel.FetchData();
                    await _viewModel.HandleSwitchTab();
                }
                await Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", "Có lỗi xảy ra : " + ex.Message, "OK");
            }
            loadingDialog.IsVisible = false;
        }
    }
}

public class ImageItem
{
    public required ImageSource ImageSource { get; set; }
    public required string FilePath { get; set; }
}
