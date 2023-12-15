# Important

The JSON files in the project ('bunkbed-bom.json' and 'bunkbed-routing.json') are copied to the build folder on compilation.

If you do not want this to happen every time building the project, you can remove the setting found in the '.csproj' file.

![image](https://github.com/HayonCS/mes-code-challenge/assets/90340842/691fa7b5-148e-4c66-b852-6a6c7ee03de0)

# Bunk Bed Build - Takt Time Calculator

Although Gentex doesn't manufacture furniture, configuration for materials needed to build things and instructions for how to build them are used the same way. Like products assembled at Gentex, a bunk bed has many small components that are assembled together, then those are further assembled together forming a heirarchy from hardware and wood to a finished bed called a bill of material or BOM.

Also like Gentex products, bunk beds are put together following an ordered set of instructions. In manufacturing this is called a routing. Measuring time to build something and comparing it to expected assembly time, called takt time, also helps to ensure efficiency and quality.

For this excercise, engineers would like a .NET console application that can output a flat list for the sum of all 'Provided' components and calculate an overall takt time given a bill of material and a routing.

# Requirements

Use [bunkbed-bom.json](./bunkbed-bom.json) and [bunkbed-routing.json](./;bunkbed-routing.json) to build a console app and complete the following:

1. Put all of the provided components and their sum quantity in a flat list and write it to a text file.

   output.csv

   ```
   component,quantity
   {Description1},{Quantity1}
   {Description2},{Quantity2}
   ...
   ```

1. Write to console if there are any steps where there are no provided components being added.
   ```
   Step {Step} '{Step Description}' has no provided components added.
   ```
1. Write to console the overall takt time (values expressed in minutes).
   ```
   Overall takt time: {time} minutes.
   ```

# Tools Needed

Use a text editor like [VS Code](https://code.visualstudio.com/).

Verify dotnet is installed, or install from [here](https://dotnet.microsoft.com/en-us/download):

```bash
dotnet --info
```

# Getting Started

Scaffold out a new dotnet webapi from a console or VS Code terminal:

```bash
dotnet new console
```

Run the api from the console/terminal:

```bash
dotnet run
```
