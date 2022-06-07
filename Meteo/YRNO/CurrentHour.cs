using System;
using System.Collections.Generic;
using System.Text;

namespace Meteo.YRNO
{
    public class CurrentHour
    {
        public DateTime? created;
        public Value precipitation;
        public Status status;
        public SymbolCode symbolCode;
        public Temperature temperature;
        public Wind wind;
    }
}
