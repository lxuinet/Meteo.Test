using System;
using System.Collections.Generic;
using System.Text;
using LX;
using Meteo.YRNO;

namespace Meteo.Views
{
    public class CurrentView : Control
    {
        private InformerItem weather;
        private Control listInfo;
        private InformerItem feelsLike;
        private InformerItem precipitation;
        private InformerItem wind;

        public CurrentView()
        {
            PaddingLeft = 28;
            AutoHeight = true;

            weather = new InformerItem();
            weather.AutoSize = true;
            weather.LeftText.TextPaddingLeft = 8;
            weather.LeftText.Text = "";
            weather.LeftIcon.Enabled = true;
            weather.LeftIcon.Size = 64;
            weather.LeftIcon.Shadow = ShadowStyle.Strong2;
            weather.LeftIcon.ImageColor = Color.White;
            weather.LeftIcon.ImageAlignment = Alignment.Zoom;
            weather.LeftIcon.ImagePadding = 0;
            weather.LeftIcon.Alignment = Alignment.LeftCenter;
            weather.AddTo(this, Alignment.LeftCenter);
            weather.OnSizeChanged += delegate { listInfo.Left = weather.Width; };

            listInfo = new Control();
            listInfo.AutoHeight = true;
            listInfo.Padding = 0;
            listInfo.Alignment = Alignment.HorizontalCenter;
            listInfo.Layout = new VerticalGallery();
            listInfo.Left = 0;
            listInfo.HorizontalScrollBar.Visible = false;
            listInfo.UserHorizontalScroll = UserMode.None;
            listInfo.AddTo(this);

            feelsLike = new InformerItem();
            feelsLike.LeftIcon.Image = Image.LoadFromFont("Icons.ttf", 24, (ushort)(0xE95C)); 
            feelsLike.LeftText.Text = "Feels like";
            feelsLike.Text.TextPaddingTop = 10;
            feelsLike.Text.Font = Font.Subtitle;
            feelsLike.AddTo(listInfo);

            precipitation = new InformerItem();
            precipitation.LeftIcon.Image = Image.LoadFromFont("Icons.ttf", 24, (ushort)(0xEE41));
            precipitation.Text.TextColor = new Color(32, 112, 216);
            precipitation.RightText.Text = "mm";
            precipitation.RightText.TextColor = new Color(32, 112, 216);
            precipitation.AddTo(listInfo);

            wind = new InformerItem();
            wind.LeftIcon.Image = Image.LoadFromFont("Icons.ttf", 24, (ushort)(0xEC56));
            wind.RightText.Text = "m/s";
            wind.RightIcon.Image = Image.LoadFromFont("Icons.ttf", 24, (ushort)(0xEDF1));
            wind.Text.Enabled = false;
            wind.AddTo(listInfo);

        }

        public void Update(CurrentHour currentHour)
        {
            var temperatureValue = currentHour.temperature?.value;
            if (temperatureValue != null)
            {
                temperatureValue = Math.Round((double)temperatureValue);
            }

            weather.Text.Text = temperatureValue != null ? temperatureValue.ToString() + "°" : "-";
            weather.Text.TextColor = temperatureValue != null && temperatureValue > 0 ? new Color(220, 0, 10) : new Color(32, 112, 216);

            var feelsLikeValue = currentHour.temperature?.feelsLike;
            if (feelsLikeValue != null)
            {
                feelsLikeValue = Math.Round((double)feelsLikeValue);
            }

            feelsLike.Text.Text = feelsLikeValue != null ? feelsLikeValue.ToString() + "°" : "-";
            feelsLike.Text.TextColor = feelsLikeValue != null && feelsLikeValue > 0 ? new Color(220, 0, 10) : new Color(32, 112, 216);

            var precipitationValue = currentHour.precipitation?.value;
            precipitation.Text.Text = precipitationValue != null ? precipitationValue.ToString() : "-";

            var windValue = currentHour.wind?.speed;
            if (windValue != null)
            {
                windValue = Math.Round((double)windValue);
            }

            wind.Text.Text = windValue != null ? windValue.ToString() : "-";

            var windDirection = currentHour.wind?.direction;
            if (windDirection != null)
            {
                windDirection = Math.Round((double)windDirection);
            }

            wind.RightIcon.Rotation = windDirection != null ? (double)windDirection : 0;

            weather.LeftIcon.Image = Controller.GetWeatherImage(currentHour.symbolCode.next1Hour);
        }
    }
}
