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

        static void Main(string[] args)
        {
            using (var reader = new StreamReader(@""))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    List<string> values = line.Split('\t').ToList();



                }
            }
        }
        
    }
}
