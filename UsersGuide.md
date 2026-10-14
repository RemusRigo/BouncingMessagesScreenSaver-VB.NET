# Bouncing Messages ScreenSaver: User's Guide



## Downloads & Runtimes

Download the latest release from the [Releases page](https://github.com/RemusRigo/BouncingMessagesScreenSaver-VB.NET/releases).

Each release contains one folder per framework and architecture, plus the `Messages` folder:

|Folder|Target Framework|Architecture|Required Runtime|
|-|-|-|-|
|`net8.0 x64`|.NET 8.0|64-bit|[.NET 8.0 Desktop Runtime (x64)](https://dotnet.microsoft.com/download/dotnet/8.0)|
|`net8.0 x86`|.NET 8.0|32-bit|[.NET 8.0 Desktop Runtime (x86)](https://dotnet.microsoft.com/download/dotnet/8.0)|
|`net10.0 x64`|.NET 10.0|64-bit|[.NET 10.0 Desktop Runtime (x64)](https://dotnet.microsoft.com/download/dotnet/10.0)|
|`net10.0 x86`|.NET 10.0|32-bit|[.NET 10.0 Desktop Runtime (x86)](https://dotnet.microsoft.com/download/dotnet/10.0)|

> **Which one do I need?** On 64-bit Windows 10/11, use **x64**. Pick the .NET version whose
> **Desktop Runtime** you already have installed, or install one from the links above.
> The plain ".NET Runtime" is not enough; the screensaver needs the **Desktop** Runtime.

To check which runtimes are installed, open a command prompt and run:

```
dotnet --list-runtimes
```

Look for a `Microsoft.WindowsDesktop.App 8.x` or `10.x` line.



## Install

1. Download and extract the folder that matches your system (see the table above), e.g. `net8.0 x64`.
2. Copy the **whole folder** to a permanent location, for example:
   `D:\My Apps\Bouncing Messages ScreenSaver\`
   The folder must contain all 4 files; the screensaver won't start if any of them is missing:
   * `BouncingMessagesSS.scr`
   * `BouncingMessagesSS.dll`
   * `BouncingMessagesSS.deps.json`
   * `BouncingMessagesSS.runtimeconfig.json`
3. Copy the `Messages` folder from the release **into** that folder (next to `BouncingMessagesSS.scr`).
4. Right-click `BouncingMessagesSS.scr` and choose **Install**.
   The Windows **Screen Saver Settings** dialog opens with *BouncingMessagesSS* selected.
5. Set the **Wait** time and click **OK**.

> If Windows blocks the file ("Windows protected your PC"), right-click `BouncingMessagesSS.scr` →
> **Properties** → check **Unblock** → **OK**, then try again.



## Test it

* In **Screen Saver Settings**, click **Preview**, or
* double-click `BouncingMessagesSS.scr`.

A message starts on the primary monitor and bounces off the edges of your monitors.
It changes color at every bounce, and the next message is shown every 4 bounces.

|Key|Description|
|-|-|
|**Space**|Pause / resume the movement|
|any other key, click, or mouse move|Exit|



## Settings

Open **Screen Saver Settings** (Start → type *screen saver*), select *BouncingMessagesSS*, and click **Settings...**

|Setting|Description|
|-|-|
|**Messages folder**|The folder with the message files; type a path and press Enter, or click **Browse**. Default: the `Messages` folder next to the screensaver|
|**Random Messages**|Checked (default): every time the screensaver starts, it picks a random `.json` file from the folder and its subfolders|
|**Message file list**|Uncheck **Random Messages** and select the file to always use|

Click **Ok** to save.
Settings are stored per user in the registry under `HKEY_CURRENT_USER\Software\Remus RIGO\BouncingMessagesSS.VB-NET`.
Right-click the title bar → **About...** for version information.



## Message files

A message file is a JSON file with one `"Title": "Description"` pair per message:

```json
{
   "Internet Browser Keyboard Shortcuts": "© 2026 Remus Rigo",
   "Alt+Left arrow": "Go back",
   "Alt+Right arrow": "Go forward",
   "Ctrl-R": "Reload page"
}
```

Messages are shown in the order they appear in the file.
To add your own, save a new `.json` file anywhere in the messages folder (subfolders are fine).

Included in `Messages\KB Shortcuts`: `Browser.json`, `Microsoft Excel.json`, `Microsoft Windows.json`, `Microsoft Word.json`.



## Multi-monitor

The message moves across all monitors and bounces off their outer edges, including the steps between
monitors of different sizes.



## Uninstall

1. In **Screen Saver Settings**, choose another screensaver (or *None*) and click **OK**.
2. Delete the folder you copied in step 2 of *Install*.
3. Optional: delete the registry key `HKEY_CURRENT_USER\Software\Remus RIGO\BouncingMessagesSS.VB-NET`.
