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
            if (hubConnection != null && hubConnection.State == HubConnectionState.Connected) 
                return true;

            hubConnection = new HubConnectionBuilder()
                .WithUrl("http://192.168.0.100:58332/nethomehub", options =>
                {
                    options.AccessTokenProvider = () => SecureStorage.GetAsync("AuthorizationToken");
                })
                .Build();
            
            hubConnection.KeepAliveInterval = TimeSpan.FromSeconds(3);
            hubConnection.ServerTimeout = TimeSpan.FromSeconds(6);

            hubConnection.Reconnecting += HubReconnecting;
            hubConnection.Reconnected += HubReconnected;
            hubConnection.Closed += HubClosed;

            hubConnection.On<bool>("Switched", (ison) =>
            {
                MessagingCenter.Send<object, bool>(this, "Switched", ison);
            });

            await hubConnection.StartAsync();
            return hubConnection.State == HubConnectionState.Connected;
        }

        private Task HubReconnecting(Exception arg)
        {
            MessagingCenter.Send<object>(this, "HubReconnecting");
            return Task.CompletedTask;
        }

        private Task HubReconnected(string arg)
        {
            MessagingCenter.Send<object>(this, "HubReconnected");
            return Task.CompletedTask;
        }

        private Task HubClosed(Exception arg)
        {
            MessagingCenter.Send<object>(this, "HubClosed");
            return Task.CompletedTask;
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
