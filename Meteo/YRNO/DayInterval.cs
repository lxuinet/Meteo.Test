using System;
using System.Collections.Generic;
using System.Text;

namespace Meteo.YRNO
{
    public class DayInterval
    {
        public DateTime? start;
        public DateTime? end;
        public Wind wind;
        public Value precipitation;
        public Temperature temperature;
        public string[] sixHourSymbols;
        public string[] twelveHourSymbols;
        public string twentyFourHourSymbol;
    }
}
