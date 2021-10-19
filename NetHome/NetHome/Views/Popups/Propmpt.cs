using NetHome.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace NetHome.Views
{
    public class Propmpt : Popup<string>
    {
        public Propmpt(string title, string message, string placeholder, string text, string okButtonText,
            bool modal, bool showCancelButton, string cancelButtonText = "Cancel", Keyboard keyboard = null)
        {
            Entry entry;
            keyboard ??= Keyboard.Default;
            Size = new Size(300, 250);
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
                        new StackLayout
                        {
                            VerticalOptions = LayoutOptions.StartAndExpand,
                            Padding = new Thickness(20,10,20,0),
                            Children =
                            {
                                new Label
                                {
                                    Text = title,
                                    VerticalOptions = LayoutOptions.CenterAndExpand,
                                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                                    TextColor = Color.White,
                                    FontAttributes = FontAttributes.Bold,
                                    FontSize = 20
                                },
                                new Label
                                {
                                    Text = message,
                                    VerticalOptions = LayoutOptions.CenterAndExpand,
                                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                                    TextColor = Color.White
                                }
                            }
                        },
                        new Frame
                        {
                            CornerRadius = 10,
                            Padding = 0,
                            HorizontalOptions = LayoutOptions.Fill,
                            Margin = new Thickness(20,0,20,10),
                            Content = entry = new Entry
                            {
                                Placeholder = placeholder,
                                Text = text,
                                Keyboard = keyboard
                            }
                        },
                        new StackLayout
                        {
                            VerticalOptions = LayoutOptions.EndAndExpand,
                            HorizontalOptions = LayoutOptions.Fill,
                            Orientation = StackOrientation.Horizontal,
                            Spacing = 0,
                            Children =
                            {
                                new Button
                                {
                                    Text = okButtonText,
                                    CornerRadius = 0,
                                    Margin = 0,
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                    Command = new Command(() => Dismiss(entry.Text))
                                },
                                new Button
                                {
                                    Text = cancelButtonText,
                                    IsVisible = showCancelButton || modal,
                                    BackgroundColor = (Color)Application.Current.Resources["Neutral"],
                                    CornerRadius = 0,
                                    Margin = 0,
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                    Command = new Command(() => Dismiss(null))
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}