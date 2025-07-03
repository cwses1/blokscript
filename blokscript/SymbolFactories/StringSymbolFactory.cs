using System.Collections.Generic;

using BlokScript.BlokScriptApp;
using BlokScript.Common;
using BlokScript.Entities;

namespace BlokScript.SymbolFactories
{
	public class StringSymbolFactory
	{
		public static BlokScriptSymbol[] CreateSymbols (string[] Strings)
		{
			List<BlokScriptSymbol> SymbolList = new List<BlokScriptSymbol>();

			foreach (string CurrentString in Strings)
			{
				SymbolList.Add(CreateSymbol(CurrentString));
			}

			return SymbolList.ToArray();
		}

		public static BlokScriptSymbol CreateSymbol (string StringParam)
		{
			BlokScriptSymbol Symbol = new BlokScriptSymbol();
			Symbol.Value = StringParam;
			Symbol.Type = BlokScriptSymbolType.String;
			return Symbol;
		}
	}
}
