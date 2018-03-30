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


        static List<string> countries = new List<string>() { "belgium", "canada", "costarica", "germany", "italy", "japan", "mexico", "south korea", "taiwan", "usa" };
        static int initialYear = 2002;
        static int finalYear = 2017;

        static bool ignoreTotalVariable = true;  //Generates "Total" variable (line 1)


        static void Main(string[] args)
        {
            using (var reader = new StreamReader(@"" + inputsFolder + countries[0] +"\\2002.txt"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    List<string> values = line.Split('\t').ToList();



                }
            }
        }

        public static void FillCountryData(string country)
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
                        }
                        else
                        {
                            //Find previous variable.
                        }



                        counter++;
                    }
                }
            }
        }
        
    }
}
