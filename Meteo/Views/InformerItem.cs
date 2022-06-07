using System;
using System.Collections.Generic;
using System.Text;
using LX;

namespace Meteo.Views
{
    public class InformerItem : Control
    {
        public PictureBox LeftIcon;
        public Label LeftText;
        public Label Text;
        public Label RightText;
        public PictureBox RightIcon;

        public InformerItem()
        {
            Width = 170;
            Height = 48;

            Layout = new HorizontalList();


            LeftIcon = new PictureBox();
            LeftIcon.Size = 32;
            LeftIcon.Enabled = false;
            LeftIcon.ImageAlignment = Alignment.BottomCenter;
            LeftIcon.ImagePaddingBottom = 8;
            LeftIcon.AddTo(this, Alignment.BottomLeft);

            LeftText = new Label();
            LeftText.Enabled = false;
            LeftText.TextPaddingLeft = 0;
            LeftText.TextPaddingRight = 0;
            LeftText.AddTo(this, Alignment.BottomLeft);

            Text = new Label();
            Text.Font = Font.H1;
            Text.TextPaddingTop = 0;
            Text.TextPaddingBottom = 0;
            Text.AddTo(this, Alignment.LeftFill);

            RightText = new Label();
            RightText.Enabled = false;
            RightText.TextPaddingLeft = 0;
            RightText.TextPaddingRight = 0;
            RightText.AddTo(this, Alignment.BottomLeft);

            RightIcon = new PictureBox();
            RightIcon.Enabled = false;
            RightIcon.Size = 32;
            RightIcon.ImageAlignment = Alignment.BottomCenter;
            RightIcon.ImagePaddingBottom = 8;
            RightIcon.AddTo(this, Alignment.BottomLeft);
        }
    }
}
