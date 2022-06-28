using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        protected override void OnAppearing()
        {
            _viewModel.OnAppearing(UserId);
            base.OnAppearing();
        }
    }
}