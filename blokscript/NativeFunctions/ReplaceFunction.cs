using System;

using BlokScript.Models;
using BlokScript.BlokScriptApp;
using BlokScript.Common;

namespace BlokScript.NativeFunctions
{
	public static class ReplaceFunction
	{
		public static BlokScriptSymbol Call (BlokScriptSymbol[] FnArgs)
		{
			BlokScriptSymbol Symbol = new BlokScriptSymbol();
			Symbol.Type = BlokScriptSymbolType.String;
			Symbol.Value = CallNative((string)FnArgs[0].Value, (string)FnArgs[1].Value, (string)FnArgs[2].Value);
			return Symbol;
		}

		public static string CallNative (string StringParam, string Arg1, string Arg2)
		{
			return StringParam.Replace(Arg1, Arg2);
		}
	}
}
