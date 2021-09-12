using Android.Content;
using Google.Android.Material.BottomNavigation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Shell), typeof(NetHome.Droid.ShellCustomRenderer))]
namespace NetHome.Droid
{
    public class ShellCustomRenderer : ShellRenderer
    {
        public ShellCustomRenderer(Context context) : base(context) { }

        protected override IShellBottomNavViewAppearanceTracker CreateBottomNavViewAppearanceTracker(ShellItem shellItem)
        {
            return new TabBarCustomAppearance(this, shellItem);
        }

        public class TabBarCustomAppearance : ShellBottomNavViewAppearanceTracker
        {
            public TabBarCustomAppearance(IShellContext shellContext, ShellItem shellItem) : base(shellContext, shellItem) { }

            protected override void SetBackgroundColor(BottomNavigationView bottomView, Color color)
            {
                bottomView.SetBackgroundColor(color.ToAndroid());
            }
        }
    }
}