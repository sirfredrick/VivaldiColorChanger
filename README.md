# Vivaldi Color Changer Service
>  Vivaldi Browser Mod and CMD Installer included!

[![Build status](https://ci.appveyor.com/api/projects/status/bylj0shkxjhhed2m/branch/master?svg=true)](https://ci.appveyor.com/project/sirfredrick231/vivaldicolorchanger/branch/master) [![Codacy Badge](https://api.codacy.com/project/badge/Grade/5c9894303e1e4461b0e945b7d35fd9f8)](https://www.codacy.com/app/sirfredrick231/VivaldiColorChanger?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=sirfredrick231/VivaldiColorChanger&amp;utm_campaign=Badge_Grade)[![GitHub release](https://img.shields.io/github/release/sirfredrick231/VivaldiColorChanger.svg) ![.NET Version](https://img.shields.io/badge/.NET%20Framework-4.6.1-lightgrey.svg)![GitHub](https://img.shields.io/github/license/sirfredrick231/VivaldiColorChanger.svg)](https://github.com/sirfredrick231/VivaldiColorChanger)

Vivaldi Color Changer Service aims to bring the functionality of the changing accent tab color to your RGB Motherboard LEDs. This project has two components a [Vivaldi Browser](https://vivaldi.com/) Mod that grabs the accent color and posts it to [Pubnub](https://www.pubnub.com/), and a Windows Service that talks to the motherboard. The mod is written in JavaScript while the Service, and Installer are written in C# targeting .NET 4.6.1. 
![Vivaldi Color Changer Service](https://i.imgur.com/EzRxdzV.png)
## Features
So what can this project do?
* This project updates your motherboard LEDs or Windows 10 Theme Color to match your Vivaldi Browser's accent color.
* Since this project has a separate Vivaldi Mod and Windows Service you can either just use the Vivaldi Mod to change other LEDs or you can change your motherboard LEDs by having another program talk with the Windows Service. The options are almost limitless!
* This project now has a EXE Installer!

#### Note:

This project currently only has RGB working with [Gigabyte RGB Fusion Motherboards](https://www.gigabyte.com/mb/rgb/) on Windows machines. Theme changing works for all Windows 10 devices.

## Getting started

Make a Pubnub account by going to the [www.pubnub.com](https://www.pubnub.com/) and clicking `Get Started` and then go to the `Sign Up` tab. 

Once you are logged in create a new app and then copy the `Publish and Subscribe Keys` for later.

Download the project binary Setup.exe installer:
[Binary Installer](https://github.com/sirfredrick231/VivaldiColorChanger/releases)
This project uses RGB Fusion SDK. The supported binaries can be found here
* Latest Version- [~~B18.0206.1.zip~~](https://www.gigabyte.com/WebPage/332/images/B18.0206.1.zip) download link is broken. If you still have the DLLs from the previous version move them into the Vivaldi Color Changer Service folder after installing and the RGB will still work.

**Note** If you are using this project Pre-Version 2.0.0 use:
* [B17.0926.1.zip](https://www.gigabyte.com/WebPage/332/images/B17.0926.1.zip)

## Installing
Just run `Setup.exe` and input you `Vivaldi Browser` path (Make sure to install the [Vivaldi Browser](https://vivaldi.com/download/) if you haven't already.) and  your `Publish and Subscribe Keys`
Copy and Paste your PubNub Subscribe Key Below.
Then you need to enter the .theme file path. If you don't want Theme Changing just leave it blank and click next. 
To turn your current theme into a .theme file got to `Settings -> Personalization -> Themes`. Then give your file a name (Do not include .theme). It can be found under `C://Users/<Username>/AppData/Local/Microsoft/Windows/Themes/<ThemeName>.theme`
Make sure to enable `Hidden Files` in the File Explorer to see the `AppData` folder.
and then the installer will install and start the Windows Service. 

You will now be prompted for a login. Enter your Computer's Name (How it appears under your network) and Username in this format:
```shell
DOMAIN\username
```
then enter the password that you would normally log into your computer with.

Once you are done with that the installation should now be done! Wasn't that easier than you thought?

When you start Vivaldi go to a new website and see the color on your motherboard (or Windows 10 Theme) magically change! (Twitter and Youtube work well.)

## Uninstalling

To uninstall the `ColorChangerService` run the `unins000.exe` and follow the instruction.
The `Vivaldi Mod` will uninstall when you update `Vivaldi`.
Follow the uninstall instructions and you should be back to a boring old setup!

## Developing

If you want to help develop this project, welcome aboard!
To get started, clone the repository and open the `solution` in [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/). Right now the project is targeting `.NET 4.6.1`

```shell
git clone https://github.com/sirfredrick231/VivaldiColorChanger.git
```

### Building

To run the Installer manually, Build the solution and look in the `ColorChangerService Project` in the release folder.

Then use [InnoSetup](http://www.jrsoftware.org/isinfo.php) to compile the `Setup.exe` make sure to change the folder path's in the `Setup.iss` to where the release folder is.

To debug just the service, we have [Topshelf](http://topshelf-project.com/) this allows us to run the Windows Service as a Console app in the debugger! 
Make sure that you copy the `GLedApi.dll`, `ycc.dll`, and `layout.ini` to the `\GLedAPIDotNet\` folder when not using `Setup.exe`.

Finally, the `ColorChangerService.exe` only works in `x86`, not `x86_64`.

To manually install the Vivaldi Mod, add the `custom.js` and `pubnub.js` files to the `Vivaldi\Application\<Version #>\resources\vivaldi` and add 
```shell
    <script src="pubnub.js"></script>
    <script src="custom.js"></script>
```
to the body of `browser.html`

Then you can turn on the `Debugging for packed apps` flag at `vivaldi://flags` on your browser. Then right click the active tab and `Inspect` then you can see the Mod's console output.

### Deploying / Publishing
Before publish please use default Visual Studio 2019 formatting by clicking ctr+k an then ctr+d.

## Planned Features/Please help me do this!

* Support for more motherboard and peripheral brands RGB Software. Such as, [Aura Sync](https://www.asus.com/campaign/aura/us/index.html), [Mystic Light](https://www.msi.com/Landing/mystic-light-motherboard#extension) etc.
* ~~Update [pubnub.js](https://github.com/pubnub/javascript/releases) and [GLedAPIDotNet](https://github.com/tylerszabo/RGB-Fusion-Tool) to their latest versions.~~
* ~~Maybe have an MSI installer?~~
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
   - Thanks to [Han-soft](http://www.han-soft.com/) for their excellent dwinshs.iss program. It made it simple to download proprietary files

## Copyright
GLedAPIDotNET from RGB-Fusion-Tool Copyright © 2018 Tyler Szabo

Installer, ColorChangerService, and custom.js, MoveDLLs, MoveVivaldiFiles, Setup.iss Copyright © 2018, 2019 Jeffrey Tucker

pubnub.js Copyright © 2013 PubNub Inc.

Inno Setup Copyright © 1997-2019 Jordan Russell

DwinHs Copyright © 2001, 2015 Han-soft Corporation

unzip.exe Copyright 1990-2009 © Info-ZIP

## Licensing
This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

The code in this project is licensed under GNU General Public License Version 3. It can be found [here](https://www.gnu.org/licenses/gpl-3.0.en.html).