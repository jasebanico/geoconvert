using System.Collections.Generic;

namespace Banico
{
    public class Admin1
    {
        public string Name { get; set; }
        public string Alias { get; set; }

        public string Code { get; set; }

        public Dictionary<string, Admin2> Admin2s { get; set; }

        public Admin1()
        {
            this.Admin2s = new Dictionary<string, Admin2>();
        }

    }
}