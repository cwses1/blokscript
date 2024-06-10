using System.Collections.Generic;

using BlokScript.BlokScriptApp;
using BlokScript.Common;
using BlokScript.Entities;

namespace BlokScript.SymbolFactories
{
	public class DatasourceSymbolFactory
	{
		public static BlokScriptSymbol[] CreateSymbols (DatasourceEntity[] Stories)
		{
			List<BlokScriptSymbol> SymbolList = new List<BlokScriptSymbol>();

			foreach (DatasourceEntity Datasource in Stories)
			{
				SymbolList.Add(CreateSymbol(Datasource));
			}

			return SymbolList.ToArray();
		}

		public static BlokScriptSymbol CreateSymbol (DatasourceEntity Datasource)
		{
			BlokScriptSymbol Symbol = new BlokScriptSymbol();
			Symbol.Value = Datasource;
			Symbol.Type = BlokScriptSymbolType.Datasource;
			return Symbol;
		}
	}
}
