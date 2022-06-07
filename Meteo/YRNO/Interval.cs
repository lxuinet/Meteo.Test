using System;
using System.Collections.Generic;
using System.Text;

namespace Meteo.YRNO
{
    public class Interval
    {
        public DateTime? start;
        public DateTime? end;
        public DateTime? nominalStart;
        public DateTime? nominalEnd;
        public CloudCover cloudCover;
        public Value dewPoint;
        public Value feelsLike;
        public Value humidity;
        public Value precipitation;
        public Value pressure;
        public Value temperature;
        public Value uvIndex;
        public Wind wind;
        public SymbolCode symbolCode;
        public Symbol symbol;
    }
}
