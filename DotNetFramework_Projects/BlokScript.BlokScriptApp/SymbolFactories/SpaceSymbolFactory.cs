using System.Collections.Generic;

using BlokScript.BlokScriptApp;
using BlokScript.Common;
using BlokScript.Entities;

namespace BlokScript.SymbolFactories
{
	public class SpaceSymbolFactory
	{
		public static BlokScriptSymbol[] CreateSymbols (SpaceEntity[] Stories)
		{
			List<BlokScriptSymbol> SymbolList = new List<BlokScriptSymbol>();

			foreach (SpaceEntity Space in Stories)
			{
				SymbolList.Add(CreateSymbol(Space));
			}

			return SymbolList.ToArray();
		}

		public static BlokScriptSymbol CreateSymbol (SpaceEntity Space)
		{
			BlokScriptSymbol Symbol = new BlokScriptSymbol();
			Symbol.Value = Space;
			Symbol.Type = Common.BlokScriptSymbolType.Space;
			return Symbol;
		}
	}
}
