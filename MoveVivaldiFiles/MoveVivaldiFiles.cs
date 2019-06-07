// Copyright (C) 2019 Jeffrey Tucker
//
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.IO;
using IniParser;
using IniParser.Model;

namespace MoveVivaldiFiles
{
    class MoveVivaldiFiles
    {
        private static bool exit = false;
        private static string subKey = "";
        private static string pubKey = "";
        private static string initialPath = System.Reflection.Assembly.GetEntryAssembly().Location;
        private static string vivaldiPath = "";
        private static string initialBrowserPath = "";
        private static string browserPath = "";
        private static string path = "";
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                while (!exit)
                {
                    path = initialPath.Substring(0, initialPath.Length - 20);
                    vivaldiPath = args[0];
                    try
                    {
                        String[] fileList = Directory.GetFiles(vivaldiPath, "browser.html", SearchOption.AllDirectories);
                        initialBrowserPath = fileList[0];
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Could not find browser.html in " + vivaldiPath + " " + e);
                        Console.ReadKey();
                        exit = true;
                    }
                    browserPath = initialBrowserPath.Substring(0, initialBrowserPath.Length - 12);
                    ReadConfig();
                    Install();
                    MoveFiles();
                }
            }
        }
        static void ReadConfig()
        {
            try
            {
                FileIniDataParser parser = new FileIniDataParser();
                IniData data = parser.ReadFile(path + "ColorChangerService\\config.ini");
                subKey = data["configKeys"]["subKey"];
                pubKey = data["configKeys"]["pubKey"];
                Console.WriteLine(subKey);
                Console.WriteLine(pubKey);
            }
            catch (Exception e)
            {
                Console.WriteLine("Config File not able to be parsed: " + e);
                Console.ReadKey();
                exit = true;
            }
        }
        static void Install()
        {
            try
            {
                if (subKey != "" && pubKey != "")
                {
                    string text = File.ReadAllText(path + "Vivaldi\\custom.js");
                    text = text.Replace("<subKey>", subKey);
                    text = text.Replace("<pubKey>", pubKey);
                    File.WriteAllText(path + "Vivaldi\\custom.js", text);
                    Console.WriteLine("Pub and Sub Keys replaced in '\\Vivaldi\\custom.js'");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to Replace Sub and Pub Keys." + e);
                Console.ReadKey();
                exit = true;
            }
        }
        static void MoveFiles()
        {
            CopyWithBackup(path + "Vivaldi\\custom.js", browserPath + "\\custom.js", path + "Vivaldi\\backup\\custom.js");
            CopyWithBackup(path + "Vivaldi\\pubnub.js", browserPath + "\\pubnub.js", path + "Vivaldi\\backup\\pubnub.js");
            CopyWithBackup(path + "Vivaldi\\browser.html", browserPath + "\\browser.html", path + "Vivaldi\\backup\\browser.html");
            exit = true;
        }
        public static void CopyWithBackup(string sourceFileName, string destFileName, string backupFileName)
        {
            if (File.Exists(destFileName))
            {
                File.Copy(destFileName, backupFileName, true);
            }

            File.Copy(sourceFileName, destFileName, true);

        }
    }
}
