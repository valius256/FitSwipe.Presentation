
namespace FitSwipe.Mobile.Extensions
{
    // Same extension method for animation as before
    public static class AnimationExtensions
    {
        public static void WidthRequestTo(this VisualElement element, double toWidth, uint duration)
        {
            double fromWidth = element.WidthRequest;

            element.Animate("BorderAnimation", new Animation(v =>
            {
                element.WidthRequest = fromWidth + (toWidth - fromWidth) * v;
            }), length: duration);
        }
    }
}
