using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LX;
using Meteo.YRNO;

namespace Meteo.Views
{
    public class MainForm : Control
    {

        #region Meteo

        private static String meteoId;
        private static DateTime meteoTime = DateTime.MinValue;
        internal static void SetId(string id)
        {
            meteoId = id;
            meteoTime = DateTime.MinValue;

            App.Configuration["LocationId"] = id;
            App.Configuration.Save();
        }

        private void MeteoProc()
        {
            while (true)
            {
                if (!string.IsNullOrEmpty(meteoId) && (DateTime.Now - meteoTime).TotalMinutes > 15)
                {

                    try
                    {
                        StartUpdate();
                        var id = meteoId;
                        var location = Controller.GetLocation(id);
                        var currentHour = Controller.GetCurrentHour(id);
                        var forecast = Controller.GetForecast(id);
                        meteoTime = DateTime.Now;

                        this.location = location;
                        this.currentHour = currentHour;
                        this.forecast = forecast;

                        Update();
                    }
                    catch (Exception exception)
                    {
                        exception.Log();
                        Thread.Sleep(5000);
                    }
                    finally
                    {
                        StopUpdate();
                    }
                }

                Thread.Sleep(100);
            }
        }

        #endregion

        private Control header;
        private SearchView searchView;
        private LocationItem locationItem;
        private PictureBox updater;
        private CurrentView currentView;
        private Control table;
        private CheckBox intervalToggle;
        private CheckBox pressureToggle;
        private Control tools;

