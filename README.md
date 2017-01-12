VSEmbed
=======

* Forked from [SLaks/VSEmbed](https://github.com/SLaks/VSEmbed)

How to extend
=============

`VSEmbed.DemoApp` is the standalone VS editor. It pulls most DLLs it needs from NuGet and MyGet. Some DLLs come from the `lib\` directory. 

Any project that uses the VS editor, will need the same set of DLLs in its own output directory.

* To bring the NuGet dependencies.
  1. Copy contents of [`VSEmbed.DemoApp\packages.config`](https://github.com/AmadeusW/VSEmbed/blob/master/VSEmbed.DemoApp/packages.config) to your project's `packages.config`
  2. Run `Update-Package -Reinstall -ProjectName [name of your target project]` in Package Manager Console [source](http://stackoverflow.com/questions/11026137/can-i-copy-the-nuget-package-configuration-from-one-project-to-another) 
* To bring hard dependencies
  1. Copy all References with `<HintPath>..\lib\` from [`VSEmbed.DemoApp\VSEmbed.DemoApp.csproj`](https://github.com/AmadeusW/VSEmbed/blob/master/VSEmbed.DemoApp/VSEmbed.DemoApp.csproj) to your project's `.csproj`
 