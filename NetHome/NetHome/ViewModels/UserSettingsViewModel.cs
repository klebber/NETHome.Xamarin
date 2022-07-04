using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using NetHome.Common;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using NetHome.Services;
using NetHome.Views.Popups;
using Xamarin.CommunityToolkit.Extensions;
using NetHome.Views;

namespace NetHome.ViewModels
{
    public class UserSettingsViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private ObservableCollection<UserModel> users;
        private Command onRefreshed;
        private Command<string> goToUserInfo;

        public ObservableCollection<UserModel> Users { get => users; set => SetProperty(ref users, value); }
        public ICommand OnRefreshed => onRefreshed ??= new Command(async () => await PerformOnRefreshed());
        public ICommand GoToUserInfo => goToUserInfo ??= new Command<string>(async (param) => await PerformGoToUserInfo(param));

        public UserSettingsViewModel()
        {
            _userService = DependencyService.Get<IUserService>();
        }

        internal async void OnAppearing()
        {
            await SetUsers();
        }


        private async Task SetUsers()
        {
            var response = await _userService.GetAllUsers();
            if (!response.IsSuccessful)
            {
                await Shell.Current.ShowPopupAsync(new Alert(response.ErrorType, response.ErrorMessage, "Ok", true));
                return;
            }
            Users = new ObservableCollection<UserModel>(response.Paylaod);
            IsWaiting = false;
        }

        private async Task PerformGoToUserInfo(string param)
        {
            var route = $"{nameof(AccountInfoPage)}?{nameof(AccountInfoPage.UserId)}={param}";
            await Shell.Current.GoToAsync(route);
        }


        private async Task PerformOnRefreshed()
        {
            await SetUsers();
        }
    }
}
