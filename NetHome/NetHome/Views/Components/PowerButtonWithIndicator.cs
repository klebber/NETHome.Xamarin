using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NetHome.Views.Components
{
    public partial class PowerButtonWithIndicator : ContentView
    {
        public static readonly BindableProperty IsWaitingProperty = BindableProperty.Create(nameof(IsWaiting),
            typeof(bool), typeof(PowerButtonWithIndicator), false);
        public static readonly BindableProperty CurrentStateProperty = BindableProperty.Create(nameof(CurrentState),
            typeof(bool), typeof(PowerButtonWithIndicator), false);
        public static readonly BindableProperty ClickCommandProperty = BindableProperty.Create(nameof(ClickCommand),
            typeof(Command), typeof(PowerButtonWithIndicator), null);

        public bool IsWaiting
        {
            get => (bool)GetValue(IsWaitingProperty);
            set => SetValue(IsWaitingProperty, value);
        }

        public bool CurrentState
        {
            get => (bool)GetValue(CurrentStateProperty);
            set => SetValue(CurrentStateProperty, value);
        }

        public Command ClickCommand
        {
            get => (Command)GetValue(ClickCommandProperty);
            set => SetValue(ClickCommandProperty, value);
        }
    }
}
