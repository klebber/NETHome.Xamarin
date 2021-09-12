using Android.App;
using Android.OS;
using Android.Views;
using NetHome.Helpers;
using System.Drawing;
using Xamarin.Essentials;

namespace NetHome.Droid
{
    public class Environment : IEnvironment
    {
        public void SetNavBarColor(Color navColor)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
                return;

            Activity activity = Platform.CurrentActivity;
            Window window = activity.Window;
            window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            window.SetNavigationBarColor(navColor.ToPlatformColor());
        }

        public void SetStatusBarColor(Color statusColor, bool darkStatusBar)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
                return;

            Activity activity = Platform.CurrentActivity;
            Window window = activity.Window;
            window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            window.ClearFlags(WindowManagerFlags.TranslucentStatus);
            window.SetStatusBarColor(statusColor.ToPlatformColor());
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                window.SetDecorFitsSystemWindows(darkStatusBar);
            }
        }
    }
}