        public MainForm()
        {
            Window.Title = "Norwegian Meteorological Institute and NRK";


            Skin whiteSkin = new WhiteSkin();
            Skin blackSkin = new BlackSkin();

            Window.Skin = whiteSkin;

            StartHotKey(Key.Escape).Press += delegate { LX.App.Exit(); return true; };
            StartHotKey(Key.BrowserBack).Press += delegate { LX.App.Exit(); return true; };

            AddToRoot();
            CanFocus = true;
            UserMouse = UserMode.On;
            Alignment = Alignment.Fill;
            Layout = new VerticalList(0);


            header = new Control();
            header.Alignment = Alignment.TopFill;
            header.Height = 180;
            header.Color = Color.Primary;
            header.SetPadding(16, 16, 16, 8);
            header.Shadow = ShadowStyle.Bottom2;
            header.HorizontalScrollBar.Height = 2;
            header.HorizontalScrollBar.Visible = true;
            header.AddTo(this);
            header.OnSizeChanged += Header_OnSizeChanged;


            PictureBox back = new PictureBox();
            back.Transparency = 10;
            back.Alignment = Alignment.Fill | Alignment.NotLayouted;
            back.Image = Image.LoadFromResource("*.back.jpg");
            back.ImageColor = Color.White;
            back.ImageAlignment = Alignment.TopLeft;
            back.Tile = ImageTile.Horizontal | ImageTile.Vertical;
            back.UserVerticalScroll = UserMode.None;
            back.UserHorizontalScroll = UserMode.None;
            back.AddTo(header);

            Control topLine = new Control();
            topLine.AddTo(header, Alignment.TopLeft);
            topLine.Width = 320;
            topLine.Height = 1;

            var theme = new Control();
            theme.Top = 8;
            theme.Layout = new HorizontalList();
            theme.AutoSize = true;
            theme.AddTo(header, Alignment.TopLeft);

            var themeIcon = theme.Add(Image.LoadFromFont("Icons.ttf", 24, 0xEB7A), Alignment.LeftCenter);
            themeIcon.AutoSize = true;
            themeIcon.ImageColor = Color.Secondary;

            var themeText = theme.Add("Theme:", Alignment.LeftCenter);
            themeText.Enabled = false;
            themeText.TextPaddingRight = 0;
            themeText.Font = Font.Button;

            var themeWhite = theme.Add("White", Alignment.LeftCenter);
            themeWhite.TextColor = Color.Secondary;
            themeWhite.Enabled = true;
            themeWhite.Font = Font.Button;

            var themeToggle = new CheckBox();
            themeToggle.View = CheckBoxView.Toggle;
            themeToggle.Visible = true;
            themeToggle.Checked = false;
            themeToggle.ContentColor = Color.Secondary;
            themeToggle.ContentStyle = ColorStyle.Normal | ColorStyle.Disabled;
            themeToggle.AddTo(theme, Alignment.LeftCenter);

            var themeBlack = theme.Add("Black", Alignment.LeftCenter);
            themeBlack.TextColor = Color.Secondary;
            themeBlack.Enabled = false;
            themeBlack.Font = Font.Button;

            themeToggle.OnCheckedChanged += delegate
            {
                themeWhite.Enabled = !themeToggle.Checked;
                themeBlack.Enabled = themeToggle.Checked;
                themeIcon.Image = themeToggle.Checked ? Image.LoadFromFont("Icons.ttf", 24, 0xEC82) : Image.LoadFromFont("Icons.ttf", 24, 0xEB7A);
                Window.Skin = !themeToggle.Checked ? whiteSkin : blackSkin;
                App.Configuration["ThemeToggle"] = themeToggle.Checked;
                App.Configuration.Save();
            };

            locationItem = new LocationItem();
            locationItem.Alignment = Alignment.TopFill;
            locationItem.Top = 48;
            locationItem.AddTo(header);

            updater = Image.LoadFromFont("Icons.ttf", 16, (ushort)(0xE926));
            updater.Visible = false;
            updater.AddTo(locationItem.Icon, Alignment.BottomRight);

            locationItem.Icon.Layout = null;

            searchView = new SearchView();
            searchView.Width = 512;
            searchView.Alignment = Alignment.TopRight;
            searchView.AddTo(header);

            currentView = new CurrentView();
            currentView.Bottom = 48;
            currentView.Visible = false;
            currentView.AddTo(header, Alignment.BottomFill);
            currentView.OnSizeChanged += delegate { header.Height = currentView.Height + 232; };


            tools = new Control();
            tools.HorizontalScrollBar.Height = 2;
            tools.Visible = false;
            tools.Layout = new HorizontalList(0);
            tools.AutoHeight = true;
            tools.AddTo(header, Alignment.BottomFill);

            var intervalText = tools.Add("Interval:", Alignment.RightCenter);
            intervalText.Enabled = false;
            intervalText.TextPaddingRight = 0;
            intervalText.Font = Font.Button;

            var intervalText1 = tools.Add("1 hour", Alignment.RightCenter);
            intervalText1.TextColor = Color.Secondary;
            intervalText1.Enabled = false;
            intervalText1.Font = Font.Button;

            intervalToggle = new CheckBox();
            intervalToggle.View = CheckBoxView.Toggle;
            intervalToggle.Checked = true;
            intervalToggle.ContentColor = Color.Secondary;
            intervalToggle.ContentStyle = ColorStyle.Normal | ColorStyle.Disabled;
            intervalToggle.AddTo(tools, Alignment.RightCenter);

            var intervalText6 = tools.Add("6 hours", Alignment.RightCenter);
            intervalText6.TextColor = Color.Secondary;
            intervalText6.Font = Font.Button;

            intervalToggle.OnCheckedChanged += delegate
            {
                Update();
                intervalText1.Enabled = !intervalToggle.Checked;
                intervalText6.Enabled = intervalToggle.Checked;
                App.Configuration["IntervalToggle"] = intervalToggle.Checked;
                App.Configuration.Save();
            };

            var line = tools.Add("|", Alignment.RightCenter);


            var pressureText = tools.Add("Pressure units:", Alignment.RightCenter);
            pressureText.Enabled = false;

            pressureText.TextPaddingRight = 0;
            pressureText.Font = Font.Button;

            var pressureGPa = tools.Add("GPa", Alignment.RightCenter);
            pressureGPa.TextColor = Color.Secondary;
            pressureGPa.Font = Font.Button;

            pressureToggle = new CheckBox();
            pressureToggle.View = CheckBoxView.Toggle;
            pressureToggle.ContentColor = Color.Secondary;
            pressureToggle.ContentStyle = ColorStyle.Normal | ColorStyle.Disabled;
            pressureToggle.AddTo(tools, Alignment.RightCenter);

            var pressureHg = tools.Add("mmHg", Alignment.RightCenter);
            pressureHg.Enabled = false;
            pressureHg.TextColor = Color.Secondary;
            pressureHg.Font = Font.Button;


            pressureToggle.OnCheckedChanged += delegate
            {
                Update();
                pressureGPa.Enabled = !pressureToggle.Checked;
                pressureHg.Enabled = pressureToggle.Checked;
                App.Configuration["PressureToggle"] = pressureToggle.Checked;
                App.Configuration.Save();
            };


            tools.OnSizeChanged += delegate
            {
                intervalText.Visible = tools.Width > 500;
                pressureText.Visible = tools.Width > 500;
            };


            table = new Control();
            table.Color = Color.Background;
            table.Name = "table";
            table.Padding = 32;
            table.AutoHeight = true;
            table.Alignment = Alignment.Fill;
            table.Layout = new VerticalList(8);
            table.AddTo(this);

            Task.Factory.StartNew(MeteoProc);

            OnSizeChanged += delegate
            {
                if (Height < 800)
                {
                    table.BringToFront();
                    table.Alignment = Alignment.TopFill;
                }
                else
                {
                    table.SendToBack();
                    table.Alignment = Alignment.Fill;
                }
            };

            OnScroll += delegate
            {
                if (Height < 800 && ScrollY > header.Height - tools.Height - 16)
                {
                    header.Alignment = Alignment.NotLayouted | Alignment.TopFill;
                    header.Top = -header.Height + tools.Height + 16;
                    table.PaddingTop = 32 + (int)header.Height;
                }
                else
                {
                    header.Alignment = Alignment.TopFill;
                    table.PaddingTop = 32;
                }
            };

            SetId(App.Configuration["LocationId", ""]);
            intervalToggle.Checked = App.Configuration["IntervalToggle", true];
            pressureToggle.Checked = App.Configuration["PressureToggle", false];
            themeToggle.Checked = App.Configuration["ThemeToggle", false];
        }

