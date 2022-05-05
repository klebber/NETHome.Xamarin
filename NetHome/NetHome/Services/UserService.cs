using NetHome.Common;
using NetHome.Common.Models;
using NetHome.Helpers;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NetHome.Services
{
    public class UserService : IUserService
    {
        private readonly IWebSocketService _webSocketService;

        public UserService()
        {
            _webSocketService = DependencyService.Get<IWebSocketService>();
        }

        public async Task Login(LoginRequest loginRequest)
        {
            string json = JsonSerializer.Serialize(loginRequest);
            HttpResponseMessage response = await HttpRequestHelper.PostUnauthorizedAsync("api/user/login", json);
            Stream stream = await response.Content.ReadAsStreamAsync();
            LoginResponse loginResponse = await JsonSerializer.DeserializeAsync<LoginResponse>(stream, JsonHelper.GetOptions());
            UserDataManager.SaveUserData(loginResponse.User);
            await UserDataManager.SaveAuthorizationToken(loginResponse.Token);
            await _webSocketService.ConnectAsync();
        }

        public async Task Validate()
        {
            if (await UserDataManager.GetAuthorizationToken() is null) return;
            HttpResponseMessage response = await HttpRequestHelper.GetAsync("api/user/validate");
            Stream stream = await response.Content.ReadAsStreamAsync();
            UserModel userData = await JsonSerializer.DeserializeAsync<UserModel>(stream, JsonHelper.GetOptions());
            UserDataManager.SaveUserData(userData);
            await _webSocketService.ConnectAsync();
        }

        public async Task Register(RegisterRequest registerRequest)
        {
            var json = JsonSerializer.Serialize(registerRequest);
            await HttpRequestHelper.PostUnauthorizedAsync("api/user/register", json);
        }

        public async Task Logout()
        {
            UserDataManager.ClearUserData();
            UserDataManager.ClearAuthorizationToken();
            await _webSocketService.CloseAsync();
        }
    }
}
