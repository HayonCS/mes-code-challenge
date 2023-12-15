# Blazor WebAssembly Barcode Generator

## Overview:
We have times in manufacturing where we need to display barcodes for inventory tracking. Your goal is to create a Blazor WebAssembly application that can generate on-screen barcodes.

## Requirements 
- Your Blazor application must contain:
    - A text field for a user to input a value.
    - There is a barcode image on-screen that will update to reflect the value that the user has entered into the text field. 
- Remove/cleanup unnecessary web pages.
- You may use any standard encoding types for the barcode.
- You may use online resources, including shared libraries.

# Tools Needed
Use a text editor like [VS Code](https://code.visualstudio.com/).

Verify dotnet is installed, or install from [here](https://dotnet.microsoft.com/en-us/download):
```bash
dotnet --info
```
# Getting Started
Scaffold out a new dotnet Blazor WebAssembly project from a console or VS Code terminal:
```bash
dotnet new blazorwasm
```

Run the webapp from the console/terminal:
```bash
dotnet run
```

Navigate to the app at http://localhost:[port]/swagger. The port number is generated for the project and can be found in the console output when run or in [launchSettings.json](/Properties/launchSettings.json).
```bash
    Now listening on: http://localhost:#####
```

Use the locally running web app to test it's behavior.

