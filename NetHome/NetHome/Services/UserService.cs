﻿using NetHome.Common;
using NetHome.Exceptions;
using NetHome.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
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

        public async Task<RequestResult> Login(LoginRequest loginRequest) => await ExecuteRequest(() => LoginFunc(loginRequest));
        public async Task<RequestResult> Validate() => await ExecuteRequest(() => ValidateFunc());
        public async Task<RequestResult> Register(RegisterRequest registerRequest) => await ExecuteRequest(() => RegisterFunc(registerRequest));
        public async Task<RequestResult> Update(UserModel user) => await ExecuteRequest(() => UpdateFunc(user));
        public async Task Logout()
        {
            UserDataManager.ClearUserData();
            await _webSocketService.CloseAsync();
        }

        private async Task<RequestResult> LoginFunc(LoginRequest loginRequest)
        {
            string json = JsonSerializer.Serialize(loginRequest);
            HttpResponseMessage response = await HttpRequestHelper.PostUnauthorizedAsync("api/user/login", json);
            Stream stream = await response.Content.ReadAsStreamAsync();
            LoginResponse loginResponse = await JsonSerializer.DeserializeAsync<LoginResponse>(stream, JsonHelper.GetOptions());
            await UserDataManager.SetUserData(loginResponse.User);
            await UserDataManager.SetAuthorizationToken(loginResponse.Token);
            await _webSocketService.ConnectAsync();
            return new RequestResult(true);
        }

        private async Task<RequestResult> ValidateFunc()
        {
            HttpResponseMessage response = await HttpRequestHelper.GetAsync("api/user/validate");
            Stream stream = await response.Content.ReadAsStreamAsync();
            UserModel userData = await JsonSerializer.DeserializeAsync<UserModel>(stream, JsonHelper.GetOptions());
            await UserDataManager.SetUserData(userData);
            await _webSocketService.ConnectAsync();
            return new RequestResult(true);
        }

        private async Task<RequestResult> RegisterFunc(RegisterRequest registerRequest)
        {
            var json = JsonSerializer.Serialize(registerRequest);
            await HttpRequestHelper.PostUnauthorizedAsync("api/user/register", json);
            return new RequestResult(true);
        }

        public async Task<RequestResult> UpdateFunc(UserModel user)
        {
            string json = JsonSerializer.Serialize(user);
            HttpResponseMessage response = await HttpRequestHelper.PostAsync("api/user/update", json);
            Stream stream = await response.Content.ReadAsStreamAsync();
            UserModel userModel = await JsonSerializer.DeserializeAsync<UserModel>(stream, JsonHelper.GetOptions());
            await UserDataManager.SetUserData(userModel);
            return new RequestResult(true);
        }

        private async Task<RequestResult> ExecuteRequest(Func<Task<RequestResult>> func)
        {
            try
            {
                if (!UserDataManager.UriExists())
                {
                    return new RequestResult(false)
                    {
                        ErrorType = "Uri not found!",
                        ErrorMessage = "Please enter a valid server uri."
                    };
                }
                return await func();
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException || e is SocketException)
                {
                    return new RequestResult(false)
                    {
                        ErrorType = "Server unreachable!",
                        ErrorMessage = "Could not reach server. Check if server is running and if uri is correct!"
                    };
                }
                if (e is ServerAuthorizationException)
                {
                    return new RequestResult(false)
                    {
                        ErrorType = "Invalid token!",
                        ErrorMessage = "Your token may have expired. Please try logging in again."
                    };
                }
                return new RequestResult(e);
            }
        }

        public async Task<RequestResultPayload<UserModel>> GetUser(string id)
        {
            try
            {
                var response = await HttpRequestHelper.GetAsync($"api/user/get?id={id}");
                var stream = await response.Content.ReadAsStreamAsync();
                var users = await JsonSerializer.DeserializeAsync<UserModel>(stream, JsonHelper.GetOptions());
                return new RequestResultPayload<UserModel>(true, users);
            }
            catch (Exception e)
            {
                return new RequestResultPayload<UserModel>(e);
            }
        }

        public async Task<RequestResultPayload<ICollection<UserModel>>> GetAllUsers()
        {
            try
            {
                var response = await HttpRequestHelper.GetAsync("api/user/getall");
                var stream = await response.Content.ReadAsStreamAsync();
                var users = await JsonSerializer.DeserializeAsync<ICollection<UserModel>>(stream, JsonHelper.GetOptions());
                return new RequestResultPayload<ICollection<UserModel>>(true, users);
            }
            catch (Exception e)
            {
                return new RequestResultPayload<ICollection<UserModel>>(e);
            }
        }

        public async Task<RequestResultPayload<ICollection<DeviceModel>>> GetAccessibleDevices(string userId)
        {
            try
            {
                var response = await HttpRequestHelper.GetAsync($"api/user/getaccessible?userId={userId}");
                var stream = await response.Content.ReadAsStreamAsync();
                var users = await JsonSerializer.DeserializeAsync<ICollection<DeviceModel>>(stream, JsonHelper.GetOptions());
                return new RequestResultPayload<ICollection<DeviceModel>>(true, users);
            }
            catch (Exception e)
            {
                return new RequestResultPayload<ICollection<DeviceModel>>(e);
            }
        }

        public async Task<RequestResult> AddDeviceAccess(DeviceAccessPayload dap)
        {
            try
            {
                var json = JsonSerializer.Serialize(dap, JsonHelper.GetOptions());
                var response = await HttpRequestHelper.PostAsync("api/user/giveaccess", json);
                return new RequestResult(true);
            }
            catch (Exception e)
            {
                return new RequestResult(e);
            }
        }

        public async Task<RequestResult> RemoveDeviceAccess(DeviceAccessPayload dap)
        {
            try
            {
                var json = JsonSerializer.Serialize(dap, JsonHelper.GetOptions());
                var response = await HttpRequestHelper.PostAsync("api/user/removeaccess", json);
                return new RequestResult(true);
            }
            catch (Exception e)
            {
                return new RequestResult(e);
            }
        }
    }
}
