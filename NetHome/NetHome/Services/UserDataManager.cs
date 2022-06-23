using NetHome.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace NetHome.Services
{
    public static class UserDataManager
    {
        private static UserModel userData;

        public static async Task SetUserData(UserModel user)
        {
            userData = user;
            await SecureStorage.SetAsync("Username", user.Username);
        }

        public static UserModel GetUserData()
        {
            return userData;
        }

        public static async Task<string> GetUsername()
        {
            return await SecureStorage.GetAsync("Username");
        }

        public static void ClearUserData()
        {
            userData = null;
            SecureStorage.Remove("Username");
            SecureStorage.Remove("AuthorizationToken");
        }

        public static async Task<bool> IsUserDataSaved()
        {
            return !string.IsNullOrWhiteSpace(await SecureStorage.GetAsync("AuthorizationToken"))
                && !string.IsNullOrWhiteSpace(await SecureStorage.GetAsync("Username"));
        }

        public static async Task SetAuthorizationToken(string token)
        {
            await SecureStorage.SetAsync("AuthorizationToken", "Bearer " + token);
        }

        public static async Task<string> GetAuthorizationToken()
        {
            return await SecureStorage.GetAsync("AuthorizationToken");
        }

        public static void SetUri(string uri)
        {
            Preferences.Set("ServerAddress", uri);
        }

        public static string GetUri()
        {
            return Preferences.Get("ServerAddress", string.Empty);
        }

        public static bool UriExists()
        {
            return Preferences.ContainsKey("ServerAddress") 
                && !string.IsNullOrWhiteSpace(Preferences.Get("ServerAddress", string.Empty));
        }

        public static void RemoveUri()
        {
            Preferences.Remove("ServerAddress");
        }
    }
}
