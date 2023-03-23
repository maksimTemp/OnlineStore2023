using System.Windows;

namespace OnlineStore2023.Utilities
{
    public static class VisibilitySupport
    {
        public static Visibility ChangeVisibility(Visibility uiElementVisibility)
        {
            return uiElementVisibility == Visibility.Visible // condition
                                          ? Visibility.Collapsed // if-case
                                          : Visibility.Visible;  // else-case
        }
    }
}
