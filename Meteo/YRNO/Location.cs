using System;
using System.Collections.Generic;
using System.Text;

namespace Meteo.YRNO
{
    public class Location
    {
        public string id;
        public string name;
        public string timeZone;
        public bool? isInOcean;
        public int? elevation; 
        public IdName country;
        public Position position;
        public IdName region;
        public IdName subregion;
        public IdName category;
        public string urlPath;
    }
}
