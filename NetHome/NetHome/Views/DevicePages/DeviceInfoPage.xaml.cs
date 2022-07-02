using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetHome.Helpers;
using NetHome.ViewModels.Devices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NetHome.Views.DevicePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(DeviceId), nameof(DeviceId))]
    public partial class DeviceInfoPage : ContentPage
    {
        private readonly DeviceInfoViewModel _viewModel;
        public string DeviceId { get; set; }

        public DeviceInfoPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new DeviceInfoViewModel();
            _viewModel.FrameContentChange += FrameContentChange;
            _viewModel.AddButtons = AddButtons;
            _viewModel.RemoveButtons = RemoveButtons;
        }

        protected override void OnAppearing()
        {
            var id = 0;
            if (DeviceId is not null)
                id = int.Parse(DeviceId);
            _viewModel.OnAppearing(id);
            base.OnAppearing();
        }

        private void FrameContentChange(object sender, string e)
        {
            PageFrame.Content = e switch
            {
                "info" => Resources.GetValue<DataTemplate>("DeviceInfoTemplate").CreateContent() as View,
                "add" => Resources.GetValue<DataTemplate>("DeviceAddTemplate").CreateContent() as View,
                "update" => Resources.GetValue<DataTemplate>("DeviceEditTemplate").CreateContent() as View,
                _ => null
            };
        }

        private void AddButtons()
        {
            ToolbarItems.Clear();

            var updateToolbarItem = new ToolbarItem { Text = "Update" };
            updateToolbarItem.Clicked += _viewModel.ExecuteEdit;

            var deleteToolbarItem = new ToolbarItem { Text = "Delete" };
            deleteToolbarItem.Command = _viewModel.DeleteCommand;

            ToolbarItems.Add(updateToolbarItem);
            ToolbarItems.Add(deleteToolbarItem);
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