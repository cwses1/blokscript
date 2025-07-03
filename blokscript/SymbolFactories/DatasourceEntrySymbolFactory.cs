using System.Collections.Generic;

using BlokScript.BlokScriptApp;
using BlokScript.Common;
using BlokScript.Entities;

namespace BlokScript.SymbolFactories
{
	public class DatasourceEntrySymbolFactory
	{
		public static BlokScriptSymbol[] CreateSymbols (DatasourceEntryEntity[] Stories)
		{
			List<BlokScriptSymbol> SymbolList = new List<BlokScriptSymbol>();

			foreach (DatasourceEntryEntity DatasourceEntry in Stories)
			{
				SymbolList.Add(CreateSymbol(DatasourceEntry));
			}

			return SymbolList.ToArray();
		}

		public static BlokScriptSymbol CreateSymbol (DatasourceEntryEntity DatasourceEntry)
		{
			BlokScriptSymbol Symbol = new BlokScriptSymbol();
			Symbol.Value = DatasourceEntry;
			Symbol.Type = BlokScriptSymbolType.DatasourceEntry;
			return Symbol;
		}
	}
}
