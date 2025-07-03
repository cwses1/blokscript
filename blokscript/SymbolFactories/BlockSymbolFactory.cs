using System.Collections.Generic;

using BlokScript.BlokScriptApp;
using BlokScript.Common;
using BlokScript.Entities;

namespace BlokScript.SymbolFactories
{
	public class BlockSymbolFactory
	{
		public static BlokScriptSymbol[] CreateSymbols (BlockSchemaEntity[] Stories)
		{
			List<BlokScriptSymbol> SymbolList = new List<BlokScriptSymbol>();

			foreach (BlockSchemaEntity Block in Stories)
			{
				SymbolList.Add(CreateSymbol(Block));
			}

			return SymbolList.ToArray();
		}

		public static BlokScriptSymbol CreateSymbol (BlockSchemaEntity Block)
		{
			BlokScriptSymbol Symbol = new BlokScriptSymbol();
			Symbol.Value = Block;
			Symbol.Type = BlokScriptSymbolType.Block;
			return Symbol;
		}
	}
}
