using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using LX;
using Newtonsoft.Json;

namespace Meteo.YRNO
{
    public static class Controller
    {
        private static Dictionary<string, IdName> symbols = new Dictionary<string, IdName>();
        static Controller()
        {
            symbols.Add("clearsky", new IdName("01", "Clear sky"));
            symbols.Add("cloudy", new IdName("04", "Cloudy"));
            symbols.Add("fair", new IdName("02", "Fair"));
            symbols.Add("fog", new IdName("15", "Fog"));
            symbols.Add("heavyrain", new IdName("10", "Heavy rain"));
            symbols.Add("heavyrainandthunder", new IdName("11", "Heavy rain and thunder"));
            symbols.Add("heavyrainshowers", new IdName("41", "Heavy rain showers"));
            symbols.Add("heavyrainshowersandthunder", new IdName("25", "Heavy rain showers and thunder"));
            symbols.Add("heavysleet", new IdName("48", "Heavy sleet"));
            symbols.Add("heavysleetandthunder", new IdName("32", "Heavy sleet and thunder"));
            symbols.Add("heavysleetshowers", new IdName("43", "Heavy sleet showers"));
            symbols.Add("heavysleetshowersandthunder", new IdName("27", "Heavy sleet showers and thunder"));
            symbols.Add("heavysnow", new IdName("50", "Heavy snow"));
            symbols.Add("heavysnowandthunder", new IdName("34", "Heavy snow and thunder"));
            symbols.Add("heavysnowshowers", new IdName("45", "Heavy snow showers"));
            symbols.Add("heavysnowshowersandthunder", new IdName("29", "Heavy snow showers and thunder"));
            symbols.Add("lightrain", new IdName("46", "Light rain"));
            symbols.Add("lightrainandthunder", new IdName("30", "Light rain and thunder"));
            symbols.Add("lightrainshowers", new IdName("40", "Light rain showers"));
            symbols.Add("lightrainshowersandthunder", new IdName("24", "Light rain showers and thunder"));
            symbols.Add("lightsleet", new IdName("47", "Light sleet"));
            symbols.Add("lightsleetandthunder", new IdName("31", "Light sleet and thunder"));
            symbols.Add("lightsleetshowers", new IdName("42", "Light sleet showers"));
            symbols.Add("lightsnow", new IdName("49", "Light snow"));
            symbols.Add("lightsnowandthunder", new IdName("33", "Light snow and thunder"));
            symbols.Add("lightsnowshowers", new IdName("44", "Light snow showers"));
            symbols.Add("lightssleetshowersandthunder", new IdName("26", "Light sleet showers and thunder"));
            symbols.Add("lightssnowshowersandthunder", new IdName("28", "Light snow showers and thunder"));
            symbols.Add("partlycloudy", new IdName("03", "Partly cloudy"));
            symbols.Add("rain", new IdName("09", "Rain"));
            symbols.Add("rainandthunder", new IdName("22", "Rain and thunder"));
            symbols.Add("rainshowers", new IdName("05", "Rain showers"));
            symbols.Add("rainshowersandthunder", new IdName("06", "Rain showers and thunder"));
            symbols.Add("sleet", new IdName("12", "Sleet"));
            symbols.Add("sleetandthunder", new IdName("23", "Sleet and thunder"));
            symbols.Add("sleetshowers", new IdName("07", "Sleet showers"));
            symbols.Add("sleetshowersandthunder", new IdName("20", "Sleet showers and thunder"));
            symbols.Add("snow", new IdName("13", "Snow"));
            symbols.Add("snowandthunder", new IdName("14", "Snow and thunder"));
            symbols.Add("snowshowers", new IdName("08", "Snow showers"));
            symbols.Add("snowshowersandthunder", new IdName("21", "Snow showers and thunder"));
        }

        private static T Get<T>(string url)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                var json = webClient.DownloadString(new Uri(url));
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public static Location[] Find(string name)
        {
            var result = string.IsNullOrWhiteSpace(name) 
                ? null 
                : Get<FindResult>($"https://www.yr.no/api/v0/locations/suggest?language=en&q={WebUtility.HtmlEncode(name)}");

            return result == null || result._embedded == null || result._embedded.location == null 
                ? new Location[0] 
                : result._embedded.location;
        }

        public static Location GetLocation(string id)
        {
            return Get<Location>($"https://www.yr.no/api/v0/locations/{id}?language=en");
        }

        public static CurrentHour GetCurrentHour(string id)
        {
            return Get<CurrentHour>($"https://www.yr.no/api/v0/locations/{id}/forecast/currenthour?language=en");
        }

        public static Forecast GetForecast(string id)
        {
            return Get<Forecast>($"https://www.yr.no/api/v0/locations/{id}/forecast?language=en");
        }

        private static Dictionary<string, Image> weatherImages = new Dictionary<string, Image>();
        public static Image GetWeatherImage(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            Image result = null;

            lock (weatherImages)
            {
                if (!weatherImages.TryGetValue(id, out result))
                {

                    var parts = id.ToLower().Split('_');

                    IdName symbol = null;
                    symbols.TryGetValue(parts[0], out symbol);


                    if (symbol != null)
                    {
                        string code = "";
                        if (parts.Length == 2)
                        {
                            code = parts[1][0].ToString();
                        }

                        result = Image.LoadFromResource($"*100.{symbol.id}{code}.png");
                    }

                    weatherImages[id] = result;
                }

            }

            return result;
        }

        private static Dictionary<string, Image> weatherSmallImages = new Dictionary<string, Image>();
        public static Image GetWeatherSmallImage(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            Image result = null;

            lock (weatherSmallImages)
            {
                if (!weatherSmallImages.TryGetValue(id, out result))
                {

                    var parts = id.ToLower().Split('_');

                    IdName symbol = null;
                    symbols.TryGetValue(parts[0], out symbol);


                    if (symbol != null)
                    {
                        string code = "";
                        if (parts.Length == 2)
                        {
                            code = parts[1][0].ToString();
                        }

                        result = Image.LoadFromResource($"*48.{symbol.id}{code}.png");
                    }

                    weatherSmallImages[id] = result;
                }

            }

            return result;
        }
    }
}
