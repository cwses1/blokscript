using System;
using System.Configuration;
using System.IO;
using System.Text;

using log4net;
using log4net.Config;

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
			
			//
			// GET CONFIGURATION.
			//
			_BaseDir = Directory.GetCurrentDirectory();
			_ConfigDir = _BaseDir;

			//
			// START THE LOG.
			//
			string Log4NetConfigPath = _ConfigDir + "\\log4net.config.xml";
			XmlConfigurator.ConfigureAndWatch(new FileInfo(Log4NetConfigPath));
			_Log = LogManager.GetLogger(typeof(Program));
			_Log.Debug("Initialized the log.");
			_Log.Info("Started.");

			try
			{
				MainInternal(Args);
			}
			catch (Exception E)
			{
				_Log.Error(E);
				throw;
			}
		}


		public static void MainInternal (string[] Args)
		{
			//
			// DO A QUICK SYSTEM CHECK IF NO ARGUMENTS.
			//
			if (Args.Length == 0)
			{
				StringBuilder MessageBuilder = new StringBuilder();

				if (!File.Exists(_BaseDir + "\\blokscript-env.json"))
					MessageBuilder.AppendLine("blokscript-env.json not found.");

				MessageBuilder.AppendLine("USAGE:");
				MessageBuilder.AppendLine("blokscript <file>");
				Console.Write(MessageBuilder.ToString());
				return;
			}



			//
			// blokscript filename.txt
			//
			if (Args.Length != 1)
			{
				//
				// NO SCRIPT NAME.  GIVE THE USER SOME HELP.
				//
				StringBuilder MessageBuilder = new StringBuilder();
				MessageBuilder.AppendLine("blokscript <file>");
				Console.Write(MessageBuilder.ToString());
				return;
			}

			string ScriptFilePath = Args[0];

			//
			// ENSURE THE SCRIPT FILE EXISTS.
			//
			if (!File.Exists(ScriptFilePath))
			{
				_Log.Error("Could not open script file.");
				return;
			}

			//
			// READ THE FILE CONTENTS INTO A STRING.
			//
			string ScriptString;

			using (StreamReader ScriptFileReader = new StreamReader(ScriptFilePath))
			{
				ScriptString = ScriptFileReader.ReadToEnd();
			}

			//
			// PARSE THE SCRIPT.
			//
			try
			{
				Parse(ScriptString);
			}
			catch (ParseCanceledException E)
			{
				_Log.Error("An error occurred during parsing.");
				_Log.Error(E);
				throw;
				//InputMismatchException InnerException = (InputMismatchException)E.InnerException;
				//(Recognizer<Symbol, ATNSimulator>)InnerException.Recognizer;
			}
			catch (Exception E)
			{
				_Log.Error("An error occurred during parsing.");
				_Log.Error(E);
			}
		}


		public static void Parse (string ScriptString)
		{
			ICharStream stream = CharStreams.fromString(ScriptString);
			ITokenSource lexer = new BlokScriptGrammarConcreteLexer(stream);
			ITokenStream tokens = new CommonTokenStream(lexer);
			BlokScriptGrammarParser parser = new BlokScriptGrammarParser(tokens);
			parser.ErrorHandler = new BailErrorStrategy();
			new BlokScriptGrammarConcreteVisitor().Visit(parser.script());
		}


		private static DateTime _CurrentDateTime;
		private static string _BaseDir;
		private static string _ConfigDir;
		public static ILog _Log;
	}
}
