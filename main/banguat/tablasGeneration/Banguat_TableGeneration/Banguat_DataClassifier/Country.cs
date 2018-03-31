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

        private List<Variable> variables { get; set; }

        public Country(string name)
        {
            this.name = name;
            this.variables = new List<Variable>();
        }
        
    }
}
