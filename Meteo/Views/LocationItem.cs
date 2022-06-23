using System;
using System.Collections.Generic;
using System.Text;
using LX;
using Meteo.Controls;
using Meteo.YRNO;

namespace Meteo.Views
{
    public class LocationItem : Control
    {
        public PictureBox Icon;
        private Label name;
        private Label info;

        public LocationItem(Location location = null)
        {
            Layout = new VerticalList();
            AutoHeight = true;
            Padding = 8;
            bool isLarge = location == null;

            Icon = isLarge
                ? Image.LoadFromResource("*.yrno.png")
                : Image.LoadFromFont("Icons.ttf", 24, (ushort)(0xEAF8));
            Icon.Alignment = Alignment.LeftCenter | Alignment.NotLayouted;
            Icon.Size = isLarge ? 96 : 24;
            Icon.Left = 8;
            Icon.ImageColor = isLarge ? Color.White : Color.Content.Alpha(150);
            Icon.AddTo(this);

            name = new ScrollableLabel();
            name.Text = "YR";
            name.Alignment = Alignment.TopFill;
            name.AutoHeight = true;
            name.AutoWidth = false;
            name.Trimming = true;
            name.TextPadding = 0;
            name.Font = isLarge ? Font.H1 : Font.H3;
            name.TextAlignment = Alignment.LeftCenter;
            name.Left = isLarge ? 116 : 32;
            name.UserMouse = isLarge ? UserMode.On : UserMode.None;
            name.AddTo(this);

            info = new ScrollableLabel();
            info.Text = "Norwegian Meteorological Institute and NRK";
            info.Alignment = Alignment.TopFill;
            info.AutoHeight = true;
            info.AutoWidth = false;
            info.Trimming = true;
            info.TextPadding = 0;
            info.TextColor = Color.Content.Alpha(150);
            info.Font = isLarge ? Font.H3 : Font.Body;
            info.TextAlignment = Alignment.LeftCenter;
            info.Left = isLarge ? 116 : 32;
            info.UserMouse = isLarge ? UserMode.On : UserMode.None;
            info.AddTo(this);

            Update(location);
        }


        public void Update(Location location)
        {
            if (location == null)
            {

            }
            else
            {
                name.Text = location.name;

                var infoText = "";

                if (location.category != null)
                {
                    infoText += location.category.name + ", ";
                }
                if (location.region != null)
                {
                    infoText += location.region.name + ", ";
                }
                if (location.country != null)
                {
                    infoText += location.country.name + ", ";
                }
                if (location.elevation != null)
                {
                    infoText += "elevation " + location.elevation + " m, ";
                }

                info.Text = infoText.TrimEnd(',', ' ');

            }
        }

    }
}
