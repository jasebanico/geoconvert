using System;

namespace Banico
{
    public class GeoConvert
    {
        static void Main(string[] args)
        {
            string countryFilename = "assets/countryinfo.txt";
            string admin1Filename = "assets/admin1CodesASCII.txt";
            string admin2Filename = "assets/admin2Codes.txt";   
            GeoFileProcessor geoFileProcessor = new GeoFileProcessor(countryFilename, admin1Filename, admin2Filename);
            geoFileProcessor.WriteOutput("assets/output.csv");
        }
    }
}