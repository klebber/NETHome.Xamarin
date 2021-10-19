using NetHome.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NetHome.ViewModels
{
    public class HomeViewModel
    {
        private readonly IUserService _userService;
        private readonly ISignalRConnection _signalRConnection;

        public HomeViewModel()
        {
            _userService = DependencyService.Get<IUserService>();
            _signalRConnection = DependencyService.Get<ISignalRConnection>();
        }
        public void OnAppearing()
        {
        }
        internal async Task SwitchAsync(bool ison)
        {
            await _signalRConnection.Switch(ison);
        }

        internal void Disconnect()
        {
            _ = _signalRConnection.Disconnect();
        }
    }
}
