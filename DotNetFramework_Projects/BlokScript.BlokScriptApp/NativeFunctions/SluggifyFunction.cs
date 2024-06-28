using System;

using BlokScript.Models;
using BlokScript.BlokScriptApp;
using BlokScript.Common;

namespace BlokScript.NativeFunctions
{
	public static class SluggifyFunction
	{
		public static BlokScriptSymbol Call (BlokScriptSymbol[] FnArgs)
		{
			BlokScriptSymbol Symbol = new BlokScriptSymbol();
			Symbol.Type = BlokScriptSymbolType.String;
			Symbol.Value = CallNative((string)FnArgs[0].Value);
			return Symbol;
		}


		public static string CallNative (string StringParam)
		{
			return StringParam.Trim().ToLower().Replace(' ', '-');
		}
	}
}
