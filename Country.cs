using System.Collections.Generic;

namespace Banico
{
    public class Country
    {
        public string Name { get; set; }
        public string Alias { get; set; }

        public List<Admin1> Admin1s { get; set; }
    }
}