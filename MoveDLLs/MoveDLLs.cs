// Copyright (C) 2019 Jeffrey Tucker
//
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
