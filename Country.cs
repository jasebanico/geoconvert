using System.Collections.Generic;

namespace Banico
{
    public class Country
    {
        public string Name { get; set; }
        public string Alias { get; set; }

        public Dictionary<string, Admin1> Admin1s { get; set; }

        public Country()
        {
            this.Admin1s = new Dictionary<string, Admin1>();
        }
    }
}