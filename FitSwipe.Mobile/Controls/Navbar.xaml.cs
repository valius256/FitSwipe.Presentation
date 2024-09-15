using System.ComponentModel;

namespace FitSwipe.Mobile.Controls
{
    public partial class Navbar : ContentView
    {
        public Navbar()
        {
            InitializeComponent();
        }

        private void navHome_Tapped(object sender, TappedEventArgs e)
        {
            Shell.Current.GoToAsync("//PTList");
        }

        private void navChat_Tapped(object sender, TappedEventArgs e)
        {
            Shell.Current.GoToAsync("//ChatPage");

        }

        private void navTraining_Tapped(object sender, TappedEventArgs e)
        {
            Shell.Current.GoToAsync("//TrainingPage");

        }

        private void navSchedule_Tapped(object sender, TappedEventArgs e)
        {
            Shell.Current.GoToAsync("//TraineeSchedulePage");

        }

        private void navProfile_Tapped(object sender, TappedEventArgs e)
        {
            Shell.Current.GoToAsync("//TraineeProfilePage");

        }
    }
}
