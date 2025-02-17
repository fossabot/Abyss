[![DBL Big](https://discordbots.org/api/widget/532099058941034498.svg)](https://discordbots.org/bot/532099058941034498) [![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fabyssal512%2FAbyss.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2Fabyssal512%2FAbyss?ref=badge_shield)
 
[![DBL Small](https://discordbots.org/api/widget/owner/532099058941034498.svg)](https://discordbots.org/bot/532099058941034498) [![Discord](https://img.shields.io/discord/598437365891203072?style=flat-square)](https://discord.gg/RsRps9M)
# Abyss Bot Platform

A **fully modular, expandable, open-source (for life)** Discord bot and platform, written in C# using .NET Core and Discord.Net.  
You can add the public instance, running on the default addon set, [here.](https://discordapp.com/api/oauth2/authorize?client_id=532099058941034498&permissions=0&scope=bot)  
You can join the support server [here.](https://discord.gg/RsRps9M)
  
### Features
- Complete modularity including custom event hooks, custom commands (using the existing command system), and full expandability, through .NET's powerful assembly loading system   
- Spotify track and album lookup (can also read from the current song you're listening to), powered by [AbyssalSpotify](http://github.com/abyssal512/AbyssalSpotify)  
- Resizing (bicubic) of emojis and custom images, both animated and not-animated  
- Live C# script evaluation  
- Dice rolling with custom expression support (e.g. `a.roll d20+d48+d10`)  
- C a t commands  
- General purpose command set

  
### Requirements
- It is heavily recommended to run Abyss on a Docker daemon running with Linux containers. Instructions for running with Docker are [here.](DOCKER.md) If you don't want to run on Docker, you're on your own.
- .NET Core 3.0 Preview 7 SDK for building (or Runtime for a pre-compiled version)
- A Discord bot application with registered user and token
- `Abyss.json` configuration file set out as below

### Configuration
An example Abyss configuration file can be found at [abyss.example.json](https://github.com/abyssal512/Abyss/blob/master/abyss.example.json), which should be renamed to `abyss.json` before running.  
It produces a result that looks like this:   
![Abyss: Watching you](https://jessica.is-pretty.cool/AQx2195.png)  
The bot will rotate through each Activity provided under the Startup.Activity property every minute. Available Activity types are Playing, Streaming, Listening, and Watching. (These match to the [enum ActivityType](https://docs.stillu.cc/api/Discord.ActivityType.html) in Discord.Net)  
  
### Modularity
Abyss has fully modular runtime assembly support. Here's a quick guide on doing so:
1) Clone this repository to your computer.
2) Create a new .NET Core 3.0 Library (**not .NET Standard 2.0**) and add your local copy of `Abyss.Core` as a dependency.
3) Create your modules and commands as you like, using `AbyssModuleBase`. Feel free to look at Abyss' included commands for help. To extend your addon to modify functionality of the platform, create a class that extends `Absys.Core.Addons.IAddon` and implement it's methods.
4) Build `Abyss.Console` (or whatever frontend you are using) in your preferred configuration.
5) Build your assembly, and copy the assembly file (something like `MyCommandAssembly.dll`) into your `Addons` folder. If you don't provide an absolute or relative directory path as the first argument to the application, it will default to the directory of the built DLL, plus `Addons`. If you do, it will use the `Addons` directory in that path.
6) Abyss will automatically discover addons and modules and register them. This will be logged in the console.
7) If there are any issues, join the [support server for help.](https://discord.gg/RsRps9M)


### Contributing
The project is broken down into the following projects:   
**Platform core**  
- `Abyss.Core` (library) The core of Abyss. This project contains the robust, fast, and safe architecture that sits at the heart of Abyss operation. It also contains all of the default commands that come with every instance.  

**Console host**  
- `Abyss.Console` This is an executable which wraps `Abyss.Core`, and pipes output to the console. This executable does not support web functionality.  

**Web host**  
- `Abyss.Web.Shared` This library contains data classes and code that is shared between `Abyss.Web.Client` and `Abyss.Web.Server`.
- `Abyss.Web.Client` This contains all the Blazor clientside code for using the Abyss administration panel.
- `Abyss.Web.Server` An alternative to `Abyss.Console`, this executable starts up the Abyss administration panel on `[Any IP]:2003` and serves clients the Blazor code in `Abyss.Web.Client`. This web server contains many useful administration tools.   

**Addons**  
- `Abyss.ExampleCustomAssembly` (library) This is an example project, which shows how you can make your own projects to expand and add new functionality and commands to Abyss.

### Copyright
Copyright (c) 2019 abyssal512 under the MIT License, available at [the LICENSE file.](LICENSE.md)


## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2Fabyssal512%2FAbyss.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2Fabyssal512%2FAbyss?ref=badge_large)