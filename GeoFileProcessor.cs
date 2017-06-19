using System;
using System.Collections.Generic;
using System.IO;

namespace Banico
{
    public class GeoFileProcessor
    {
        public Dictionary<string, Country> Countries = new Dictionary<string, Country>();
        public GeoFileProcessor(string countryFilename, string admin1Filename, string admin2Filename)
        {
            this.ReadCountryFile(countryFilename);
            this.ReadAdmin1File(admin1Filename);
            this.ReadAdmin2File(admin2Filename);
        }

        private void ReadCountryFile(string countryFilename)
        {
            FileStream fileStream = new FileStream(countryFilename, FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine().Trim();
                    if ((!string.IsNullOrEmpty(line)) && (!line.StartsWith("#")))
                    {
                        string[] split = line.Split('\t');
                        Country country = new Country();
                        country.Name = split[4];
                        country.Alias = split[0];
                        this.Countries.Add(country.Alias, country);
                    }
                }
            }
        }

        private void ReadAdmin1File(string admin1Filename)
        {
            FileStream fileStream = new FileStream(admin1Filename, FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    if ((!string.IsNullOrEmpty(line)) && (!line.StartsWith("#")))
                    {
                        string[] split = line.Split('\t');
                        Admin1 admin1 = new Admin1();
                        string admin1Code = split[0];
                        string countryAlias = admin1Code.Split('.')[0];
                        string admin1Alias = admin1Code.Split('.')[1];
                        admin1.Name = split[1];
                        admin1.Alias = admin1Alias;
                        Country country = this.Countries[countryAlias];
                        country.Admin1s.Add(admin1.Alias, admin1);
                    }
                }
            }
        }

        private void ReadAdmin2File(string admin2File)
        {
            FileStream fileStream = new FileStream(admin2File, FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    if ((!string.IsNullOrEmpty(line)) && (!line.StartsWith("#")))
                    {
                        string[] split = line.Split('\t');
                        Admin2 admin2 = new Admin2();
                        string admin2Code = split[0];
                        string countryAlias = admin2Code.Split('.')[0];
                        string admin1Alias = admin2Code.Split('.')[1];
                        string admin2Alias = admin2Code.Split('.')[2];
                        admin2.Name = split[1];
                        admin2.Alias = admin2Alias;
                        Country country = this.Countries[countryAlias];
                        if (country.Admin1s.ContainsKey(admin1Alias))
                        {
                            Admin1 admin1 = country.Admin1s[admin1Alias];
                            admin1.Admin2s.Add(admin2Alias, admin2);
                            Console.WriteLine(country.Name + "," + country.Alias + "," + admin1.Name + "," + admin1.Alias + "," + admin2.Name + "," + admin2.Alias);
                        }
                    }
                }
            }
        }
    }
}