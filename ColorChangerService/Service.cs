// Copyright (C) 2018 Jeffrey Tucker
//
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using Topshelf;
using System.Diagnostics;

namespace ColorChangerService
{
    static class Service
    {
        static void Main(string[] args)
        {
            if (!EventLog.SourceExists("Color Changer Service"))
            {
                EventLog.CreateEventSource("Color Changer Service", "Application Log");
                return;
            }

            EventLog log = new EventLog();
            log.Source = "Color Changing Service";

            HostFactory.Run(x =>
            {
                x.Service<ColorChanger>(s =>
                {
                    s.ConstructUsing(name => new ColorChanger(log));
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.SetDescription("Changes the RGB Color of Gigabyte Motherboards to a random color every three seconds.");
                x.SetDisplayName("Color Changer");
                x.SetServiceName("ColorChanger");
            });
        }
    }
}
