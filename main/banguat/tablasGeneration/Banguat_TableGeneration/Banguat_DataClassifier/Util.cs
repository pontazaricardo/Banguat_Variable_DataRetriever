using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banguat_DataClassifier
{
    class Util
    {
        public static bool GenerateMonths = true;

        public static Dictionary<string, int> positionsInArray = new Dictionary<string, int>() { { "2002", 0 }, { "2003", 1 }, { "2004", 2 }, { "2005", 3 }, { "2006", 4 },
            { "2007", 5 }, { "2008", 6 }, { "2009", 7 }, { "2010", 8 }, { "2011", 9 }, { "2012", 10 }, { "2013", 11 }, { "2014", 12 }, { "2015", 13 }, { "2016", 14 }, { "2017", 15 } };

        /// <summary>
        /// This function returns the correct position for a month based on the column in the raw data.
        /// Jan = 0: 3,4
        /// Feb = 1: 5,6
        /// 
        /// As:
        /// Column 0 = variable name
        /// Column 1 = Total import
        /// Column 2 = Total export
        /// </summary>
        /// <param name="monthNumber"></param>
        /// <returns></returns>
        public static int GetMonthBasedOnColumnNumber(int monthNumber)
        {
            int result = 0;

            if (monthNumber % 2 == 1)
            {
                monthNumber++;
            }
            //At this point, monthNumber is even.

            result = monthNumber / 2;
            result = result - 2;
            return result;
        }

        /// <summary>
        /// Creates 
        /// </summary>
        /// <param name="countries"></param>
        /// <param name="outputFolderRoot"></param>
        public static void SaveVariablesPerCountry(List<Country> countries, string outputFolderRoot)
        {
            Console.WriteLine("Printing all variables data!");

            foreach (Country country in countries)
            {
                Console.WriteLine("\n-----------\n" + country.name + "\n-----------\n");

                string outputFolderImports = outputFolderRoot + country.name + "\\imports";
                string outputFolderExports = outputFolderRoot + country.name + "\\exports";

                System.IO.Directory.CreateDirectory(outputFolderImports);
                System.IO.Directory.CreateDirectory(outputFolderExports);


                foreach (KeyValuePair<string, Variable> kvpair in country.variables)
                {
                    string variableName = kvpair.Key;
                    Variable variable = kvpair.Value;

                    Console.Write(variableName + ": ");

                    if (!variable.ContainsAllData())
                    {
                        Console.WriteLine(" MISSING IN YEARS: " + variable.ReportMissingYears());
                        continue;
                    }

                    string toPrintImports = "";
                    string toPrintExports = "";

                    if (GenerateMonths)
                    {
                        toPrintImports = " , Jan , Feb , Mar , Apr , May , Jun , Jul , Aug , Sep , Oct , Nov , Dec\n";
                        toPrintExports = toPrintImports;
                    }

                    for (int row = 0; row < 16; row++)
                    {
                        int year = 2002 + row;

                        toPrintImports += year + " , ";
                        toPrintExports += year + " , ";

                        for (int column = 0; column < 11; column++)
                        {
                            toPrintImports += variable.imports[row, column] + " , ";
                            toPrintExports += variable.exports[row, column] + " , ";
                        }

                        toPrintImports += variable.imports[row, 11] + "\n";
                        toPrintExports += variable.exports[row, 11] + "\n";
                    }

                    string fileNameImports = outputFolderImports + "\\" + variableName.Replace("\"", "") + ".csv";
                    string fileNameExports = outputFolderExports + "\\" + variableName.Replace("\"", "") + ".csv";

                    System.IO.File.WriteAllText(fileNameImports, toPrintImports);
                    System.IO.File.WriteAllText(fileNameExports, toPrintExports);

                    Console.WriteLine("Done!");
                }
            }
        }

        public static void SaveMatlabVariables(List<Country> countries, string outputFolderRoot)
        {
            Console.WriteLine("Printing all matlab variables data!");

            foreach (Country country in countries)
            {
                Console.WriteLine("\n-----------\n" + country.name + "\n-----------\n");

                string outputFolderImports = outputFolderRoot + country.name + "\\imports";
                string outputFolderExports = outputFolderRoot + country.name + "\\exports";

                System.IO.Directory.CreateDirectory(outputFolderImports);
                System.IO.Directory.CreateDirectory(outputFolderExports);


                foreach (KeyValuePair<string, Variable> kvpair in country.variables)
                {
                    string variableName = kvpair.Key;
                    Variable variable = kvpair.Value;

                    Console.Write(variableName + ": ");

                    if (!variable.ContainsAllData())
                    {
                        Console.WriteLine(" MISSING IN YEARS: " + variable.ReportMissingYears());
                        continue;
                    }

                    string toPrintImports = "";
                    string toPrintExports = "";

                    for (int row = 0; row < 16; row++)
                    {
                        for (int column = 0; column < 12; column++)
                        {
                            if((row==15) && (column == 11))
                            {
                                toPrintImports += variable.imports[row, column];
                                toPrintExports += variable.exports[row, column];
                            }
                            else
                            {
                                toPrintImports += variable.imports[row, column] + "\n";
                                toPrintExports += variable.exports[row, column] + "\n";
                            }

                            
                        }
                        
                    }

                    string fileNameImports = outputFolderImports + "\\" + matlabFileNameCorrection(variableName) + ".m";
                    string fileNameExports = outputFolderExports + "\\" + matlabFileNameCorrection(variableName) + ".m";

                    System.IO.File.WriteAllText(fileNameImports, toPrintImports);
                    System.IO.File.WriteAllText(fileNameExports, toPrintExports);

                    Console.WriteLine("Done!");
                }
            }
        }

        private static string matlabFileNameCorrection(string str)
        {
            str = str.Replace("\"", "");

            str = UppercaseWords(str);

            str = str.Replace(" ", "");

            return str;
        }

        private static string UppercaseWords(string value)
        {
            char[] array = value.ToCharArray();
            // Handle the first letter in the string.
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }
            // Scan through the letters, checking for spaces.
            // ... Uppercase the lowercase letters following spaces.
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }


    }
}
