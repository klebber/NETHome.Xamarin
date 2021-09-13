using NetHome.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NetHome.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        private readonly RegistrationViewModel _viewModel;
        public RegistrationPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new RegistrationViewModel();
        }
    }
}