using System;
using System.Configuration;
using System.IO;
using System.Text;

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Atn;

using BlokScript.Parser;

namespace BlokScript.BlokScriptApp
{
	public class Program
	{
		public static void Main (string[] Args)
		{
			//
			// GET CURRENT TIME STAMP.
			//
			_CurrentDateTime = DateTime.Now;
			
			try
			{
				MainInternal(Args);
			}
			catch (Exception E)
			{
				Console.WriteLine(E.Message);
				Console.WriteLine(E.StackTrace);
			}
		}


		public static void MainInternal (string[] Args)
		{
			string BlokScriptEnvFilePath = Directory.GetCurrentDirectory() + "\\blokscript-env.json";
			Console.WriteLine(BlokScriptEnvFilePath);

			if (!File.Exists(BlokScriptEnvFilePath))
			{
				Console.WriteLine("blokscript-env.json not found.");
				return;
			}

			if (Args.Length == 0)
			{
				StringBuilder MessageBuilder = new StringBuilder();
				MessageBuilder.AppendLine("USAGE:");
				MessageBuilder.AppendLine("blokscript -f <file>");
				MessageBuilder.AppendLine("blokscript -s \"<script>\"");
				Console.Write(MessageBuilder.ToString());
				return;
			}

			if (Args.Length != 2)
			{
				//
				// NO SCRIPT NAME.  GIVE THE USER SOME HELP.
				//
				StringBuilder MessageBuilder = new StringBuilder();
				MessageBuilder.AppendLine("blokscript -f <file>");
				MessageBuilder.AppendLine("blokscript -s \"<script>\"");
				Console.Write(MessageBuilder.ToString());
				return;
			}

			string Option = Args[0];
			string ScriptOrFilePath = Args[1];
			string ScriptString;

			if (Option == "-s")
			{
				ScriptString = ScriptOrFilePath;
			}
			else if (Option == "-f")
			{
				string ScriptFilePath = ScriptOrFilePath;

				//
				// ENSURE THE SCRIPT FILE EXISTS.
				//
				if (!File.Exists(ScriptFilePath))
				{
					return;
				}

				//
				// READ THE FILE CONTENTS INTO A STRING.
				//
				using (StreamReader ScriptFileReader = new StreamReader(ScriptFilePath))
				{
					ScriptString = ScriptFileReader.ReadToEnd();
				}
			}
			else
			{
				StringBuilder MessageBuilder = new StringBuilder();
				MessageBuilder.AppendLine($"Invalid option: '{Option}'.");
				MessageBuilder.AppendLine("USAGE:");
				MessageBuilder.AppendLine("blokscript -f <file>");
				MessageBuilder.AppendLine("blokscript -s \"<script>\"");
				Console.Write(MessageBuilder.ToString());
				return;
			}

			//
			// PARSE THE SCRIPT.
			//
			try
			{
				Parse(ScriptString);
			}
			catch (Exception E)
			{
				Console.WriteLine(E.Message);
				Console.WriteLine(E.StackTrace);
			}
		}


		public static void Parse (string ScriptString)
		{
			ICharStream stream = CharStreams.fromString(ScriptString);
			ITokenSource lexer = new BlokScriptGrammarConcreteLexer(stream);
			ITokenStream tokens = new CommonTokenStream(lexer);
			BlokScriptGrammarParser parser = new BlokScriptGrammarParser(tokens);
			parser.ErrorHandler = new BailErrorStrategy();

			//
			// CREATE THE VISITOR.
			//
			BlokScriptGrammarConcreteVisitor CreatedVisitor = new BlokScriptGrammarConcreteVisitor();
			CreatedVisitor.WorkingDir = Directory.GetCurrentDirectory();
			CreatedVisitor.Visit(parser.script());
		}

		private static DateTime _CurrentDateTime;
	}
}
