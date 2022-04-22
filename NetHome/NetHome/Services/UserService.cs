using NetHome.Common;
using NetHome.Common.Models;
using NetHome.Helpers;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace NetHome.Services
{
    public class UserService : IUserService
    {
        public async Task Login(LoginRequest loginRequest)
        {
            string json = JsonSerializer.Serialize(loginRequest);
            HttpResponseMessage response = await HttpRequestHelper.PostUnauthorizedAsync("api/user/login", json);
            Stream stream = await response.Content.ReadAsStreamAsync();
            LoginResponse loginResponse = await JsonSerializer.DeserializeAsync<LoginResponse>(stream, JsonHelper.GetOptions());
            SaveUserData(loginResponse.User);
            await SaveAuthorizationToken(loginResponse.Token);
        }

        public async Task Validate()
        {
            if (await GetAuthorizationToken() is null) return;
            HttpResponseMessage response = await HttpRequestHelper.GetAsync("api/user/validate");
            Stream stream = await response.Content.ReadAsStreamAsync();
            UserModel userData = await JsonSerializer.DeserializeAsync<UserModel>(stream, JsonHelper.GetOptions());
            SaveUserData(userData);
        }

        public async Task Register(RegisterRequest registerRequest)
        {
            var json = JsonSerializer.Serialize(registerRequest);
            await HttpRequestHelper.PostAsync("api/user/register", json);
        }

        private void SaveUserData(UserModel user)
        {
            Preferences.Remove("UserDataJSON");
            Preferences.Set("UserDataJSON", JsonSerializer.Serialize(user));
        }

        private async Task SaveAuthorizationToken(string token)
        {
            await SecureStorage.SetAsync("AuthorizationToken", "Bearer " + token);
        }

        public static async Task<string> GetAuthorizationToken()
        {
            return await SecureStorage.GetAsync("AuthorizationToken");
        }

        public UserModel GetUserData()
        {
            string json = Preferences.Get("UserDataJSON", null);
            return json is not null ? JsonSerializer.Deserialize<UserModel>(json) : null;
        }

        public void ClearUserData()
        {
            SecureStorage.Remove("AuthorizationToken");
            Preferences.Remove("UserDataJSON");
        }
    }
}
