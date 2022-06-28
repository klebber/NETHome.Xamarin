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

namespace NetHome.ViewModels
{
    public class AccountInfoViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private UserModel user;
        private bool isEditing;
        private Command editCommand;
        private Command applyChangesCommand;
        private Command cancelEditingCommand;
        public UserModel User { get => user; set { SetProperty(ref user, value); OnPropertyChanged(nameof(RoleList)); } }
        public string RoleList => User is not null ? string.Join("\n", User.Roles) : string.Empty;
        public bool IsEditing { get => isEditing; set => SetProperty(ref isEditing, value); }
        public ICommand EditCommand => editCommand ??= new Command(Edit, () => !IsEditing);
        public ICommand ApplyChangesCommand => applyChangesCommand ??= new Command(async () => await ApplyChanges());
        public ICommand CancelEditingCommand => cancelEditingCommand ??= new Command(CancelEditing);

        public AccountInfoViewModel()
        {
            _userService = DependencyService.Get<IUserService>();
        }

        internal void OnAppearing(string userId)
        {
            SetUser(userId);
        }

        private void SetUser(string userId)
        {
            UserModel userData = UserDataManager.GetUserData();
            if (userData.Id == userId)
            {
                User = userData;
                return;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void Edit()
        {
            IsEditing = true;
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
            IsEditing = false;
            SetUser(User.Id);
        }


        private void CancelEditing()
        {
            IsEditing= false;
        }
    }
}
