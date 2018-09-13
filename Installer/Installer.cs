// Copyright (C) 2018 Jeffrey Tucker
//
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace Installer
{
    static class Installer
    {
        static bool exit;
        static string fullPath = System.Reflection.Assembly.GetEntryAssembly().Location;
        static string path = fullPath.Substring(0, fullPath.Length - 13);

        static void Main(string[] args)
        {
            if (args.Length == 0 || (!args[0].Equals("uninstall") && !args[0].Equals("Uninstall")))
            {
                installer();
            }
            else
            {
                uninstaller();
                reset("\\ColorChangerService\\config.ini",
                    "[configKeys]\n" +
                    "subKey = <SubKey>\n"
                    );
                Console.WriteLine("Sub Key Reset in 'ColorChangerService\\config.ini'");
            }
            resetKeys();
            Console.WriteLine("");
            Console.WriteLine("Please restart vivaldi for the changes to take affect.");
            Console.WriteLine("");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void installer()
        {
            while (!exit)
            {
                Console.WriteLine("Welcome to the Color Changer Service Console Installer.");
                replaceKeys();
                installService();
                installVivaldiMod();
            }
        }
        static void uninstaller()
        {
            while (!exit)
            {
                Console.WriteLine("Welcome to the Color Changer Service Console Uninstaller.");
                uninstallService();
                Console.WriteLine("Service Successfully Uninstalled!");
                uninstallVivaldiMod();
                Console.WriteLine("Vivaldi Mod Successfully Uninstalled!");
            }
        }

        static void replaceKeys()
        {
            string oldConfigText = "";
            String SubKey = "";
            String PubKey = "";
            try
            {
                Console.WriteLine("Please Copy and Paste your PubNub Subsribe Key Below.");
                SubKey = Console.ReadLine();
                Console.WriteLine("Please Copy and Paste your PubNub Publish Key Below.");
                PubKey = Console.ReadLine();
                string text = File.ReadAllText(path + "\\ColorChangerService\\config.ini");
                oldConfigText = text;
                text = text.Replace("<SubKey>", SubKey);
                File.WriteAllText(path + "\\ColorChangerService\\config.ini", text);
                Console.WriteLine("Sub Key replaced in 'ColorChangerService\\config.ini'");
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to Replace Sub Key. Reverting to original config.ini file Error Code: \\n" + e);
                File.WriteAllText(path + "\\ColorChangerService\\config.ini", oldConfigText);
                reset("\\ColorChangerService\\config.ini", oldConfigText);
                exit = true;
            }
            string oldVivaldiText = "";
            try
            {
                if (SubKey != "" && PubKey != "")
                {
                    string text = File.ReadAllText(path + "\\Vivaldi\\custom.js");
                    oldVivaldiText = text;
                    text = text.Replace("<SubKey>", SubKey);
                    text = text.Replace("<PubKey>", PubKey);
                    File.WriteAllText(path + "\\Vivaldi\\custom.js", text);
                    Console.WriteLine("Pub and Sub Keys replaced in '\\Vivaldi\\custom.js'");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to Replace Sub and Pub Keys. Reverting to original custom.js file Error Code: \\n" + e);
                reset("\\Vivaldi\\custom.js", oldVivaldiText);
                exit = true;
            }
        }
        static void resetKeys()
        {
            try
            {
                reset("\\Vivaldi\\custom.js",
                    File.ReadAllText(path + "\\Vivaldi\\custom.txt")
                    );
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not find custom.txt at " + path + "\\Vivaldi\\custom.txt");
                Console.WriteLine("Please re-run uninstaller with the custom.txt to reset the custom.js file.");
                Console.WriteLine("");
                Console.WriteLine("Error code: " + e);
            }
            Console.WriteLine("Pub and Sub Keys Reset in '\\Vivaldi\\custom.txt'");
        }
        static void reset(string directory, string oldText)
        {
            File.WriteAllText(path + directory, oldText);
        }
        static void installService()
        {
            bool installExit = false;
            bool wasInstalled = false;
            while (!installExit)
            {
                Console.WriteLine("Commence Installation of the Color Changer Service? (Y/n)");
                String answer = Console.ReadLine();
                switch (answer)
                {
                    case "y":
                        installExit = true;
                        wasInstalled = install();
                        break;
                    case "Y":
                        installExit = true;
                        wasInstalled = install();
                        break;
                    case "n":
                        installExit = true;
                        break;
                    case "N":
                        installExit = true;
                        break;
                    case "":
                        installExit = true;
                        wasInstalled = install();
                        break;
                    default:
                        break;
                }
            }
            bool startExit = false;
            bool wasStarted = false;
            while (!startExit && wasInstalled)
            {
                Console.WriteLine("Installation Complete:");
                Console.WriteLine("Would you like to start Color Changer Service now? (Y/n)");
                String answer = Console.ReadLine();
                switch (answer)
                {
                    case "y":
                        startExit = true;
                        wasStarted = start();
                        break;
                    case "Y":
                        startExit = true;
                        wasStarted = start();
                        break;
                    case "n":
                        startExit = true;
                        wasStarted = false;
                        break;
                    case "N":
                        startExit = true;
                        wasStarted = false;
                        break;
                    case "":
                        startExit = true;
                        wasStarted = start();
                        break;
                    default:
                        break;
                }
            }
            if (wasStarted)
            {
                Console.WriteLine("The Service has been started!");
            }
            else
            {
                Console.WriteLine("The Service was not started.");

            }
        }
        static bool start()
        {
            var proc = new Process();
            proc.StartInfo.FileName = path + "\\ColorChangerService\\ColorChangerService.exe";
            proc.StartInfo.Arguments = "start";
            proc.Start();
            proc.WaitForExit();
            var exitCode = proc.ExitCode;
            proc.Close();
            return (exitCode == 0);
        }
        static bool install()
        {
            var proc = new Process();
            proc.StartInfo.FileName = path + "\\ColorChangerService\\ColorChangerService.exe";
            proc.StartInfo.Arguments = "install --localsystem --autostart";
            proc.Start();
            proc.WaitForExit();
            var exitCode = proc.ExitCode;
            proc.Close();
            return (exitCode == 0);
        }

        static void uninstallService()
        {
            bool uninstallExit = false;
            bool wasUninstalled = false;
            bool isDefault = false;
            while (!uninstallExit)
            {
                Console.WriteLine("Commence Uninstall of the Color Changer Service? (Y/n)");
                String answer = Console.ReadLine();
                switch (answer)
                {
                    case "y":
                        uninstallExit = true;
                        wasUninstalled = uninstall();
                        break;
                    case "Y":
                        uninstallExit = true;
                        wasUninstalled = uninstall();
                        break;
                    case "n":
                        uninstallExit = true;
                        break;
                    case "N":
                        uninstallExit = true;
                        break;
                    case "":
                        uninstallExit = true;
                        wasUninstalled = uninstall();
                        break;
                    default:
                        isDefault = true;
                        break;
                }
                if (!isDefault && !wasUninstalled)
                {
                    Console.WriteLine("Uninstall Failed.");
                }
            }
        }
        static bool uninstall()
        {
            var proc = new Process();
            proc.StartInfo.FileName = path + "\\ColorChangerService\\ColorChangerService.exe";
            proc.StartInfo.Arguments = "uninstall";
            proc.Start();
            proc.WaitForExit();
            var exitCode = proc.ExitCode;
            proc.Close();
            return (exitCode == 0);
        }


        static void installVivaldiMod()
        {
            bool vivaldiExit = false;
            while (!vivaldiExit)
            {
                Console.WriteLine("Would you like to install the accompanying Vivaldi Mod? (Y/n)");
                String answer = Console.ReadLine();
                switch (answer)
                {
                    case "y":
                        vivaldiExit = true;
                        break;
                    case "Y":
                        vivaldiExit = true;
                        break;
                    case "n":
                        vivaldiExit = true;
                        exit = true;
                        break;
                    case "N":
                        vivaldiExit = true;
                        exit = true;
                        break;
                    case "":
                        vivaldiExit = true;
                        break;
                    default:
                        break;
                }
            }
            if (!exit)
            {
                moveVivaldiFiles(getVivaldiPath());
            }
            exit = true;
        }
        static void uninstallVivaldiMod()
        {
            bool uninstallExit = false;
            bool wasUninstalled = false;
            bool isDefault = false;
            while (!uninstallExit)
            {
                Console.WriteLine("Commence Uninstall of the Vivaldi Mod? (Y/n)");
                String answer = Console.ReadLine();
                switch (answer)
                {
                    case "y":
                        uninstallExit = true;
                        wasUninstalled = deleteModFiles();
                        break;
                    case "Y":
                        uninstallExit = true;
                        wasUninstalled = deleteModFiles();
                        break;
                    case "n":
                        uninstallExit = true;
                        break;
                    case "N":
                        uninstallExit = true;
                        break;
                    case "":
                        uninstallExit = true;
                        wasUninstalled = deleteModFiles();
                        break;
                    default:
                        isDefault = true;
                        break;
                }
                if (!isDefault && !wasUninstalled)
                {
                    Console.WriteLine("Uninstall Failed.");
                }
            }
            exit = true;
        }
        static String getVivaldiPath()
        {
            bool pathExit = false;
            String browserPath = "";
            while (!pathExit)
            {
                Console.WriteLine("Please enter the path to your Vivaldi Folder.");
                try
                {
                    String vivaldiPath = Console.ReadLine();
                    String[] fileList = Directory.GetFiles(vivaldiPath, "browser.html", SearchOption.AllDirectories);
                    browserPath = fileList[0];
                    pathExit = true;
                }
                catch (Exception e)
                {

                }
            }
            return browserPath.Substring(0, browserPath.Length - 12);
        }
        static void moveVivaldiFiles(String vivaldiPath)
        {
            try
            {
                if (File.Exists(vivaldiPath + "\\browser.html") && !File.Exists(path + "Vivaldi\\backup\\browser.html"))
                {
                    File.Copy(vivaldiPath + "\\browser.html", path + "Vivaldi\\backup\\browser.html");

                }
                if (File.Exists(vivaldiPath + "\\browser.html") && !File.Exists(vivaldiPath + "\\browser.bak.html"))
                {
                    File.Copy(vivaldiPath + "\\browser.html", vivaldiPath + "\\browser.bak.html");
                }
                String[] files = {
                    "\\browser.html",
                    "\\custom.js",
                    "\\pubnub.js"
                };
                bool firstRun = true;
                foreach (String file in files)
                {

                    if (!firstRun)
                    {
                        if (File.Exists(vivaldiPath + file))
                        {
                            try
                            {
                                File.Delete(vivaldiPath + file);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                return;
                            }
                        }
                        File.Copy(path + "\\Vivaldi" + file, vivaldiPath + file);
                    }
                    else
                    {
                        String[] browserText = File.ReadAllLines(vivaldiPath + file);
                        List<String> browserList = browserText.OfType<String>().ToList();
                        int shift = 0;
                        List<String> toAdd = new List<String>();
                        
                        if (!browserList.Exists(x => x.Trim() == "<script src=\"pubnub.js\"></script>"))
                        {
                            toAdd.Add("<script src=\"pubnub.js\"></script>");
                            shift = 1;
                        }
                        if (!browserList.Exists(x => x.Trim() == "<script src=\"custom.js\"></script>"))
                        {
                            toAdd.Add("<script src=\"custom.js\"></script>");
                        }
                        if (toAdd.Count > 1) {
                            shift = 0;
                        }

                        browserList.InsertRange(browserList.Count - 2 - shift, toAdd);

                        String[] finalText = new String[browserList.Count + toAdd.Count];
                        finalText = browserList.ToArray<String>();
                        try
                        {
                            File.WriteAllLines(vivaldiPath + file, finalText);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Could not modify browser.html Error code: ");
                            Console.WriteLine(e);
                        }
                        firstRun = false;
                    }
                }
                Console.WriteLine("Vivaldi Mod successfully installed!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Vivaldi file(s) have failed to move to the Vivaldi folder: ");
                Console.WriteLine(e);
            }

        }
        static bool deleteModFiles()
        {
            bool isUninstalled = false;
            try
            {
                String vivaldiPath = getVivaldiPath();
                String[] files = {
                    "\\browser.html",
                    "\\custom.js",
                    "\\pubnub.js"
                };
                foreach (String file in files)
                {
                    if (File.Exists(vivaldiPath + file))
                    {
                        try
                        {
                            File.Delete(vivaldiPath + file);
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine(e.Message);
                            return false;
                        }
                    }
                }
                try
                {
                    File.Copy(path + "\\Vivaldi\\backup" + files[0], vivaldiPath + files[0]);
                    isUninstalled = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("browser.html has failed to be re-added to the Vivaldi folder,");
                    Console.WriteLine("please rename browser.bak.html in " + vivaldiPath + " to browser.html.");
                    Console.WriteLine("");
                    Console.WriteLine("Error code:");
                    Console.WriteLine(e);
                    isUninstalled = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Vivaldi file(s) have not been removed from the Vivaldi folder: ");
                Console.WriteLine(e);
                isUninstalled = false;
            }
            return isUninstalled;
        }
    }
}
