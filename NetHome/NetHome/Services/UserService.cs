using NetHome.Common.Models;
using NetHome.Helpers;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace NetHome.Services
{
    public class UserService : IUserService
    {
        public async Task<bool> Login(LoginModel loginModel)
        {
            LoginResponseModel response = await HttpRequestHelper.LoginAsync(loginModel);
            if (response == null) return false;
            string token = response.Token;
            SaveUserInfo(response.User);
            await SecureStorage.SetAsync("AuthorizationToken", token);
            return true;
        }

        public async Task<bool> Validate()
        {
            string token = await SecureStorage.GetAsync("AuthorizationToken");
            if (token == null) return false;
            var user = await HttpRequestHelper.ValidateAsync(token);
            if (user == null) return false;
            SaveUserInfo(user);
            return true;
        }
        
        public async Task<bool> Register(RegisterModel registerModel)
        {
            HttpResponseMessage response = await HttpRequestHelper.RegisterAsync(registerModel);
            return response.IsSuccessStatusCode;
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
            Preferences.Remove("UserDataJSON");
        }
    }
}
