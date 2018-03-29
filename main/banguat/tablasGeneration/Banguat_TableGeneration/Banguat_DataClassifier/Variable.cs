using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Banguat_DataClassifier;

namespace Banguat_DataClassifier
{
    class Variable
    {
        string name = "";

        public string[,] imports = new string[16, 12];
        public string[,] exports = new string[16, 12];

        public bool[] yearFlags = new bool[] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };

        public Variable(string name)
        {
            this.name = name;
        }
        
        public void FillData(string year, string[] data, string[] headers)
        {
            int row = Banguat_DataClassifier.Util.positionsInArray[year];

            if ((data.Count() != 27) || (headers.Count()!=27))//There must be 27 columns
                throw new System.ArgumentException("Variable " + name + " - " + year + " does not contain 27 columns. Check original file.");

            for(int i = 3; i < data.Count(); i++)
            {
                string headerName = headers[i];
                string value = data[i];

                int column = Banguat_DataClassifier.Util.GetMonthBasedOnColumnNumber(i);

                fillRows(row, column, headerName, value);
            }

            yearFlags[row] = true;
        }

        private void fillRows(int row, int column, string type, string value)
        {
            if(type== "Importaciones")
            {
                imports[row, column] = value;
            }else if(type== "Exportaciones")
            {
                exports[row, column] = value;
            }else
            {
                throw new System.ArgumentException("Type " + type + " is not a valid column header. Check original file.");
            }
        }


        /// <summary>
        /// Indicates if this variable contains all the data for all the years
        /// </summary>
        /// <returns></returns>
        public bool ContainsAllData()
        {
            bool containsAllData = yearFlags[0] && yearFlags[1] && yearFlags[2] && yearFlags[3] && yearFlags[4] && yearFlags[5] && yearFlags[6] && yearFlags[7] && yearFlags[8] &&
                                    yearFlags[9] && yearFlags[10] && yearFlags[11] && yearFlags[12] && yearFlags[13] && yearFlags[14] && yearFlags[15];

            return containsAllData;
        }

    }
}