        private void Header_OnSizeChanged(object sender, EventArgs e)
        {
            double width = header.Width - header.PaddingHorizontal;
            searchView.Width = width < 512 ? width : 512;
            table.Top = header.Height;
        }

        private void StartUpdate()
        {
            updater.Visible = true;
            updater.StartTimer("update", 10).Tick += (object sender, LX.Timer e) =>
            {
                updater.Rotation += (360 * e.ElapsedMilliseconds) / 1000.0;    // 1 rotate at 1 sec
            };
        }

        private void StopUpdate()
        {
            updater.StopTimer("update");
            updater.Rotation = 0;
            updater.Visible = false;
        }


        private Location location;
        private CurrentHour currentHour;
        private Forecast forecast;

        private void Update()
        {
            if (location == null)
            {
                return;
            }

            Window.SafeAction(() =>
            {

                tools.Visible = true;
                currentView.Visible = true;
                locationItem.Update(location);
                currentView.Update(currentHour);

                table.ScrollY = 0;
                table.Controls.Clear();

                var intervals = intervalToggle.Checked ? forecast.longIntervals : forecast.shortIntervals;

                int day = -1;
                for (int i = 0; i < intervals.Length; i++)
                {
                    var interval = intervals[i];

                    if (day != interval.start.Value.Day)
                    {
                        var dayItem = new Control();
                        dayItem.Height = 64;
                        dayItem.AddTo(table, Alignment.TopFill);

                        var dayText = dayItem.Add(interval.start.Value.ToString("dd.MM, dddd"), Alignment.HorizontalCenter);
                        dayText.Shape = CornerShape.Oval;
                        dayText.Radius = 8;
                        dayText.PaddingLeft = 8;
                        dayText.TextPaddingLeft = 12;
                        dayText.Font = Font.H3;
                        dayText.AutoWidth = false;
                        dayText.Enabled = false;
                        dayText.Color = Color.Primary.Auto(20);

                        Label dayWeatherText = null;
                        var dayInterval = forecast.dayIntervals.Where(p => p.start.Value.Day == interval.start.Value.Day).FirstOrDefault();
                        if (dayInterval != null)
                        {
                            dayWeatherText = dayItem.Add($"{Math.Round(dayInterval.temperature.min.Value)}° / {Math.Round(dayInterval.temperature.max.Value)}°", Alignment.RightCenter);
                            dayWeatherText.Shadow = ShadowStyle.Normal2;
                            dayWeatherText.Font = Font.H2;
                            dayWeatherText.Right = 12;

                            var dayWeatherIcon = dayWeatherText.Add(Controller.GetWeatherImage(dayInterval.twentyFourHourSymbol), Alignment.LeftCenter);
                            dayWeatherIcon.Size = 64;
                            dayWeatherIcon.Shadow = ShadowStyle.Strong2;
                        }


                        dayItem.OnSizeChanged += delegate
                        {
                            dayText.Text = dayItem.Width < 400 ? interval.start.Value.ToString("dd.MM") : interval.start.Value.ToString("dd.MM, dddd");
                            if (dayWeatherText != null)
                            {
                                dayWeatherText.Visible = dayItem.Width > 270;
                            }
                        };
                    }

                    day = interval.start.Value.Day;

                    var item = new IntervalItem(interval, pressureToggle.Checked);
                    item.AddTo(table);

                    var nextInterval = i < intervals.Length - 1 ? intervals[i + 1] : null;

                    if (nextInterval != null && nextInterval.start.Value.Day == day)
                    {
                        var line = new Control();
                        line.Height = 2;
                        line.PaddingLeft = 16;
                        line.PaddingRight = 16;
                        line.AddTo(table, Alignment.TopFill);

                        var innerLine = new Control();
                        innerLine.Color = Color.Primary.Auto(15);
                        innerLine.AddTo(line, Alignment.Fill);
                    }
                }

            });
        }

    }
}
