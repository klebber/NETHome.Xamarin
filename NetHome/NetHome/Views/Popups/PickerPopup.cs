using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NetHome.Views.Popups
{
    public class PickerPopup : Popup<string>
    {
        public PickerPopup(string title, string message, string placeholder, List<string> source, string okButtonText,
            bool modal, bool showCancelButton, string cancelButtonText = "Cancel", Keyboard keyboard = null)
        {
            Picker picker;
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
                            Content = picker = new Picker
                            {
                                Title = placeholder,
                                ItemsSource = source
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
                                    Command = new Command(() => Dismiss(picker.SelectedItem.ToString()))
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
