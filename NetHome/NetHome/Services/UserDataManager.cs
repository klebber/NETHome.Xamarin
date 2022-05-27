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
        public static void SaveUserData(UserModel user)
        {
            Preferences.Remove("UserDataJSON");
            Preferences.Set("UserDataJSON", JsonSerializer.Serialize(user));
        }

        public static UserModel GetUserData()
        {
            string json = Preferences.Get("UserDataJSON", null);
            return json is not null ? JsonSerializer.Deserialize<UserModel>(json) : null;
        }

        public static void ClearUserData()
        {
            Preferences.Remove("UserDataJSON");
        }

        public static async Task SaveAuthorizationToken(string token)
        {
            await SecureStorage.SetAsync("AuthorizationToken", "Bearer " + token);
        }

        public static async Task<string> GetAuthorizationToken()
        {
            return await SecureStorage.GetAsync("AuthorizationToken");
        }
        
        public static void ClearAuthorizationToken()
        {
            SecureStorage.Remove("AuthorizationToken");
        }

        public static void SetUri(string uri)
        {
            Preferences.Set("ServerAddress", uri);
        }

        public static string GetUri()
        {
            return Preferences.Get("ServerAddress", string.Empty);
        }

        public static void RemoveUri()
        {
            Preferences.Remove("ServerAddress");
        }
    }
}
