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
        public async Task Login(LoginModel loginModel)
        {
            string json = JsonSerializer.Serialize(loginModel);
            HttpResponseMessage response = await HttpRequestHelper.PostAsync("api/user/login", json);
            Stream stream = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            if (!response.IsSuccessStatusCode)
            {
                string message;
                string reason = response.ReasonPhrase;
                try
                {
                    message = (await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream, options))["Message"];
                }
                catch (JsonException)
                {
                    message = "Login error has occured!";
                }
                throw new ServerException(reason, message);
            }
            LoginResponseModel loginResponse = await JsonSerializer.DeserializeAsync<LoginResponseModel>(stream, options);
            SaveUserInfo(loginResponse.User);
            await SecureStorage.SetAsync("AuthorizationToken", loginResponse.Token);
        }

        public async Task Validate()
        {
            string token = await SecureStorage.GetAsync("AuthorizationToken");
            if (token == null) return;
            HttpResponseMessage response = await HttpRequestHelper.GetAsync("api/user/validate", token);
            Stream stream = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            if (!response.IsSuccessStatusCode)
            {
                ClearUserData();
                string message;
                string reason = response.ReasonPhrase;
                try
                {
                    message = (await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream, options))["Message"];
                }
                catch (JsonException)
                {
                    message = "Validation error has occured! Try loging in again.";
                }
                throw new ServerException(reason, message);
            }
            UserModel userData = await JsonSerializer.DeserializeAsync<UserModel>(stream, options);
            SaveUserInfo(userData);
        }
        
        public async Task Register(RegisterModel registerModel)
        {
            var json = JsonSerializer.Serialize(registerModel);
            HttpResponseMessage response = await HttpRequestHelper.PostAsync("api/user/register", json);
            Stream stream = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            if (!response.IsSuccessStatusCode)
            {
                string message;
                string reason = response.ReasonPhrase;
                try
                {
                    message = (await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream, options))["Message"];
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
            var json = Preferences.Get("UserDataJSON", null);
            return json == null ? null : JsonSerializer.Deserialize<UserModel>(json);
        }

        public void ClearUserData()
        {
            SecureStorage.Remove("AuthorizationToken");
            Preferences.Remove("UserDataJSON");
        }
    }
}
