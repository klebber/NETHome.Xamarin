using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NetHome.Views.Controls
{
    public class CustomCornerFrame : Frame
    {
        public static new readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CustomCornerFrame), typeof(CornerRadius), typeof(CustomCornerFrame));
        public CustomCornerFrame()
        {
            Padding = 0;
            base.CornerRadius = 0;
        }

        public new CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

    }
}
