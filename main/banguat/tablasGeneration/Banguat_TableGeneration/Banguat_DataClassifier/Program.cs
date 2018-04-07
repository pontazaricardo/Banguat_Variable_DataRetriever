using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Banguat_DataClassifier
{
    class Program
    {
        static string inputsFolder = "C:\\Users\\liwuen\\Documents\\GitHub\\Banguat_Variable_DataRetriever\\main\\banguat\\data\\inputs_txt\\";
        static string outputsVariablesGroupedFolder = "C:\\Users\\liwuen\\Documents\\GitHub\\Banguat_Variable_DataRetriever\\main\\banguat\\data\\outputs_tables\\";

        static List<Country> countries = new List<Country>();

        static int initialYear = 2002;
        static int finalYear = 2017;

        static bool ignoreTotalVariable = true;  //Generates "Total" variable (line 1)


        static void Main(string[] args)
        {

            #region Create countries

            countries.Add(new Country("belgium"));
            countries.Add(new Country("canada"));
            countries.Add(new Country("costarica"));
            countries.Add(new Country("germany"));
            countries.Add(new Country("italy"));
            countries.Add(new Country("japan"));
            countries.Add(new Country("mexico"));
            countries.Add(new Country("south korea"));
            countries.Add(new Country("taiwan"));
            countries.Add(new Country("usa"));

            #endregion

            foreach (Country country in countries)
            {
                country.variables = FillCountryData(country.name);
            }

            Util.SaveVariablesPerCountry(countries, outputsVariablesGroupedFolder);

        }

        public static Dictionary<string, Variable> FillCountryData(string country)
        {
            Dictionary<string, Variable> variables = new Dictionary<string, Variable>();

            for (int year= initialYear; year <= finalYear; year++)
            {
                using (var reader = new StreamReader(@"" + inputsFolder + country + "\\"+ year + ".txt"))
                {
                    string[] headers = new string[] { "", "Importaciones", "Exportaciones",	"Importaciones", "Exportaciones", "Importaciones", "Exportaciones", "Importaciones", "Exportaciones",
                                "Importaciones", "Exportaciones", "Importaciones", "Exportaciones", "Importaciones", "Exportaciones", "Importaciones", "Exportaciones", "Importaciones", "Exportaciones",
                                "Importaciones", "Exportaciones", "Importaciones", "Exportaciones", "Importaciones", "Exportaciones", "Importaciones", "Exportaciones"};

                    int counter = 0;

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        string[] values = line.Split('\t').ToArray();

                        if (counter == 0)
                        {
                            //header lines
                            headers = values;
                            counter++;

                            continue;

                        }

                        string variableName = values[0];

                        if ((variableName == "TOTAL"))
                        {
                            if (ignoreTotalVariable)
                            {
                                counter++;
                                continue;
                            }
                        }

                        if (!variables.ContainsKey(variableName))
                        {
                            //This is a new variable. Add it to the dictionary.
                            Variable variablePivot = new Variable(variableName);
                            variablePivot.FillData(year, values, headers);

                            variables.Add(variableName, variablePivot);
                        }
                        else
                        {
                            //Find previous variable.
                            variables[variableName].FillData(year, values, headers);
                        }
                        counter++;
                    }
                }
            }

            return variables;
        }
        
    }
}
