# Vivaldi Color Changer Service
>  Vivaldi Browser Mod and CMD Installer included!

[![Build status](https://ci.appveyor.com/api/projects/status/bylj0shkxjhhed2m/branch/master?svg=true)](https://ci.appveyor.com/project/sirfredrick231/vivaldicolorchanger/branch/master) [![Codacy Badge](https://api.codacy.com/project/badge/Grade/5c9894303e1e4461b0e945b7d35fd9f8)](https://www.codacy.com/app/sirfredrick231/VivaldiColorChanger?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=sirfredrick231/VivaldiColorChanger&amp;utm_campaign=Badge_Grade)

Vivaldi Color Changer Service aims to bring the functionality of the changing accent tab color to your RGB Motherboard LEDs. This project has two components a [Vivaldi Browser](https://vivaldi.com/) Mod that grabs the accent color and posts it to [Pubnub](https://www.pubnub.com/), and a Windows Service that talks to the motherboard. The mod is written in JavaScript while the Service, and Installer are written in C# targeting .NET 4.6.1. 

## Features
So what can this project do?
* This project updates your motherboard LEDs to match your Vivaldi Browser's accent color.
* Since this project has a separate Vivaldi Mod and Windows Service you can either just use the Vivaldi Mod to change other LEDs or you can change your motherboard LEDs by having another program talk with the Windows Service. The options are almost limitless!

#### Note:

This project currently only works with [Gigabyte RGB Fusion Motherboards](https://www.gigabyte.com/mb/rgb/) on Windows machines.

## Getting started

Make a Pubnub account by going to the [www.pubnub.com](https://www.pubnub.com/) and clicking `Get Started` and then go to the `Sign Up` tab. 

Once you are logged in create a new app and then copy the `Publish and Subscribe Keys` for later.

Download the project binaries and the required RGB Fusion API files:
* [Binaries](https://github.com/sirfredrick231/VivaldiColorChanger/releases)
* [B17.0926.1.zip](https://www.gigabyte.com/WebPage/332/images/B17.0926.1.zip)

Move the RGB Fusion files `GLedApi.dll`, `ycc.dll`, and `layout.ini` to the ColorChangerService Folder.

## Installing
Now, run `Installer.exe` in the root folder.

Copy and paste your `Publish and Subscribe Keys`
```shell
Welcome to the Color Changer Service Console Installer.
Please Copy and Paste your PubNub Subsribe Key Below.
<Paste Sub Key Here>
Please Copy and Paste your PubNub Publish Key Below.
<Paste Pub Key Here>
```
and then install and start the Windows Service.

```shell
Commence Installation of the Color Changer Service? (Y/n)
y
Installation Complete:
Would you like to start Color Changer Service now? (Y/n)
y
```

Finally Install the Vivaldi Mod. Make sure to install the [Vivaldi Browser](https://vivaldi.com/download/) if you haven't already.

```shell
Would you like to install the accompanying Vivaldi Mod? (Y/n)
y
Please enter the path to your Vivaldi Folder.
<Path to Vivaldi Program Folder>
```
The path is usually `C:\Program Files\Vivaldi` or `C:\Program Files (x86)\Vivaldi`

Once you are done with that the installation should now be done! Wasn't that easier than you thought?

When you start Vivaldi go to a new website and see the color on your motherboard magically change! (Twitter and Youtube work well.)

## Uninstalling

To uninstall either the Windows Service or Vivaldi Mod, go to the root folder in an Admin CMD and run
```shell
Installer.exe uninstall
```
Follow the uninstall instructions and you should be back to a boring old setup!

## Developing

If you want to help develop this project, welcome aboard!
To get started, clone the repository and open the `solution` in [Visual Studio 2017](https://visualstudio.microsoft.com/downloads/). Right now the project is targeting `.NET 4.6.1`

```shell
git clone https://github.com/your/awesome-project.git
```

### Building

To run the Installer manually, Build the solution and copy the output of the `Installer project` to the root and the output of the `ColorChangerService Project` to `\ColorChangerService\`. Then, copy the `Vivaldi folder` to the root.

You can now run the Installer directly.

To debug just the service, we have [Topshelf](http://topshelf-project.com/) this allows us to run the Windows Service as a Console app in the debugger! 
Make sure that you copy the `GLedApi.dll`, `ycc.dll`, and `layout.ini` to the `\GLedAPIDotNet\` folder.
Also, change `config.ini` to have the right subscribe key. Finally, the `ColorChangerService.exe` only works in `x86`, not `x86_64`.

To manually install the Vivaldi Mod, add the `custom.js` and `pubnub.js` files to the `Vivaldi\Application\<Version #>\resources\vivaldi` and add 
```shell
    <script src="pubnub.js"></script>
    <script src="custom.js"></script>
```
to the body of `browser.html`

Then you can turn on the `Debugging for packed apps` flag at `vivaldi://flags` on your browser. Then right click the active tab and `Inspect` then you can see the Mod's console output.

### Deploying / Publishing
Before publish please use default Visual Studio 2017 formatting by clicking ctr+k an then ctr+d.

## Planned Features/Please help me do this!

* Support for more motherboard and peripheral brands RGB Software. Such as, [Aura Sync](https://www.asus.com/campaign/aura/us/index.html), [Mystic Light](https://www.msi.com/Landing/mystic-light-motherboard#extension) etc.
* Update [pubnub.js](https://github.com/pubnub/javascript/releases) and [GLedAPIDotNet](https://github.com/tylerszabo/RGB-Fusion-Tool) to their latest versions.
* Maybe have an MSI installer?
* Linux Version?

## Contributing

If you'd like to contribute, please fork the repository and use a feature
branch. Pull requests are warmly welcome.

See `CONTRIBUTING.md` for the specifics.

## Links

- Repository: https://github.com/sirfredrick231/VivaldiColorChanger
- Issue tracker: https://github.com/sirfredrick231/VivaldiColorChanger/issues
- Related projects:
   - Thanks to Tyler Szabo for the GLedApiDotNet project from the RGB-Fusion-Tool: https://github.com/tylerszabo/RGB-Fusion-Tool
   - Thanks to Topshelf for making an easy to use Windows Service Library: http://topshelf-project.com/
   - Thanks to [Vivaldi](https://vivaldi.com/) and it's amazing community: https://forum.vivaldi.net/topic/26553/is-there-a-way-to-get-the-accent-color-in-custom-js 

## Copyright
GLedAPIDotNET from RGB-Fusion-Tool Copyright © 2018 Tyler Szabo

Installer, ColorChangerService, and custom.js Copyright © 2018 Jeffrey Tucker

pubnub.js Copyright © 2013 PubNub Inc.

## Licensing
This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

The code in this project is licensed under GNU General Public License Version 3. It can be found [here](https://www.gnu.org/licenses/gpl-3.0.en.html).