using NetHome.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NetHome.ViewModels
{
    public class HomeViewModel
    {
        private readonly IServerConnection _serverConnection;
        public HomeViewModel()
        {
            _serverConnection = DependencyService.Get<IServerConnection>();
        }

        internal async Task SwitchAsync(bool ison)
        {
            await _serverConnection.Switch(ison);
        }

        internal void Connect()
        {
            _ = _serverConnection.Connect();
        }

        internal void Disconnect()
        {
            _ = _serverConnection.Disconnect();
        }
    }
}
