using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NetHome.Services
{
    public class ServerConnection : IServerConnection
    {
        private HubConnection hubConnection;
        public async Task<bool> Connect()
        {
            if (hubConnection != null && hubConnection.State == HubConnectionState.Connected) return true;

            hubConnection = new HubConnectionBuilder()
                .WithUrl("http://192.168.0.100:34674/nethomehub", options =>
                {
                    options.AccessTokenProvider = () =>
                    {
                        return SecureStorage.GetAsync("AuthorizationToken");
                    };
                })
                .Build();

            _ = hubConnection.On<bool>("Switched", (ison) =>
            {
                MessagingCenter.Send<object, bool>(this, "Switched", ison);
            });
            await hubConnection.StartAsync(); //timeout??
            return hubConnection.State == HubConnectionState.Connected;
        }

        public Task Disconnect()
        {
            return hubConnection?.StopAsync();
        }

        public async Task Switch(bool ison)
        {
            if (hubConnection.State == HubConnectionState.Connected)
            {
                await hubConnection.SendAsync("Switch", ison);
            }
        }
    }
}
