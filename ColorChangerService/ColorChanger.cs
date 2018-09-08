// Copyright (C) 2018 Jeffrey Tucker
//
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Drawing;
using GLedApiDotNet;
using GLedApiDotNet.LedSettings;
using PubnubApi;
using Newtonsoft.Json;
using System.Diagnostics;
using IniParser;
using IniParser.Model;

namespace ColorChangerService
{
    public class ColorChanger
    {
        EventLog log;
        IRGBFusionMotherboard motherboardLEDs = new LazyMotherboard();
        Pubnub pubnub;


        public ColorChanger(EventLog log)
        {
            this.log = log;
            PNConfiguration config = new PNConfiguration();
            string configKey = "";
            string path = System.Reflection.Assembly.GetEntryAssembly().Location;
            log.WriteEntry("The Service executable path is: " + path, EventLogEntryType.Information);
            try
            {
                FileIniDataParser parser = new FileIniDataParser();
                IniData data = parser.ReadFile(path.Substring(0, path.Length - 23) + "\\config.ini");
                configKey = data["configKeys"]["subKey"];
            }
            catch (Exception e)
            {

                log.WriteEntry("Config File not able to be parsed: " + e, EventLogEntryType.Error);
            }
            if (configKey != "" || configKey != null)
            {
                config.SubscribeKey = configKey;
            }
            else
            {
                config.SubscribeKey = "";
                log.WriteEntry("Subscribe Key not entered in config.ini file.", EventLogEntryType.Error);
            }
            pubnub = new Pubnub(config);
        }

        public void Start()
        {
            log.WriteEntry("On Start", EventLogEntryType.Information, 2, 0);
            pubnub.AddListener(new SubscribeCallbackExt(
                (pubnubObj, message) =>
                {
                    dynamic json = JsonConvert.DeserializeObject(message.Message.ToString());
                    String hexVal = json.message;
                    Color tabColor = ColorTranslator.FromHtml(hexVal);
                    Console.WriteLine(tabColor);
                    ChangeColor(tabColor);



                    if (message != null)
                    {

                        if (message.Channel != null)
                        {
                            log.WriteEntry("Color: " + hexVal + " has been received through " + message.Channel + " channel.", EventLogEntryType.Information, 10, 0);
                        }
                    }
                },
                (pubnubObj, presence) => { },
                    (pubnubObj, status) =>
                    {
                        if (status.Category == PNStatusCategory.PNUnexpectedDisconnectCategory)
                        {
                            log.WriteEntry("An unexpected disconnection has occured", EventLogEntryType.Information, 5, 0);
                        }
                        else if (status.Category == PNStatusCategory.PNConnectedCategory)
                        {
                            log.WriteEntry("Connection to PubNub Successful", EventLogEntryType.Information, 15, 0);
                        }
                        else if (status.Category == PNStatusCategory.PNReconnectedCategory)
                        {
                            log.WriteEntry("Reconnection to PubNub Successful", EventLogEntryType.Information, 25, 0);
                        }

                    }));

            pubnub.Subscribe<string>()
    .Channels(new string[] {
            "Vivaldi RGB"
    })
    .Execute();
        }

        public void Stop()
        {
            log.WriteEntry("On Stop", EventLogEntryType.Information, 3, 0);
            pubnub.Unsubscribe<String>().Channels(new string[] { "Vivaldi RGB" }).Execute();
        }

        public void ChangeColor(Color color)
        {
            PulseLedSetting setting = new PulseLedSetting(color, 100, 0, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(3));
            motherboardLEDs.SetAll(setting);
            log.WriteEntry("Changed Color: to " + HexConverter(color), EventLogEntryType.Information, 20);
        }
        private static String HexConverter(Color color)
        {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

    }
};