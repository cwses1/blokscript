using System.Collections.Generic;
using System.Text.RegularExpressions;

using BlokScript.BlokScriptApp;
using BlokScript.Common;
using BlokScript.Entities;

namespace BlokScript.SymbolFactories
{
	public class RegexSymbolFactory
	{
		public static BlokScriptSymbol[] CreateSymbols (Regex[] Regexes)
		{
			List<BlokScriptSymbol> SymbolList = new List<BlokScriptSymbol>();

			foreach (Regex CurrentRegex in Regexes)
			{
				SymbolList.Add(CreateSymbol(CurrentRegex));
			}

			return SymbolList.ToArray();
		}

		public static BlokScriptSymbol CreateSymbol (Regex RegexParam)
		{
			BlokScriptSymbol Symbol = new BlokScriptSymbol();
			Symbol.Value = RegexParam;
			Symbol.Type = BlokScriptSymbolType.Regex;
			return Symbol;
		}
	}
}
