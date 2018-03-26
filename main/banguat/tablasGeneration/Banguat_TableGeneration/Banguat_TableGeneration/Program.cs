using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Banguat_TableGeneration
{
    class Program
    {
        public static string inputPathFolder = "C:\\Users\\liwuen\\Documents\\GitHub\\Banguat_Variable_DataRetriever\\main\\banguat\\data\\inputs_txt";
        public static string outputPathFolder = "C:\\Users\\liwuen\\Documents\\GitHub\\Banguat_Variable_DataRetriever\\main\\banguat\\data\\outputs";

        public static List<string> years = new List<string> { "2002", "2003", "2004", "2005", "2006", "2007", "2008", "2009", "2010", "2011", "2012", "2013", "2014", "2015", "2016" };

        public static Dictionary<int, string> MonthIndexes = new Dictionary<int, string>() { { 0, "Jan" }, { 1, "Feb" }, { 2, "Mar" }, { 3, "Apr" }, { 4, "May" }, { 5, "Jun" },
            { 6, "Jul" }, { 7, "Aug" }, { 8, "Sep" }, { 9, "Oct" }, { 10, "Nov" }, { 11, "Dec" } };

        public static string country = "usa";
        public static string variableName = "CAFE";
        public static string operationType = "exportacion";
        
        static void Main(string[] args)
        {
            Console.WriteLine("Loading files...");

            List<TableObjectRaw> loadedTableObjects = new List<TableObjectRaw>();
            for (int i = 0; i < years.Count(); i++)
            {
                Console.Write("   Year: " + years[i] + "... ");

                TableObjectRaw tableObjectRaw = ConstructTableObjectRawFromFile(inputPathFolder + "\\" + country, years[i]);
                loadedTableObjects.Add(tableObjectRaw);

                Console.WriteLine("Done!");
            }

            Console.Write("Verifying existence of variable: ");
            if (!VerifyVariableExistence(loadedTableObjects, variableName, operationType))
            {
                Console.WriteLine("ERROR: Variable does not exist in all the files.");
                return;
            }

            Console.Write("Constructing variable... ");

            Variable variable = ConstructVariableGivenYears(loadedTableObjects, variableName, operationType);

            Console.WriteLine("Done!");

            Console.WriteLine("Saving to files...");
            SaveToFile(variable, outputPathFolder + "\\" + country, operationType);

            Console.WriteLine("Done!");

            Console.ReadLine();
        }

        public static void SaveToFile(Variable variable, string resultPathFolder, string operationType)
        {
            string outPutFileName = resultPathFolder + "\\" + variable.name + " - " + operationType + ".txt";

            string fileContents = GenerateFileContents(variable, operationType);

            System.IO.Directory.CreateDirectory(resultPathFolder);

            System.IO.File.WriteAllText(outPutFileName, fileContents);


        }

        public static string GenerateFileContents(Variable variable, string operationType)
        {
            string result = "";

            Dictionary<string, List<string>> values = new Dictionary<string, List<string>>();

            if (operationType == "importacion")
            {
                values = variable.importaciones;
            }
            else
            {
                values = variable.exportaciones;
            }

            #region First line

            result += " , ";
            for (int i = 0; i <= 10; i++)
            {
                result += MonthIndexes[i] + " , ";
            }
            result += MonthIndexes[11] + "\n";

            #endregion

            #region Content of file

            foreach (KeyValuePair<string, List<string>> line in values)
            {
                string year = line.Key;
                List<string> months = line.Value;

                result += year + " , ";

                for (int i = 0; i < months.Count() - 1; i++)
                {
                    result += months[i] + " , ";
                }
                result += months.Last() + " \n";
            }

            #endregion

            return result;
        }
        
        public static Variable ConstructVariableGivenYears(List<TableObjectRaw> listOfTableObjects, string variableName, string operationType)
        {
            Variable variable = new Variable();

            variable.name = variableName;

            for (int i = 0; i < listOfTableObjects.Count(); i++)
            {
                string year = listOfTableObjects[i].year;
                List<string> months = new List<string>();

                if (operationType == "importacion")
                {
                    months = listOfTableObjects[i].importaciones[variableName];
                    variable.importaciones.Add(year, months);
                }
                else
                {
                    months = listOfTableObjects[i].exportaciones[variableName];
                    variable.exportaciones.Add(year, months);
                }
            }

            return variable;
        }

        /// <summary>
        /// Verifies if a given variable exists in all the tables.
        /// </summary>
        /// <param name="listOfTableObjects"></param>
        /// <param name="variable"></param>
        /// <param name="type">string: "importacion" or "exportacion"</param>
        /// <returns></returns>
        public static bool VerifyVariableExistence(List<TableObjectRaw> listOfTableObjects, string variableName, string operationType)
        {
            bool isInAllTables = true;

            for (int i = 0; i < listOfTableObjects.Count(); i++)
            {
                TableObjectRaw tableObjectRaw = listOfTableObjects[i];
                if (operationType == "importacion")
                {
                    if (!(tableObjectRaw.importaciones.ContainsKey(variableName)))
                    {
                        isInAllTables = false;

                        Console.WriteLine("Error: Year: " + tableObjectRaw.year + " does not contain " + variableName + " / " + operationType + ".");
                    }
                }
                else
                {
                    if (!(tableObjectRaw.exportaciones.ContainsKey(variableName)))
                    {
                        isInAllTables = false;

                        Console.WriteLine("Error: Year: " + tableObjectRaw.year + " does not contain " + variableName + " / " + operationType + ".");
                    }
                }
            }

            return isInAllTables;

        }


        /// <summary>
        /// For a given path, constructs the TableObject in memory.
        /// </summary>
        /// <param name="sourcePathFolder"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static TableObjectRaw ConstructTableObjectRawFromFile(string sourcePathFolder, string year)
        {
            TableObjectRaw tableObject = new TableObjectRaw();

            int row = 0;  //Indicates row

            bool importsBeforeExportsHeader = true;

            using (var reader = new StreamReader(@"" + sourcePathFolder + "\\" + year + ".txt"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    List<string> values = line.Split(',').ToList();


                    string importacionesLabel = "IMPORTACIONES";
                    string exportacionesLabel = "EXPORTACIONES";

                    if (row == 0)
                    {
                        row++;
                        if (
                            ((values[3].ToUpper() == importacionesLabel) && (values[5].ToUpper() == importacionesLabel) && (values[7].ToUpper() == importacionesLabel) &&
                            (values[9].ToUpper() == importacionesLabel) && (values[11].ToUpper() == importacionesLabel) && (values[13].ToUpper() == importacionesLabel) &&
                            (values[15].ToUpper() == importacionesLabel) && (values[17].ToUpper() == importacionesLabel) && (values[19].ToUpper() == importacionesLabel) &&
                            (values[21].ToUpper() == importacionesLabel) && (values[23].ToUpper() == importacionesLabel) && (values[25].ToUpper() == importacionesLabel)
                            ) && 
                            ((values[4].ToUpper() == exportacionesLabel) && (values[6].ToUpper() == exportacionesLabel) && (values[8].ToUpper() == exportacionesLabel) &&
                            (values[10].ToUpper() == exportacionesLabel) && (values[12].ToUpper() == exportacionesLabel) && (values[14].ToUpper() == exportacionesLabel) &&
                            (values[16].ToUpper() == exportacionesLabel) && (values[18].ToUpper() == exportacionesLabel) && (values[20].ToUpper() == exportacionesLabel) &&
                            (values[22].ToUpper() == exportacionesLabel) && (values[24].ToUpper() == exportacionesLabel) && (values[26].ToUpper() == exportacionesLabel)
                            ))
                        {
                            importsBeforeExportsHeader = true;
                        }else if (
                            ((values[3].ToUpper() == exportacionesLabel) && (values[5].ToUpper() == exportacionesLabel) && (values[7].ToUpper() == exportacionesLabel) &&
                            (values[9].ToUpper() == exportacionesLabel) && (values[11].ToUpper() == exportacionesLabel) && (values[13].ToUpper() == exportacionesLabel) &&
                            (values[15].ToUpper() == exportacionesLabel) && (values[17].ToUpper() == exportacionesLabel) && (values[19].ToUpper() == exportacionesLabel) &&
                            (values[21].ToUpper() == exportacionesLabel) && (values[23].ToUpper() == exportacionesLabel) && (values[25].ToUpper() == exportacionesLabel)
                            ) &&
                            ((values[4].ToUpper() == importacionesLabel) && (values[6].ToUpper() == importacionesLabel) && (values[8].ToUpper() == importacionesLabel) &&
                            (values[10].ToUpper() == importacionesLabel) && (values[12].ToUpper() == importacionesLabel) && (values[14].ToUpper() == importacionesLabel) &&
                            (values[16].ToUpper() == importacionesLabel) && (values[18].ToUpper() == importacionesLabel) && (values[20].ToUpper() == importacionesLabel) &&
                            (values[22].ToUpper() == importacionesLabel) && (values[24].ToUpper() == importacionesLabel) && (values[26].ToUpper() == importacionesLabel)
                            ))
                        {
                            importsBeforeExportsHeader = false;
                        }else
                        {
                            Console.WriteLine("ERROR: ERROR IN HEADER NAMES.");
                            return null;
                        }
                        
                        continue;
                    }

                    string variableName = values[0];

                    List<string> importaciones = null;
                    List<string> exportaciones = null;

                    if (importsBeforeExportsHeader)
                    {
                        importaciones = new List<string> { values[3], values[5], values[7], values[9], values[11], values[13], values[15], values[17], values[19], values[21], values[23], values[25] };
                        exportaciones = new List<string> { values[4], values[6], values[8], values[10], values[12], values[14], values[16], values[18], values[20], values[22], values[24], values[26] };
                    }else
                    {
                        exportaciones = new List<string> { values[3], values[5], values[7], values[9], values[11], values[13], values[15], values[17], values[19], values[21], values[23], values[25] };
                        importaciones = new List<string> { values[4], values[6], values[8], values[10], values[12], values[14], values[16], values[18], values[20], values[22], values[24], values[26] };
                    }
                    
                    tableObject.year = year;
                    tableObject.importaciones.Add(variableName, importaciones);
                    tableObject.exportaciones.Add(variableName, exportaciones);

                    row++;
                }
            }

            return tableObject;
        }


        public class TableObjectRaw
        {
            public TableObjectRaw()
            {
                importaciones = new Dictionary<string, List<string>>();
                exportaciones = new Dictionary<string, List<string>>();
            }

            public string year;
            public Dictionary<string, List<string>> importaciones;  //variable vs. months
            public Dictionary<string, List<string>> exportaciones;  //variable vs. months

        }

        public class Variable
        {
            public Variable()
            {
                importaciones = new Dictionary<string, List<string>>();
                exportaciones = new Dictionary<string, List<string>>();
            }

            public string name;
            public Dictionary<string, List<string>> importaciones;  //year vs. months
            public Dictionary<string, List<string>> exportaciones;  //year vs. months
        }

    }
}
