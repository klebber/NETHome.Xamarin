using NetHome.Common.Models;
using NetHome.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NetHome.Services
{
    public class WebSocketService : IWebSocketService
    {
        private ClientWebSocket _client;
        private readonly IDeviceStateService _stateService;
        private CancellationTokenSource _tokenSource;

        public WebSocketService()
        {
            _stateService = DependencyService.Get<IDeviceStateService>();
        }

        public async Task ConnectAsync()
        {
            _client = new ClientWebSocket();
            _tokenSource = new CancellationTokenSource();
            var uriString = UserDataManager.GetUri();
            var uriBuilder = new UriBuilder(uriString) { Scheme = "ws" };
            var token = await UserDataManager.GetAuthorizationToken();
            _client.Options.SetRequestHeader("Authorization", token);
            _client.Options.KeepAliveInterval = TimeSpan.FromSeconds(5);
            await _client.ConnectAsync(uriBuilder.Uri, CancellationToken.None);

            if (_client.State == WebSocketState.Open)
                _ = Recieving(_tokenSource.Token);
        }

        public async Task CloseAsync()
        {
            if (_client.State is WebSocketState.Open or WebSocketState.CloseReceived)
            {
                await _client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "Socket close requested from client.", CancellationToken.None);
            }
            else if (!_tokenSource.IsCancellationRequested)
            {
                _tokenSource.Cancel();
            }
        }

        private async Task Recieving(CancellationToken _cancelationToken)
        {
            var buffer = new byte[1024 * 4];

            while (_client.State == WebSocketState.Open)
            {
                try
                {
                    var result = await _client.ReceiveAsync(new ArraySegment<byte>(buffer), _cancelationToken);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        var device = JsonSerializer.Deserialize<DeviceModel>(json, JsonHelper.GetOptions());
                        _stateService.DeviceStateChanged(device);
                    }

                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "WebSocket Close recieved!", CancellationToken.None);
                        break;
                    }
                }
                catch (WebSocketException)
                {
                    await CloseAsync();
                }
            }
        }
    }
}
