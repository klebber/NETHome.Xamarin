using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetHome.Helpers;
using NetHome.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NetHome.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(UserId), nameof(UserId))]
    public partial class AccountInfoPage : ContentPage
    {
        private readonly AccountInfoViewModel _viewModel;
        public string UserId { get; set; }

        public AccountInfoPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new AccountInfoViewModel();
            _viewModel.FrameContentChange += FrameContentChange;
            _viewModel.AddButtons = AddButtons;
            _viewModel.RemoveButtons = RemoveButtons;
        }

        protected override void OnAppearing()
        {
            _viewModel.OnAppearing(UserId);
            base.OnAppearing();
        }

        private void FrameContentChange(object sender, string e)
        {
            PageFrame.Content = e switch
            {
                "info" => Resources.GetValue<DataTemplate>("UserInfoTemplate").CreateContent() as View,
                "user" => Resources.GetValue<DataTemplate>("UserEditingTemplate").CreateContent() as View,
                "owner" => Resources.GetValue<DataTemplate>("OwnerEditingTemplate").CreateContent() as View,
                _ => null
            };
        }

        private void AddButtons()
        {
            ToolbarItems.Clear();

            var updateToolbarItem = new ToolbarItem { Text = "Edit" };
            updateToolbarItem.Command = _viewModel.EditCommand;

            ToolbarItems.Add(updateToolbarItem);
        }

        private void RemoveButtons()
        {
            if (ToolbarItems.Count == 0)
                return;
            for (int i = ToolbarItems.Count - 1; i >= 0; i--)
                ToolbarItems.Remove(ToolbarItems[i]);
        }
    }
}