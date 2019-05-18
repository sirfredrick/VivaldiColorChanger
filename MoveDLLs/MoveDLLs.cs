using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoveDLLs
{
    class MoveDLLs
    {
        private static bool exit = false;
        private static string initialPath = System.Reflection.Assembly.GetEntryAssembly().Location;
        private static string programPath = "";
        private static string path = "";
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                while (!exit)
                {
                    path = initialPath.Substring(0, initialPath.Length - 12);
                    programPath = args[0];
                    MoveFiles();
                }
            }
        }
        static void MoveFiles()
        {
            File.Copy(path + "\\B18.0206.1\\DLLs\\Motherboard\\GLedApi.dll", programPath + "\\GLedApi.dll", true);
            File.Copy(path + "\\B18.0206.1\\DLLs\\Motherboard\\layout.ini", programPath + "\\layout.ini", true);
            File.Copy(path + "\\B18.0206.1\\DLLs\\Motherboard\\ycc.dll", programPath + "\\ycc.dll", true);
            exit = true;
        }
    }
}
