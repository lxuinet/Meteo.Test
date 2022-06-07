using System;
using System.Collections.Generic;
using System.Text;

namespace Meteo.YRNO
{
    public class Forecast
    {
        public DateTime? created;
        public DateTime? update;
        public DayInterval[] dayIntervals;
        public Interval[] shortIntervals;
        public Interval[] longIntervals;
    }
}
