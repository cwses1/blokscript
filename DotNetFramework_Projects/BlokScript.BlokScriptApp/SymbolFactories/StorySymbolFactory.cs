using System.Collections.Generic;

using BlokScript.BlokScriptApp;
using BlokScript.Common;
using BlokScript.Entities;

namespace BlokScript.SymbolFactories
{
	public class StorySymbolFactory
	{
		public static BlokScriptSymbol[] CreateSymbols (StoryEntity[] Stories)
		{
			List<BlokScriptSymbol> SymbolList = new List<BlokScriptSymbol>();

			foreach (StoryEntity Story in Stories)
			{
				SymbolList.Add(CreateSymbol(Story));
			}

			return SymbolList.ToArray();
		}

		public static BlokScriptSymbol CreateSymbol (StoryEntity Story)
		{
			BlokScriptSymbol Symbol = new BlokScriptSymbol();
			Symbol.Value = Story;
			Symbol.Type = Common.BlokScriptSymbolType.Story;
			return Symbol;
		}
	}
}
