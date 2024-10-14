
using FitSwipe.Shared.Dtos.Trainings;
using System.Globalization;

namespace FitSwipe.Mobile.Converters.TrainingPageConverter
{
    public class TrainingRatingVisibilityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is GetTrainingWithTraineeAndPTDto training)
            {
                if (training.Status == Shared.Enums.TrainingStatus.Finished && string.IsNullOrEmpty(training.Feedback) && training.Rating == null)
                {
                    return true;
                }
            }
            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
