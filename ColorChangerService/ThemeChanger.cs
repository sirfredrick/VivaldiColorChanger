// Copyright (C) 2019 Jeffrey Tucker
//
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniParser;
using IniParser.Model;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;
namespace ColorChangerService
{
    public class ThemeChanger
    {
        readonly EventLog log;
        public ThemeChanger(EventLog log)
        {
            this.log = log;
        }
        public Boolean switchTheme(string themePath)
        {
            string path = System.Reflection.Assembly.GetEntryAssembly().Location;
            path = path.Substring(0, path.Length - 23);
            try
            {
                String msg = String.Empty;
                Process p = new Process();
                p.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                p.StartInfo.FileName = path + "ChangeTheme.bat";
                log.WriteEntry("Running: " + p.StartInfo.FileName + " " + p.StartInfo.Arguments);
                p.Start();
            }
            catch (Exception e)
            {
                log.WriteEntry("Could not load theme: " + e);
                return false;
            }
            return true;
        }
        public void changeColor(Color color)
        {
            string path = System.Reflection.Assembly.GetEntryAssembly().Location;
            log.WriteEntry("The Service executable path is: " + path);
            try
            {
                log.WriteEntry(color.GetHashCode().ToString());
                FileIniDataParser parser = new FileIniDataParser();
                try
                {
                    string themePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Local\\Microsoft\\Windows\\Themes\\Vivaldi.theme";
                    IniData data = parser.ReadFile(themePath);
                    string colorHex = "0x" + color.A.ToString("X2") + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
                    data["VisualStyles"]["ColorizationColor"] = "0x" + color.A.ToString("X2") + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
                    log.WriteEntry(colorHex);
                    parser.WriteFile(themePath, data);

                }
                catch (Exception e)
                {
                    log.WriteEntry("Could not parse .theme file: " + e);
                }
            }
            catch (Exception e)
            {
                log.WriteEntry("Unable to write to theme file: " + e);
            }
        }
    }
}