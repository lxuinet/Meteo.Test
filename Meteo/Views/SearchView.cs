using System;
using System.Threading;
using System.Threading.Tasks;
using LX;
using Meteo.YRNO;

namespace Meteo.Views
{
    public class SearchView : Control
    {
        private TextBox searchText;
        private PictureBox searchIcon;
        private PictureBox clearButton;
        private Menu menu;

        public SearchView()
        {
            Height = 48;
            Color = Color.Primary;
            Padding = 8;
            BorderSize = 2;
            BorderColor = Color.Secondary;
            SetShape(CornerShape.Oval, CornerShape.Oval, CornerShape.Rectangle, CornerShape.Rectangle);
            Radius = 4;
            Ranges.MaximumWidth = 48;
            OnSizeChanged += delegate { menu.Width = Width; };

            menu = new Menu();
            menu.Transparency = 240;
            menu.Shadow = ShadowStyle.None;
            menu.BorderSize = 1;
            menu.BorderColor = Color.Secondary;

            searchIcon = Add(Image.LoadFromFont("Icons.ttf", 24, (ushort)(0xE9ED)), Alignment.LeftCenter);
            searchIcon.UserMouse = UserMode.On;
            searchIcon.ImageColor = Color.Secondary;
            searchIcon.Size = 32;
            searchIcon.OnClick += SearchIcon_OnClick;

            searchText = new TextBox();
            searchText.Visible = false;
            searchText.TextPadding = 0;
            searchText.AutoSize = false;
            searchText.BorderSize = 0;
            searchText.Color = Color.Parent;
            searchText.Font = Font.Subtitle;
            searchText.Alignment = Alignment.Fill;
            searchText.Left = 32;
            searchText.Right = 32;
            searchText.Focused = false;
            searchText.AddTo(this);
            searchText.OnTextChanged += SearchText_OnTextChanged;
            searchText.OnFocusedChanged += SearchText_OnFocusedChanged;

            clearButton = Add(Image.LoadFromFont("Icons.ttf", 24, (ushort)(0xEB33)), Alignment.RightCenter);
            clearButton.Visible = false;
            clearButton.Shape = CornerShape.Oval;
            clearButton.ImageColor = Color.Secondary;
            clearButton.Radius = 16;
            clearButton.Size = 32;
            clearButton.Style = ColorStyle.All;
            clearButton.UserMouse = UserMode.On;
            clearButton.OnClick += delegate { searchText.Text = ""; };

            var locationId = App.Configuration["LocationId", ""];
            searchText.Focused = string.IsNullOrEmpty(locationId);
        }

        private void SearchIcon_OnClick(object sender, MouseEventArgs e)
        {
            searchText.Focused = true;
            searchText.SelectAllText();
            SearchText_OnTextChanged(sender, e);
        }

        private void SearchText_OnFocusedChanged(object sender, EventArgs e)
        {
            if (searchText.Focused)
            {
                Ranges.MaximumWidth = double.MaxValue;
                clearButton.Visible = true;
                searchText.Visible = true;

                StartHotKey(Key.Escape).Press += delegate { searchText.Focused = false; return true; };
                StartHotKey(Key.BrowserBack).Press += delegate { searchText.Focused = false; return true; };
            }
            else
            {
                //Transparency = 100;
                Ranges.MaximumWidth = 48;
                clearButton.Visible = false;
                searchText.Visible = false;

                StopHotKey(Key.Escape);
                StopHotKey(Key.BrowserBack);
            }
        }

        private void SearchText_OnTextChanged(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {

                var text = searchText.Text;

                Thread.Sleep(250);

                if (text != searchText.Text)
                    return;

                var locations = new Location[0];

                try
                {
                    locations = Controller.Find(text);
                }
                catch(Exception exception)
                {
                    exception.Log();
                }

                if (text != searchText.Text)
                    return;

                //Window.SafeAction(() =>
                //{

                    menu.Controls.Clear();

                    if (locations.Length > 0)
                    {
                        foreach (var location in locations)
                        {
                            var item = new LocationItem(location);
                            item.AddTo(menu, Alignment.TopFill);
                            item.OnClick += delegate
                            {
                                searchText.Focused = false;
                                MainForm.SetId(location.id);
                            };

                            var line = new Control();
                            line.Height = 1;
                            line.Enabled = false;
                            line.Style = ColorStyle.Disabled;
                            line.AddTo(menu, Alignment.TopFill);
                        }

                        menu.Width = Width;
                        menu.ShowPopup(this, 0, Height - 1);
                    }
                    else
                    {
                        menu.HidePopup();
                    }

                //});
            });
        }

    }
}
