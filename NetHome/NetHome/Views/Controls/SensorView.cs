using NetHome.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Xamarin.Forms;

namespace NetHome.Views.Controls
{
    public class SensorView : ContentView
    {
        private DeviceModel sensor;
        public DeviceModel Sensor { get => sensor; set => SetProperty(ref sensor, value); }

        public StackLayout stack;
        public SensorView(DeviceModel sensor)
        {
            Sensor = sensor;
            stack = new StackLayout();
            stack.Spacing = 0;
            GenerateComponents();
            Content = stack;
        }

        private void GenerateComponents()
        {
            stack.Children.Clear();
            switch (Sensor.GetType().Name)
            {
                case nameof(THSensorModel):
                    stack.Children.Add(MakeView("temperature.png", Sensor.Room + " temperature:",((THSensorModel)Sensor).Temperature.ToString() + "°C"));
                    stack.Children.Add(MakeView("humidity.png", Sensor.Room + " humidity:", ((THSensorModel)Sensor).Humidity.ToString() + "%"));
                    break;

                case nameof(DWSensorModel):
                    stack.Children.Add(MakeView(((DWSensorModel)sensor).Placement switch
                    {
                        "Door" => "door.png",
                        "Window" => "window.png",
                        _ => "device_default.png"
                    }, Sensor.Name + ":", ((DWSensorModel)sensor).IsOpen ? "open" : "closed"));
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private View MakeView(string image, string name, string value)
        {
            return new StackLayout
            {
                Spacing = 0,
                Children =
                {
                    new BoxView
                    {
                        Color = (Color)Application.Current.Resources["Neutral"],
                        HeightRequest = 1,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    },
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Spacing = 0,
                        HeightRequest = 50,
                        Children =
                        {
                            new Image
                            {
                                Source = image,
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Center,
                                Margin = new Thickness(20,10,10,10),
                                Aspect = Aspect.AspectFit
                            },
                            new Label
                            {
                                Text = name,
                                HorizontalOptions = LayoutOptions.StartAndExpand,
                                VerticalOptions = LayoutOptions.Center,
                                Margin = new Thickness(10,0,0,0)
                            },
                            new Label
                            {
                                Text = value,
                                HorizontalOptions = LayoutOptions.End,
                                VerticalOptions = LayoutOptions.Center,
                                Margin = new Thickness(0,0,30,0)
                            }
                        }
                    }
                }
            };
        }

        protected void SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            field = newValue;
            OnPropertyChanged(propertyName);
        }
    }
}