using System.Collections.Generic;
using BlokScript.Common;

namespace BlokScript.Parsers
{
	public class BlokScriptVerbosityParser
	{
		static BlokScriptVerbosityParser ()
		{
			_ParserHash = new Dictionary<string, BlokScriptVerbosity>();
			_ParserHash["verbose"] = BlokScriptVerbosity.Verbose;
			_ParserHash["quiet"] = BlokScriptVerbosity.Quiet;
		}

		public static BlokScriptVerbosity Parse (string VerbosityStringParam)
		{
			return _ParserHash[VerbosityStringParam];
		}


		private static Dictionary<string, BlokScriptVerbosity> _ParserHash;
	}
}
