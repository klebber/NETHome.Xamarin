using NetHome.Common;
using NetHome.Common.Models;
using NetHome.Helpers;
using System.Collections.Generic;
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
            HttpResponseMessage response = await HttpRequestHelper.PostAsync("api/user/login", json);
            Stream stream = await response.Content.ReadAsStreamAsync();
            if (!response.IsSuccessStatusCode)
            {
                string message;
                string reason = response.ReasonPhrase;
                try
                {
                    message = (await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream, JsonHelper.GetOptions()))["Message"];
                }
                catch (JsonException)
                {
                    message = "Login error has occured!";
                }
                throw new ServerException(reason, message);
            }
            LoginResponse loginResponse = await JsonSerializer.DeserializeAsync<LoginResponse>(stream, JsonHelper.GetOptions());
            SaveUserInfo(loginResponse.User);
            await SecureStorage.SetAsync("AuthorizationToken", loginResponse.Token);
        }

        public async Task Validate()
        {
            string token = await SecureStorage.GetAsync("AuthorizationToken");
            if (token is null) return;
            HttpResponseMessage response = await HttpRequestHelper.GetAsync("api/user/validate", token);
            Stream stream = await response.Content.ReadAsStreamAsync();
            if (!response.IsSuccessStatusCode)
            {
                ClearUserData();
                string message;
                string reason = response.ReasonPhrase;
                try
                {
                    message = (await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream, JsonHelper.GetOptions()))["Message"];
                }
                catch (JsonException)
                {
                    message = "Validation error has occured! Try loging in again.";
                }
                throw new ServerException(reason, message);
            }
            UserModel userData = await JsonSerializer.DeserializeAsync<UserModel>(stream, JsonHelper.GetOptions());
            SaveUserInfo(userData);
        }
        
        public async Task Register(RegisterRequest registerRequest)
        {
            var json = JsonSerializer.Serialize(registerRequest);
            HttpResponseMessage response = await HttpRequestHelper.PostAsync("api/user/register", json);
            Stream stream = await response.Content.ReadAsStreamAsync();
            if (!response.IsSuccessStatusCode)
            {
                string message;
                string reason = response.ReasonPhrase;
                try
                {
                    message = (await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream, JsonHelper.GetOptions()))["Message"];
                }
                catch (JsonException)
                {
                    message = "Registration error has occured!";
                }
                throw new ServerException(reason, message);
            }
        }

        private void SaveUserInfo(UserModel user)
        {
            Preferences.Remove("UserDataJSON");
            Preferences.Set("UserDataJSON", JsonSerializer.Serialize(user));
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
