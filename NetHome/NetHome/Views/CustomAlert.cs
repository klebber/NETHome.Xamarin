using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace NetHome.Views
{
    public class CustomAlert : Popup
    {
        public CustomAlert(string title, string message, string okButtonText, bool modal)
        {
            Size = new Size(300, 200);
            Color = Color.Transparent;
            IsLightDismissEnabled = !modal;
            Content = new Frame
            {
                CornerRadius = 20,
                Padding = 0,
                HasShadow = true,
                Content = new StackLayout
                {
                    BackgroundColor = (Color)Application.Current.Resources["Primary"],
                    Children =
                    {
                        new Label
                        {
                            Text = title,
                            VerticalOptions = LayoutOptions.StartAndExpand,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            TextColor = Color.White,
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 20,
                            Padding = new Thickness(20,10,20,0),
                        },
                        new Label
                        {
                            Text = message,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.CenterAndExpand,
                            TextColor = Color.White,
                            Padding = new Thickness(20,0,20,10),
                        },
                        new StackLayout
                        {
                            VerticalOptions = LayoutOptions.EndAndExpand,
                            HorizontalOptions = LayoutOptions.Fill,
                            Orientation = StackOrientation.Horizontal,
                            Children =
                            {
                                new Button
                                {
                                    Text = okButtonText,
                                    CornerRadius = 0,
                                    Margin = 0,
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                    Command = new Command(() => Dismiss(new object()))
                                }
                            }
                        }

                    }
                }
            };
        }
    }
}