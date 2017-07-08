using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Banico
{
    public class GeoFileProcessor
    {
        public Dictionary<string, string> Continent = new Dictionary<string, string>();
        public Dictionary<string, Country> Countries = new Dictionary<string, Country>();
        public GeoFileProcessor(string countryFilename, string admin1Filename, string admin2Filename)
        {
            this.Continent.Add("AF", "Africa");
            this.Continent.Add("AS", "Asia");
            this.Continent.Add("EU", "Europe");
            this.Continent.Add("NA", "North America");
            this.Continent.Add("OC", "Oceania");
            this.Continent.Add("SA", "South America");
            this.Continent.Add("AN", "Antartica");

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
                        string continent = split[8];
                        country.Continent = this.Continent[continent];
                        country.Name = split[4];
                        country.Code = split[0];
                        country.Alias = this.ToAlias(country.Name);
                        this.Countries.Add(country.Code, country);
                    }
                }
            }
        }

        private string ToAlias(string name)
        {
            string output = name;
            output = output.ToLower();
            output = output.Replace(" ", "-");
            //output = output.Replace("-", "_");
            output = output.Replace("/", "-");
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            output = rgx.Replace(output, "");

            return output;
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
                        string admin1Id = split[0];
                        string countryCode = admin1Id.Split('.')[0];
                        string admin1Code= admin1Id.Split('.')[1];
                        admin1.Name = split[1];
                        admin1.Code = admin1Code;
                        admin1.Alias = this.ToAlias(split[2]);
                        if (this.Countries.ContainsKey(countryCode)) {
                            Country country = this.Countries[countryCode];
                            country.Admin1s.Add(admin1.Code, admin1);
                        }
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
                        string admin2Id = split[0];
                        string countryCode = admin2Id.Split('.')[0];
                        string admin1Code = admin2Id.Split('.')[1];
                        string admin2Code = admin2Id.Split('.')[2];
                        admin2.Name = split[1];
                        admin2.Code = admin2Code;
                        admin2.Alias = this.ToAlias(split[2]);
                        if (this.Countries.ContainsKey(countryCode)) {
                        Country country = this.Countries[countryCode];
                            if (country.Admin1s.ContainsKey(admin1Code))
                            {
                                Admin1 admin1 = country.Admin1s[admin1Code];
                                admin1.Admin2s.Add(admin2Code, admin2);
                            }

                        }
                    }
                }
            }
        }

        public void WriteOutput(string countryFilter, string outputFile)
        {
            FileStream fileStream = new FileStream(outputFile, FileMode.CreateNew);
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                foreach (Country country in this.Countries.Values.OrderBy(c => c.Name))
                {
                    if ((country.Name == countryFilter) || (string.IsNullOrEmpty(countryFilter))) {
                        foreach (Admin1 admin1 in country.Admin1s.Values.OrderBy(a1 => a1.Name))
                        {
                            foreach (Admin2 admin2 in admin1.Admin2s.Values.OrderBy(a2 => a2.Name))
                            {
                                writer.WriteLine(country.Continent + "," +
                                    this.ToAlias(country.Continent) + "," +
                                    country.Name + "," +
                                    country.Alias + "," +
                                    admin1.Name + "," +
                                    admin1.Alias + "," +
                                    admin2.Name + "," +
                                    admin2.Alias);
                            }
                        }
                    }
                }
            }
        }
    }
}