

![](https://github.com/KarmaScripter/Booger/blob/main/Resources/Images/Booger.png)

- A quick & dirty C# application that interacts with OpenAI GPT-3.5 Turbo API

## Setup
Create an Open AI account to obtain an API key (free):
https://platform.openai.com/account/api-keys

You can use the key as a command line parameter (without compiling the project):
1. Click Booger.exe under Releases on the right side of this page
2. Download Booger.zip
3. Unzip the file and run Booger.exe /the key obtained above (Booger.exe /sk-Ih...WPd)

Or in App.xaml.cs, modify this:
"<Your Open AI API Key is something like \"sk-Ih...WPd\">";

Build Booger.sln with Visual Studio 2022 (Community version okay).  This app is targeted for .NET 6 and 8. 
If you don't have .NET 8 installed, remove net8.0-windows in Booger.csproj.


## Code

- [Static](https://github.com/KarmaScripter/Booger/tree/main/Static) - other tools available.
- [Enuerations](https://github.com/KarmaScripter/Booger/tree/main/Enumerations)  - enumerations used by Booger.
- [Services](https://github.com/KarmaScripter/Booger/tree/main/Services) - chat gpt services.
- [Extensions](https://github.com/KarmaScripter/Booger/tree/main/Extensions) - extension methods.
- [Models](https://github.com/KarmaScripter/Booger/tree/main/Models) - classes for prompting and training.
- [Windows](https://github.com/KarmaScripter/Booger/tree/main/Windows) - controls for the user interface and related functionality.
- [Resources](https://github.com/KarmaScripter/Booger/tree/main/Resources)- images, documents, files, etc


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
