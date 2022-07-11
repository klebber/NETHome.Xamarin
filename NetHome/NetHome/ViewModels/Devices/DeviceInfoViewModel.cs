using System;
using System.Collections.Generic;
using System.Text;
using NetHome.Common;
using NetHome.Services;
using System.Windows.Input;
using Xamarin.Forms;
using NetHome.Helpers;
using Xamarin.CommunityToolkit.Helpers;
using System.Threading.Tasks;
using NetHome.Views.Popups;
using Xamarin.CommunityToolkit.Extensions;
using System.Text.RegularExpressions;
using System.Net;
using System.Linq;

namespace NetHome.ViewModels.Devices
{
    public class DeviceInfoViewModel : BaseViewModel
    {
        private readonly IDeviceManager _deviceManager;
        private readonly IDeviceService _deviceService;
        private DevicePayload payload;
        private Command applyChangesCommand;
        private Command cancelEditingCommand;
        private Command addDeviceCommand;
        private Command cancelAddingCommand;
        private Command deleteCommand;
        private List<RoomModel> roomModels;
        private List<DeviceTypeModel> typeModels;
        private readonly WeakEventManager<string> _frameContentChangeEventManager;
        public event EventHandler<string> FrameContentChange
        {
            add => _frameContentChangeEventManager.AddEventHandler(value);
            remove => _frameContentChangeEventManager.RemoveEventHandler(value);
        }

        public string NewName { get; set; }
        public string NewModel { get; set; }
        public string NewGeneralType { get; set; }
        public string NewRoom { get; set; }
        public string NewType { get; set; }
        public DevicePayload Payload { get => payload; set { SetProperty(ref payload, value); OnPropertyChanged(); } }
        public List<RoomModel> RoomModels { get => roomModels; set => SetProperty(ref roomModels, value); }
        public List<string> Rooms { get => RoomModels.Select(r => r.Name).ToList(); }
        public List<DeviceTypeModel> TypeModels { get => typeModels; set => SetProperty(ref typeModels, value); }
        public List<string> Types { get => TypeModels.Select(t => t.Name).ToList(); }
        public ICommand ApplyChangesCommand => applyChangesCommand ??= new Command(async () => await ApplyChanges());
        public ICommand CancelEditingCommand => cancelEditingCommand ??= new Command(CancelEditing);
        public ICommand AddDeviceCommand => addDeviceCommand ??= new Command(async () => await AddDevice());
        public ICommand CancelAddingCommand => cancelAddingCommand ??= new Command(async () => await CancelAdding());
        public ICommand DeleteCommand => deleteCommand ??= new Command(async () => await ExecuteDelete());
        public Action AddButtons { get; set; }
        public Action RemoveButtons { get; set; }

        public DeviceInfoViewModel()
        {
            _deviceManager = DependencyService.Get<IDeviceManager>();
            _deviceService = DependencyService.Get<IDeviceService>();
            _frameContentChangeEventManager = new WeakEventManager<string>();
        }

        internal async void OnAppearing(int deviceId)
        {
            var roomsResponse = await _deviceService.GetAllRooms();
            if (roomsResponse.IsSuccessful)
            {
                RoomModels = roomsResponse.Paylaod;
            }
            else
            {
                await Shell.Current.ShowPopupAsync(new Alert(roomsResponse.ErrorType, roomsResponse.ErrorMessage, "Ok", true));
                await Shell.Current.GoToAsync("..");
                return;
            }
            var typesResponse = await _deviceService.GetAllDeviceTypes();
            if (typesResponse.IsSuccessful)
            {
                TypeModels = typesResponse.Paylaod;
            }
            else
            {
                await Shell.Current.ShowPopupAsync(new Alert(typesResponse.ErrorType, typesResponse.ErrorMessage, "Ok", true));
                await Shell.Current.GoToAsync("..");
                return;
            }
            if (deviceId == 0)
            {
                Payload = new DevicePayload();
                Payload.IpAdress = "http://192.168.";
                GoToAdd();
            }
            else
            {
                var response = await _deviceService.GetDevicePayload(deviceId);
                if (response.IsSuccessful)
                {
                    Payload = response.Paylaod;
                }
                else
                {
                    await Shell.Current.ShowPopupAsync(new Alert(response.ErrorType, response.ErrorMessage, "Ok", true));
                    await Shell.Current.GoToAsync("..");
                }
                GoToInfo();
            }
        }

        private void GoToAdd()
        {
            _frameContentChangeEventManager.RaiseEvent(new object(), "add", nameof(FrameContentChange));
            RemoveButtons();
        }

        private void GoToInfo()
        {
            _frameContentChangeEventManager.RaiseEvent(new object(), "info", nameof(FrameContentChange));
            AddButtons();
        }

        private void GoToUpdate()
        {
            _frameContentChangeEventManager.RaiseEvent(new object(), "update", nameof(FrameContentChange));
            RemoveButtons();
        }

        private async Task ApplyChanges()
        {
            var response = await _deviceService.UpdateDevice(Payload);
            if (response.IsSuccessful)
            {
                await Shell.Current.ShowPopupAsync(new Alert("Success!", "Device has successfuly been updated.", "Ok", true));
                GoToInfo();
            }
            else
            {
                await Shell.Current.ShowPopupAsync(new Alert(response.ErrorType, response.ErrorMessage, "Ok", true));
            }
        }

        private void CancelEditing()
        {
            GoToInfo();
        }

        private async Task AddDevice()
        {
            var typeName = $"NetHome.Common.{NewGeneralType}Model, NetHome.Common";
            Type t = Type.GetType(typeName);
            var newDevice = (DeviceModel)Activator.CreateInstance(t);
            newDevice.Name = NewName;
            newDevice.Model = NewModel;
            newDevice.Room = NewRoom;
            newDevice.Type = NewType;
            Payload.Device = newDevice;
            var response = await _deviceService.AddDevice(Payload);
            if (response.IsSuccessful)
            {
                await Shell.Current.ShowPopupAsync(new Alert("Success!", "Device has successfuly been added.", "Ok", true));
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.ShowPopupAsync(new Alert(response.ErrorType, response.ErrorMessage, "Ok", true));
            }
        }

        private async Task CancelAdding()
        {
            await Shell.Current.GoToAsync("..");
        }

        internal void ExecuteEdit(object sender, EventArgs e)
        {
            GoToUpdate();
        }

        public async Task ExecuteDelete()
        {
            var response = await _deviceService.DeleteDevice(Payload.Device);
            if (response.IsSuccessful)
            {
                await Shell.Current.ShowPopupAsync(new Alert("Success!", "Device has successfuly been deleted.", "Ok", true));
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Shell.Current.ShowPopupAsync(new Alert(response.ErrorType, response.ErrorMessage, "Ok", true));
            }
        }
    }
}
