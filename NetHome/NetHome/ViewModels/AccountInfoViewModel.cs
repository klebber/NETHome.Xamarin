using System;
using System.Collections.Generic;
using System.Text;
using NetHome.Common;
using NetHome.Helpers;
using NetHome.Services;
using System.Windows.Input;
using Xamarin.Forms;
using NetHome.Views.Popups;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Helpers;
using System.Linq;

namespace NetHome.ViewModels
{
    public class AccountInfoViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private readonly IDeviceService _deviceService;
        private UserModel user;
        private List<DeviceModel> devicesAccess;
        private Command editCommand;
        private Command applyChangesCommand;
        private Command cancelEditingCommand;
        private Command addDeviceAccessCommand;
        private Command<int> removeDeviceAccessCommand;
        private bool isOwnerEditing;
        private bool isAdminOrOwner;
        private readonly WeakEventManager<string> _frameContentChangeEventManager;
        public event EventHandler<string> FrameContentChange
        {
            add => _frameContentChangeEventManager.AddEventHandler(value);
            remove => _frameContentChangeEventManager.RemoveEventHandler(value);
        }

        public UserModel User { get => user; set { SetProperty(ref user, value); OnPropertyChanged(nameof(RoleList)); } }
        public string RoleList => User is not null ? string.Join("\n", User.Roles) : string.Empty;
        public List<DeviceModel> DevicesAccess { get => devicesAccess; set => SetProperty(ref devicesAccess, value); }
        public bool IsAdminOrOwner { get => isAdminOrOwner; set => SetProperty(ref isAdminOrOwner, value); }
        public ICommand EditCommand => editCommand ??= new Command(Edit);
        public ICommand ApplyChangesCommand => applyChangesCommand ??= new Command(async () => await ApplyChanges());
        public ICommand CancelEditingCommand => cancelEditingCommand ??= new Command(CancelEditing);
        public ICommand AddDeviceAccessCommand => addDeviceAccessCommand ??= new Command(async () => await AddDeviceAccess());
        public ICommand RemoveDeviceAccessCommand => removeDeviceAccessCommand ??= 
            new Command<int>(async (param) => await RemoveDeviceAccess(param));
        public Action AddButtons { get; set; }
        public Action RemoveButtons { get; set; }

        public AccountInfoViewModel()
        {
            _userService = DependencyService.Get<IUserService>();
            _deviceService = DependencyService.Get<IDeviceService>();
            _frameContentChangeEventManager = new WeakEventManager<string>();
        }

        internal async void OnAppearing(string userId)
        {
            await SetUser(userId);
        }

        private async Task SetUser(string userId)
        {
            UserModel userData = UserDataManager.GetUserData();
            if (userData.Id == userId)
            {
                User = userData;
                IsAdminOrOwner = User.IsAdminOrOwner();
                isOwnerEditing = false;
            }
            else
            {
                var response = await _userService.GetUser(userId);
                if (!response.IsSuccessful)
                {
                    await Shell.Current.ShowPopupAsync(new Alert(response.ErrorType, response.ErrorMessage, "Ok", true));
                    await Shell.Current.GoToAsync("..");
                    return;
                }
                User = response.Paylaod;
                isOwnerEditing = true;
                IsAdminOrOwner = User.IsAdminOrOwner();
                if (!IsAdminOrOwner)
                {
                    await PopulateDeviceAccess();
                }
            }
            GoToInfo();
        }

        private async Task PopulateDeviceAccess()
        {
            var responseDA = await _userService.GetAccessibleDevices(User.Id);
            if (!responseDA.IsSuccessful)
            {
                await Shell.Current.ShowPopupAsync(new Alert(responseDA.ErrorType, responseDA.ErrorMessage, "Ok", true));
                await Shell.Current.GoToAsync("..");
                return;
            }
            DevicesAccess = new List<DeviceModel>(responseDA.Paylaod);
        }

        private void Edit()
        {
            if (isOwnerEditing)
                GoToOwnerEdit();
            else
                GoToUserEdit();
        }

        private async Task ApplyChanges()
        {
            IsWaiting = true;
            var response = await _userService.Update(User);
            if (!response.IsSuccessful)
            {
                IsWaiting = false;
                await Shell.Current.ShowPopupAsync(new Alert(response.ErrorType, response.ErrorMessage, "Ok", true));
                return;
            }
            IsWaiting = false;
            await Shell.Current.ShowPopupAsync(new Alert("Success!", "Account info has been updated.", "Ok", true));
            await SetUser(User.Id);
            GoToInfo();
        }


        private void CancelEditing()
        {
            GoToInfo();
        }

        private void GoToInfo()
        {
            _frameContentChangeEventManager.RaiseEvent(new object(), "info", nameof(FrameContentChange));
            AddButtons();
        }

        private void GoToUserEdit()
        {
            _frameContentChangeEventManager.RaiseEvent(new object(), "user", nameof(FrameContentChange));
            RemoveButtons();
        }

        private void GoToOwnerEdit()
        {
            _frameContentChangeEventManager.RaiseEvent(new object(), "owner", nameof(FrameContentChange));
            RemoveButtons();
        }

        private async Task AddDeviceAccess()
        {
            var itemsSource = await _deviceService.GetAllDevices();
            string result = await Shell.Current.ShowPopupAsync(new PickerPopup(
                "Add Device Access", "Pick a device to give access to:",
                "Device Name", itemsSource.Paylaod.Select(d => d.Name).ToList(), "Give Access", true, true));
            if (string.IsNullOrWhiteSpace(result))
                return;
            var deviceId = itemsSource.Paylaod.Single(d => d.Name == result).Id;
            var dap = new DeviceAccessPayload()
            {
                DeviceId = deviceId,
                UserId = User.Id
            };
            var response = await _userService.AddDeviceAccess(dap);
            if (!response.IsSuccessful)
            {
                await Shell.Current.ShowPopupAsync(new Alert(response.ErrorType, response.ErrorMessage, "Ok", true));
                return;
            }
            await Shell.Current.ShowPopupAsync(new Alert("Success!",
                $"This user has been granted access to device: {deviceId}.", "Ok", true));
            await PopulateDeviceAccess();
        }

        private async Task RemoveDeviceAccess(int deviceId)
        {
            var dap = new DeviceAccessPayload()
            {
                DeviceId = deviceId,
                UserId = User.Id
            };
            var response = await _userService.RemoveDeviceAccess(dap);
            if (!response.IsSuccessful)
            {
                await Shell.Current.ShowPopupAsync(new Alert(response.ErrorType, response.ErrorMessage, "Ok", true));
                return;
            }
            await Shell.Current.ShowPopupAsync(new Alert("Success!", 
                $"This user's access to device: {deviceId} has been revoked.", "Ok", true));
            await PopulateDeviceAccess();
        }
    }
}
