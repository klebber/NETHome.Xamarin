using NetHome.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NetHome.ViewModels
{
    public class HomeViewModel
    {
        private readonly IUserService _userService;
        private readonly IServerConnection _serverConnection;

        public HomeViewModel()
        {
            _userService = DependencyService.Get<IUserService>();
            _serverConnection = DependencyService.Get<IServerConnection>();
        }
        public void OnAppearing()
        {
        }
        internal async Task SwitchAsync(bool ison)
        {
            await _serverConnection.Switch(ison);
        }

        internal void Disconnect()
        {
            _ = _serverConnection.Disconnect();
        }
    }
}
