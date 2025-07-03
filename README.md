# BlokScript

[BlokScript](https://www.blokscript.com) is a programming language for [Storyblok](https://www.storyblok.com).

## Building

BlokScript is written in C#.  Install the following:
* [.NET 9.0](https://dotnet.microsoft.com/en-us/download).

```
% git clone https://github.com/cwses1/blokscript.git
% cd blokscript/blokscript
% dotnet build
% cd bin/Debug/net9.0
% ./blokscript
```

### Parser & Lexer Generation

For BlokScript developers.  Install the following:
* [OpenJDK](https://jdk.java.net/24/)
* [ANTLR4](https://www.antlr.org/download.html) - get the `antlr-4.13.1-complete.jar` file into the `grammar` directory.

Generate the lexer and parser files:

```
% cd grammar
% java -jar antlr-4.13.1-complete.jar -o BlokScript.Parser -no-listener -visitor -Dlanguage=CSharp -package BlokScript.Parser BlokScriptGrammar.g4
```

* The generated lexer and parser files are output to the `grammar/BlokScript.Parser` directory.
* Install these files by copying them to the `blokscript/BlokScript.Parser` directory.
