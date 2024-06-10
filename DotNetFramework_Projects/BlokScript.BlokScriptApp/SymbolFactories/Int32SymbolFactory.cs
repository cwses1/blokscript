using System.Collections.Generic;

using BlokScript.BlokScriptApp;
using BlokScript.Common;
using BlokScript.Entities;

namespace BlokScript.SymbolFactories
{
	public class Int32SymbolFactory
	{
		public static BlokScriptSymbol[] CreateSymbols (int[] IntArrayParam)
		{
			List<BlokScriptSymbol> SymbolList = new List<BlokScriptSymbol>();

			foreach (int CurrentInt32 in IntArrayParam)
			{
				SymbolList.Add(CreateSymbol(CurrentInt32));
			}

			return SymbolList.ToArray();
		}

		public static BlokScriptSymbol CreateSymbol (int Int32Param)
		{
			BlokScriptSymbol Symbol = new BlokScriptSymbol();
			Symbol.Value = Int32Param;
			Symbol.Type = BlokScriptSymbolType.Int32;
			return Symbol;
		}
	}
}
