

﻿![](https://github.com/is-leeroy-jenkins/Booger/blob/main/Resources/Assets/Github/Repo.png)

- A quick & dirty WPF application that interacts with OpenAI GPT-3.5 Turbo API
- Based on .NET6 and written in C#.

## Setup
Create an Open AI account to obtain an API key (free):
https://platform.openai.com/account/api-keys

You can use the key as a command line parameter (without compiling the project):
1. Click Booger.exe under Releases on the right side of this page
2. Download Booger.zip
3. Unzip the file and run Booger.exe and enter the key obtained above (Booger.exe /sk-Ih...WPd) in the settings tab



Build Booger.sln with Visual Studio 2022 (Community version okay).  This app is targeted for .NET 6 and 8. 
If you don't have .NET 8 installed, remove net8.0-windows in Booger.csproj.


## Code

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


## Supporting Libraries

Whetstone.ChatGPT
Does all the heavy lifting for ChatGPT API calls.

https://www.nuget.org/packages/Whetstone.ChatGPT

CommunityToolkit.Mvvm
 
https://www.nuget.org/packages/CommunityToolkit.Mvvm
 
ModernWpfUI
 
https://www.nuget.org/packages/ModernWpfUI/
 
RestoreWindowPlace

https://www.nuget.org/packages/RestoreWindowPlace
