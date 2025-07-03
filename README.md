# BlokScript

[BlokScript](https://www.blokscript.com) is a programming language for [Storyblok](https://www.storyblok.com).

## Building

You will need the following for this:

* [.NET Core 8.0](https://dotnet.microsoft.com/en-us/download)

### Parser & Lexer Generation

You will need the following for this:

* Java Runtime
* ANTLR4 - `antlr-4.13.1-complete.jar`

To generate the lexer and parser files:

* Go to the `grammar` directory.
* Run the following command:

`java -jar antlr-4.13.1-complete.jar -o BlokScript.Parser -no-listener -visitor -Dlanguage=CSharp -package BlokScript.Parser BlokScriptGrammar.g4`

* The generated lexer and parser files are output to the `grammar/BlokScript.Parser` directory.
* Install these files by copying them to the `blokscript/BlokScript.Parser` directory.
