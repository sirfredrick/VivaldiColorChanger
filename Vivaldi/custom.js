// Copyright (C) 2018 Jeffrey Tucker
//
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with this program.  If not, see <http://www.gnu.org/licenses/>.
var firstTime = true;
var colorOld = '';
var pubnub = new PubNub({
    subscribeKey: "<subKey>", // always required
    publishKey: "<pubKey>" // only required if publishing
});
setInterval(function wait() {
    var adr = document.querySelector(".toolbar-addressbar.toolbar");
    if (adr != null) {
        var color = getComputedStyle(adr).getPropertyValue("--colorAccentBg");
        if (firstTime) {
            colorOld = color;
            firstTime = false;
            console.log("Subscribing..");
            pubnub.subscribe({
                channels: ['Vivaldi RGB']
            });
        }
        if (color != colorOld) {
            console.log('Color = ' + color);
            pubnub.publish(
                {
                    message: {
                        message: color + '',
                    },
                    channel: 'Vivaldi RGB'
                },
                function (status, response) {
                    if (status.error) {
                        console.log(status)
                    } else {
                        console.log("message Published w/ timetoken", response.timetoken)
                    }
                }
            );
            colorOld = color
        }
    }
}, 3000);