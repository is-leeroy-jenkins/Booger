<div align="left">
  <p>
    <a href="https://github.com/is-leeroy-jenkins/Bubba/tree/master/Resources/Github/Users.md">Download</a> •  <a href="">Documentation</a> •<a href="https://github.com/is-leeroy-jenkins/Bubba/tree/master/Resources/Github/Compilation.md">Build</a> • <a href="#-license">License</a>
  </p>
  <p>
    
  </p>
</div>

﻿![](https://github.com/is-leeroy-jenkins/Booger/blob/main/Resources/Assets/Github/Repo.png)


## ![](https://github.com/is-leeroy-jenkins/Bubba/blob/master/Resources/Assets/GitHubImages/features.png) Features

- A small Windows Presentation Foundation (WPF) application that interacts with OpenAI GPT-3.5 Turbo API
- Based on .NET6 and written in C#.

## ![](https://github.com/is-leeroy-jenkins/Booger/blob/main/Resources/Assets/Github/install.png) Setup
Create an Open AI account to obtain an API key (free):
https://platform.openai.com/account/api-keys

You can use the key as a command line parameter (without compiling the project):
1. Click Booger.exe under Releases on the right side of this page
2. Download Booger.zip
3. Unzip the file and run Booger.exe and enter the key obtained above (Booger.exe /sk-Ih...WPd) in the settings tab



Build Booger.sln with Visual Studio 2022 (Community version okay).  This app is targeted for .NET 6 and 8. 
If you don't have .NET 8 installed, remove net8.0-windows in Booger.csproj.


## ![](https://github.com/is-leeroy-jenkins/Booger/blob/main/Resources/Assets/Github/csharp.png)Code

- [Static](https://github.com/is-leeroy-jenkins/Booger/tree/main/Static) - other tools available.
- [Converters](https://github.com/is-leeroy-jenkins/Booger/tree/main/Converters) - various conversions.
- [Behaviors](https://github.com/is-leeroy-jenkins/Booger/tree/main/Behaviors) - behaviors for the styles.
- [Enumerations](https://github.com/is-leeroy-jenkins/Booger/tree/main/Enumerations)  - enumerations used by Booger.
- [Services](https://github.com/is-leeroy-jenkins/Booger/tree/main/Services) - chat gpt services.
- [Extensions](https://github.com/is-leeroy-jenkins/Booger/tree/main/Extensions) - extension methods.
- [Models](https://github.com/is-leeroy-jenkins/Booger/tree/main/Models) - classes for prompting and training.
- [UI](https://github.com/is-leeroy-jenkins/Booger/tree/main/UI) - classes for the user interface and related functionality.
- [ColorModes](https://github.com/is-leeroy-jenkins/Booger/tree/main/UI/ColorModes) - light and dark modes.
- [Styles](https://github.com/is-leeroy-jenkins/Booger/tree/main/Resources/Styles) - other look and feel stuff.
- [Resources](https://github.com/is-leeroy-jenkins/Booger/tree/main/Resources)- images, documents, files, etc

## ![](https://github.com/is-leeroy-jenkins/Booger/blob/main/Resources/Assets/GitHubImages/openai.png)  Machine Learning 

- [Federal Appropriations](https://github.com/is-leeroy-jenkins/Booger/blob/main/Resources/Github/Appropriations.md) - federal funding data available for fine-tuning learning models
- [Federal Regulations](https://github.com/is-leeroy-jenkins/Booger/blob/main/Resources/Github/Regulations.md) - regulatory data available for fine-tuning learning models



## ![](https://github.com/is-leeroy-jenkins/Booger/blob/main/Resources/Assets/Github/documentation.png)  Documentation

- [User Guide](https://github.com/is-leeroy-jenkins/Booger/tree/master/Resources/Github/Users.md) - how to use Booger.
- [Compilation Guide](https://github.com/is-leeroy-jenkins/Booger/tree/master/Resources/Github/Compilation.md) - instructions on how to compile Booger.
- [Configuration Guide](https://github.com/is-leeroy-jenkins/Booger/tree/master/Resources/Github/Configuration.md) - information for the Booger configuration file. 
- [Distribution Guide](https://github.com/is-leeroy-jenkins/Booger/tree/master/Resources/Github/Distribution.md) -  distributing Booger.


## 📦 Download

Pre-built and binaries (setup, portable and archive) are available on the with install instructions (e.g. silent install). 




## ![](https://github.com/is-leeroy-jenkins/Booger/blob/main/Resources/Assets/Github/tools.png) Build

- [x] VisualStudio 2022
- [x] Based on .NET8 and WPF


```bash
$ git clone https://github.com/is-leeroy-jenkins/Booger.git
$ cd Booger
```
Run `Booger.sln`


You can build the application like any other .NET / WPF application on Windows.

1. Make sure that the following requirements are installed:

   - [.NET 8.x - SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
   - Visual Studio 2022 with `.NET desktop development` and `Universal Windows Platform development`

2. Clone the repository with all submodules:

   ```PowerShell
   # Clone the repository
   git clone https://github.com/is-leeroy-jenkins/Booger

   # Navigate to the repository
   cd Booger

   # Clone the submodules
   git submodule update --init
   ```

3. Open the project file `.\Source\Booger.sln` with Visual Studio or JetBrains Rider to build (or debug)
   the solution.

   > **ALTERNATIVE**
   >
   > With the following commands you can directly build the binaries from the command line:
   >
   > ```PowerShell
   > dotnet restore .\Source\Booger.sln
   >
   > dotnet build .\Source\Booger.sln --configuration Release --no-restore
   > ```



## 🙏 Acknowledgements

Booger uses the following projects and libraries. Please consider supporting them as well (e.g., by starring their repositories):

|                                 Library                                       |             Description                                                |
| ----------------------------------------------------------------------------- | ---------------------------------------------------------------------- |
| [CefSharp.WPF.Core](https://github.com/cefsharp)                              | .NET (WPF/Windows Forms) bindings for the Chromium Embedded Framework  |
| [Epplus](https://github.com/EPPlusSoftware/EPPlus)                  		    | EPPlus-Excel spreadsheets for .NET      								 |
| [Google.Api.CustomSearchAPI.v1](https://developers.google.com/custom-search)  | Google APIs Client Library for working with Customsearch v1            |
| [LinqStatistics](https://www.nuget.org/packages/LinqStatistics)                     | Linq extensions to calculate basic statistics.                                                 |
| [System.Data.SqlServerCe](https://www.nuget.org/packages/System.Data.SqlServerCe_unofficial)                  | Unofficial package of System.Data.SqlServerCe.dll if you need it as dependency.      |
| [Microsoft.Office.Interop.Outlook 15.0.4797.1004](https://www.nuget.org/packages/Microsoft.Office.Interop.Outlook)                        | This an assembly you can use for Outlook 2013/2016/2019 COM interop, generated and signed by Microsoft. This is entirely unsupported and there is no license since it is a repackaging of Office assemblies.                                       |
| [ModernWpfUI 0.9.6](https://www.nuget.org/packages/ModernWpfUI/0.9.7-preview.2)                     | Modern styles and controls for your WPF applications.         |
| [Newtonsoft.Json 13.0.3](https://www.nuget.org/packages/Newtonsoft.Json)                                          | Json.NET is a popular high-performance JSON framework for .NET                  |
| [RestoreWindowPlace 2.1.0](https://www.nuget.org/packages/RestoreWindowPlace)                                             | Save and restore the place of the WPF window                                           |
| [SkiaSharp 2.88.9](https://github.com/mono/SkiaSharp)   | SkiaSharp is a cross-platform 2D graphics API for .NET platforms based on Google's Skia Graphics Library.                           |
| [Syncfusion.Licensing 24.1.41](https://www.nuget.org/packages/Syncfusion.Licensing)                           | Syncfusion licensing is a .NET library for validating the registered Syncfusion license in an application at runtime.         |
| [System.Data.OleDb 9.0.0](https://www.nuget.org/packages/System.Data.OleDb)  | This package implements a data provider for OLE DB data sources.                            |
| [System.Data.SqlClient 4.9.0](https://www.nuget.org/packages/System.Data.SqlClient) | The legacy .NET Data Provider for SQL Server.                       |
| [MahApps.Metro](https://mahapps.com/)                                         | UI toolkit for WPF applications                                        |
| [System.Data.SQLite.Core](https://www.nuget.org/packages/System.Data.SQLite.Core)                        | The official SQLite database engine for both x86 and x64 along with the ADO.NET provider. |
| [System.Speech 9.0.0](https://www.nuget.org/packages/System.Speech)          | Provides APIs for speech recognition and synthesis built on the Microsoft Speech API in Windows.                               |
| [ToastNotifications.Messages.Net6 1.0.4](https://www.nuget.org/packages/ToastNotifications.Messages.Net6)          | Toast notifications for WPF allows you to create and display rich notifications in WPF applications.                              |
| [ToastNotifications.Net6 1.0.4]([https://github.com/lahell/PSDiscoveryProtocol](https://github.com/haitongxuan/ToastNotifications))          | Toast notifications for WPF allows you to create and display rich notifications in WPF applications.                              |
| [Syncfusion.SfSkinManager.WPF 24.1.41](https://www.nuget.org/packages/Syncfusion.SfSkinManager.WPF)          | The Syncfusion WPF Skin Manageris a .NET UI library that contains the SfSkinManager class, which helps apply the built-in themes to the Syncfusion UI controls for WPF.                               |
| [Syncfusion.Shared.Base 24.1.41](https://www.nuget.org/packages/Syncfusion.Shared.Base)          | Syncfusion WinForms Shared Components                               |
| [Syncfusion.Shared.WPF 24.1.41](https://www.nuget.org/packages/Syncfusion.Shared.WPF)          | Syncfusion WPF components                               |
| [Syncfusion.Themes.FluentDark.WPF 24.1.41](https://www.nuget.org/packages/Syncfusion.Themes.FluentDark.WPF)          | The Syncfusion WPF Fluent Dark Theme for WPF contains the style resources to change the look and feel of a WPF application to be similar to the modern Windows UI style introduced in Windows 8 apps.                               |
| [Syncfusion.Tools.WPF 24.1.41](https://www.nuget.org/packages/Syncfusion.Tools.WPF)          | This package contains WPF AutoComplete, WPF DockingManager, WPF Navigation Pane, WPF Hierarchy Navigator, WPF Range Slider, WPF Ribbon, WPF TabControl, WPF Wizard, and WPF Badge components for WPF application.                               |
| [Syncfusion.UI.WPF.NET 24.1.41](https://www.nuget.org/packages/Syncfusion.UI.WPF.NET)          | Syncfusion WPF Controls is a library of 100+ WPF UI components and file formats to build modern WPF applications.                              |


