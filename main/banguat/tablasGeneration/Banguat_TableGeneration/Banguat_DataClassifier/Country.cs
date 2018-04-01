using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banguat_DataClassifier
{
    class Country
    {
        public string name = "";

        public Dictionary<string, Variable> variables = new Dictionary<string, Variable>();

        public Country(string name)
        {
            this.name = name;
        }
        
    }
}
