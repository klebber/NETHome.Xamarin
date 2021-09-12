using System.Drawing;

namespace NetHome.Helpers
{
    public interface IEnvironment
    {
        void SetStatusBarColor(Color statusColor, bool darkStatusBar);
        void SetNavBarColor(Color navColor);
    }
}
