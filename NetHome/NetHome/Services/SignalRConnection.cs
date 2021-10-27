using Microsoft.AspNetCore.SignalR.Client;
using NetHome.Helpers;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NetHome.Services
{
    public class SignalRConnection : ISignalRConnection
    {
        private HubConnection hubConnection;
        public async Task Connect()
        {
            if (hubConnection is not null && hubConnection.State == HubConnectionState.Connected)
                return;
            string uri = Preferences.Get("ServerAddress", null);
            if (uri is null) throw new ServerException("Server adress not found!", "Please enter valid server adress and try again.");
            uri = new Uri(uri).AbsoluteUri;
            hubConnection = new HubConnectionBuilder()
                .WithUrl(uri + "nethomehub", options =>
                {
                    options.AccessTokenProvider = () => SecureStorage.GetAsync("AuthorizationToken");
                })
                .Build();

            hubConnection.KeepAliveInterval = TimeSpan.FromSeconds(3);
            hubConnection.ServerTimeout = TimeSpan.FromSeconds(6);

            hubConnection.Reconnecting += HubReconnecting;
            hubConnection.Reconnected += HubReconnected;
            hubConnection.Closed += HubClosed;

            //hubConnection.On<bool>("Switched", (ison) =>
            //{
            //    MessagingCenter.Send<object, bool>(this, "Switched", ison);
            //});

            try
            {
                await hubConnection.StartAsync();
            }
            catch (HttpRequestException e)
            {
                throw new ServerException("Hub connection error!", e.Message);
            }
            if (hubConnection.State != HubConnectionState.Connected)
                throw new ServerException("Hub connection error!", "Unable to connect to SignalR hub.");
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
