using NetHome.Helpers;
using NetHome.Models.User;
using System.IO;
using System.Net;
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
            HttpResponseMessage response = await HttpRequestHelper.LoginAsync(loginModel);
            if (!response.IsSuccessStatusCode) return false;
            string token = response.Content.ReadAsStringAsync().Result;
            await SecureStorage.SetAsync("AuthorizationToken", token);
            return true;
        }

        public async Task<bool> Validate()
        {
            string token = await SecureStorage.GetAsync("AuthorizationToken");
            return token != null && await HttpRequestHelper.ValidateAsync(token);
        }
        
        public async Task<bool> Register(RegisterModel registerModel)
        {
            HttpResponseMessage response = await HttpRequestHelper.RegisterAsync(registerModel);
            return response.IsSuccessStatusCode;
        }
    }
}
