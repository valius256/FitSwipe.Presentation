using System.ComponentModel;

namespace FitSwipe.Mobile.Controls
{
    public partial class StarRatingControl : ContentView
    {
        public static readonly BindableProperty RatingProperty =
            BindableProperty.Create(nameof(Rating), typeof(double), typeof(StarRatingControl), 0.0, propertyChanged: OnRatingChanged);
        
        public static readonly BindableProperty SizeProperty =
            BindableProperty.Create(nameof(Size), typeof(int), typeof(StarRatingControl), 0);

        public static readonly BindableProperty ReadOnlyProperty =
            BindableProperty.Create(nameof(ReadOnly), typeof(bool), typeof(StarRatingControl), false);

        public event EventHandler? StarChanged;

        public bool ReadOnly
        {
            get => (bool)GetValue(ReadOnlyProperty);
            set => SetValue(ReadOnlyProperty, value);
        }
        protected virtual void OnStarChanged(EventArgs e)
        {
            // If there are any subscribers, raise the event
            StarChanged?.Invoke(this, e);
        }
        public double Rating
        {
            get => (double)GetValue(RatingProperty);
            set => SetValue(RatingProperty, value);
        }
        public int Size
        {
            get => (int)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public StarRatingControl()
        {
            InitializeComponent();
            UpdateStars();
        }

        private static void OnRatingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (StarRatingControl)bindable;
            control.UpdateStars();
        }

        private void UpdateStars()
        {
            StarImage1 = GetStarImage(Rating, 1);
            StarImage2 = GetStarImage(Rating, 2);
            StarImage3 = GetStarImage(Rating, 3);
            StarImage4 = GetStarImage(Rating, 4);
            StarImage5 = GetStarImage(Rating, 5);
        }

        private string GetStarImage(double rating, int portion)
        {
            return rating >= portion ? "star_filled.png" : (rating >= portion - 0.5 ? "star_half_filled.png" : "star_no_fill.png");
        } 


        private string _starImage1 = string.Empty;
        public string StarImage1
        {
            get => _starImage1;
            set
            {
                _starImage1 = value;
                OnPropertyChanged(nameof(StarImage1)); // Notify UI of the change
            }
        }

        private string _starImage2 = string.Empty;
        public string StarImage2
        {
            get => _starImage2;
            set
            {
                _starImage2 = value;
                OnPropertyChanged(nameof(StarImage2));
            }
        }

        private string _starImage3 = string.Empty;
        public string StarImage3
        {
            get => _starImage3;
            set
            {
                _starImage3 = value;
                OnPropertyChanged(nameof(StarImage3));
            }
        }

        private string _starImage4 = string.Empty;
        public string StarImage4
        {
            get => _starImage4;
            set
            {
                _starImage4 = value;
                OnPropertyChanged(nameof(StarImage4));
            }
        }

        private string _starImage5 = string.Empty;
        public string StarImage5
        {
            get => _starImage5;
            set
            {
                _starImage5 = value;
                OnPropertyChanged(nameof(StarImage5));
            }
        }

        private void tgrStar3_Tapped(object sender, TappedEventArgs e)
        {
            if (!ReadOnly)
            {
                Rating = 3;
                StarChanged?.Invoke(this, e);
            }
            
        }

        private void tgrStar1_Tapped(object sender, TappedEventArgs e)
        {
            if (!ReadOnly)
            {
                Rating = 1;
                StarChanged?.Invoke(this, e);
            }
        }

        private void tgrStar2_Tapped(object sender, TappedEventArgs e)
        {
            if (!ReadOnly)
            {
                Rating = 2;
                StarChanged?.Invoke(this, e);
            }
        }

        private void tgrStar4_Tapped(object sender, TappedEventArgs e)
        {
            if (!ReadOnly)
            {
                Rating = 4;
                StarChanged?.Invoke(this, e);
            }
        }

        private void tgrStar5_Tapped(object sender, TappedEventArgs e)
        {
            if (!ReadOnly)
            {
                Rating = 5;
                StarChanged?.Invoke(this, e);
            }
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
