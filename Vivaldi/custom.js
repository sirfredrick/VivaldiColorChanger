var firstTime = true;
var colorOld = '';
var pubnub = new PubNub({
    subscribeKey: '<subKey>', // always required
    publishKey: '<pubKey>' // only required if publishing
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