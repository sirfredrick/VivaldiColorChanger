﻿// Copyright (C) 2018 Tyler Szabo
//
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Drawing;

namespace GLedApiDotNet.LedSettings
{
    public class PulseLedSetting : LedSetting
    {
        public PulseLedSetting(Color color, byte maxBrightness, byte minBrightness, ushort fadeOnTime, ushort fadeOffTime)
        {
            this.Mode = Modes.Pulse;
            this.Color = color;

            this.SetBrightness(maxBrightness, minBrightness);

            this.Time0 = fadeOnTime;
            this.Time1 = fadeOffTime;
        }

        public PulseLedSetting(Color color, byte maxBrightness, byte minBrightness, TimeSpan fadeOnTime, TimeSpan fadeOffTime)
            : this(color, maxBrightness, minBrightness, 0, 0)
        {
            this.TimeSpan0 = fadeOnTime;
            this.TimeSpan1 = fadeOffTime;
        }

        public TimeSpan FadeOnTime => TimeSpan0;
        public TimeSpan FadeOffTime => TimeSpan1;

        public override string ToString()
        {
            return string.Format("Pulse: Color={0}, MaxBrightness={1}, MinBrightness={2}, FadeOnTime={3}s, FadeOffTime={4}s",
                Color, MaxBrightness, MinBrightness, FadeOnTime.TotalSeconds, FadeOffTime.TotalSeconds);
        }
    }
}
