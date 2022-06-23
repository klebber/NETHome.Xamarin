using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace NetHome.ViewModels
{
    public class BaseViewModel : BindableObject
    {
        private bool isWaiting;

        public bool IsWaiting { get => isWaiting; set => SetProperty(ref isWaiting, value); }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                OnPropertyChanged(propertyName);
                return true;
            }

            return false;
        }
    }
}
