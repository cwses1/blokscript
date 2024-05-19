# BlokScript

[BlokScript](https://www.blokscript.com) is a programming language for [Storyblok](https://www.storyblok.com).

## Building

### .NET Framework Build

You will need Visual Studio 2022 for this.

* Go to the `DotNetFramework_Projects` directory.
* Open the `BlokScript.sln` using Visual Studio 22.  Visual Studio Code won't work.
* In Visual Studio, go to 'Build' then select 'Build Solution.'
* Visual Studio will download packages and do the build.
* `blokscript.exe` will be in the `BlokScript.BlokScriptApp\bin\Debug` directory.

### .NET Core Build

You will need the following for this:

* [.NET Core 8.0](https://dotnet.microsoft.com/en-us/download)
* [NAnt 0.92](https://nant.sourceforge.net/).  GitHub repository is [here](https://github.com/nant/nant) if you have issues with SourceForge.

To do the build:

* Go to the `DotNetCore_BuildScripts` directory and open a terminal.
* `clean`
* `load`
* `build`
* `blokscript.exe` will be in the `DotNetCore_Projects\BlokScript.BlokScriptApp\bin\Debug\net8.0` directory.

### Parser & Lexer Generation

You will need the following for this:

* Java Runtime
* ANTLR4 - `antlr-4.13.1-complete.jar`

To generate the lexer and parser files:

* Go to the `Grammar_BuildScripts` directory.
* Run the following command:

`java -jar antlr-4.13.1-complete.jar -o BlokScript.Parser -no-listener -visitor -Dlanguage=CSharp -package BlokScript.Parser BlokScriptGrammar.g4`

* The files generated are output to the `BlokScript.Parser` directory.
* Install these files by copying them to the `BlokScript.Parser` directory under `DotNetFramework_Projects` or `DotNetCore_Projects`.