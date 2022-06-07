using System;
using System.Collections.Generic;
using System.Text;
using LX;
using Meteo.YRNO;

namespace Meteo.Views
{
    public class IntervalItem : Control
    {
        public IntervalItem(Interval interval, bool useHg)
        {

            Alignment = Alignment.TopFill;
            AutoHeight = true;
            Layout = new VerticalGallery() { VerticalAlignment = InlineAlignment.Center, HorizontalAlignment = InlineAlignment.Center };
            HorizontalScrollBar.Visible = false;

            var time = addTextItem(this, $"{interval.start?.Hour.ToString().PadLeft(2, '0')}:{interval.start?.Minute.ToString().PadLeft(2, '0')}");
            time.PaddingLeft = 16;
            var timeIcon = time.Add(Controller.GetWeatherSmallImage(interval.symbolCode.next6Hours));
            timeIcon.ImageAlignment = Alignment.Zoom;
            timeIcon.Size = new SizeF(32, 48);
            timeIcon.Shadow = ShadowStyle.Strong2;

            var temperature = addTextItem(this, $"{roundValue(interval.temperature?.value)}°");
            temperature.Font = Font.H3;
            temperature.PaddingLeft = 16;
            var temperatureIcon = temperature.Add(Image.LoadIcon(24, 0xEB7F), Alignment.LeftCenter);
            temperatureIcon.Enabled = false;
            temperatureIcon.Size = new SizeF(32, 48);

            var wind = addTextItem(this, "-");
            wind.PaddingLeft = 16;
            var windIcon = wind.Add(Image.LoadIcon(18, (ushort)(0xEC56)), Alignment.LeftCenter);
            windIcon.Size = new SizeF(32, 48);
            if (interval.wind?.speed != null)
            {
                double speed = interval.wind.speed.Value;
                wind.Text = $"{roundValue(speed)} m/s";
                if (interval.wind?.direction != null && speed > 0)
                {
                    windIcon.Image = Image.LoadIcon(18, (ushort)(0xEDF1));
                    windIcon.ImageColor = speed > 15 ? Color.Red : speed > 5 ? Color.Orange : Color.Content;
                    windIcon.Rotation = interval.wind.direction.Value;
                }
            }

            var precipitation = addTextItem(this, "-");
            precipitation.PaddingLeft = 16;
            precipitation.Enabled = false;
            var precipitationIcon = precipitation.Add(Image.LoadIcon(18, 0xE9B6), Alignment.LeftCenter);
            precipitationIcon.ImageColor = new Color(32, 112, 216);
            precipitationIcon.Size = new SizeF(32, 48);
            if (interval.precipitation?.value != null && interval.precipitation.value > 0)
            {
                precipitation.Enabled = true;
                precipitation.Text = $"{interval.precipitation.value} mm";
            }

            var humidity = addTextItem(this, "-");
            humidity.Enabled = false;
            humidity.TextPaddingLeft = 64;
            if (interval.humidity?.value != null)
            {
                humidity.Text = $"{roundValue(interval.humidity.value)}%";
            }

            var pressure = addTextItem(this, "-");
            pressure.Enabled = false;
            pressure.TextPaddingLeft = 64;
            if (interval.pressure?.value != null)
            {
                if (useHg)
                {
                    pressure.Text = $"{roundValue(interval.pressure.value.Value / 1.333)} mmHg";
                }
                else
                {
                    pressure.Text = $"{roundValue(interval.pressure.value.Value)} GPa";
                }
                
            }

        }

        private Label addTextItem(Control control, string text)
        {
            var item = new Label();
            item.AutoSize = false;
            item.Height = 48;
            item.Width = 196;
            item.Text = text;
            item.TextPadding = 0;
            item.TextAlignment = Alignment.LeftCenter;
            item.Font = Font.Subtitle;
            item.AddTo(control, Alignment.TopLeft);
            return item;
        }

        private string roundValue(double? value)
        {
            return value != null ? Math.Round(value.Value).ToString() : "";
        }
    }
}
