using System;

namespace Banico
{
    public class GeoConvert
    {
        static void Main(string[] args)
        {
            string country = "assets/countryinfo.txt";
            string admin1 = "assets/admin1CodesASCII.txt";
            string admin2 = "admin2Codes.txt";   
            GeoFileProcessor geoFileProcessor = new GeoFileProcessor(country, admin1, admin2);
        }
    }
}