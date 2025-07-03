using System;
using System.Collections;
using System.Text.RegularExpressions;

using BlokScript.BlokScriptApp;
using BlokScript.Parsers;
using BlokScript.Common;

namespace BlokScript.Comparators
{
	public class BlokScriptSymbolConstraintEqualsComparator
	{
		public static bool AreEqual (BlokScriptSymbol LeftSymbol, BlokScriptSymbol RightSymbol)
		{
			//
			// VERIFY THAT WE CAN EVEN COMPARE THESE SYMBOLS.
			//
			if (!CanCompare(LeftSymbol, RightSymbol))
				throw new NotImplementedException($"Cannot compare left symbol type {LeftSymbol.Type} with right symbol type {RightSymbol.Type} for equality.");

			return Compare(LeftSymbol, RightSymbol);
		}

		public static bool CanCompare (BlokScriptSymbol LeftSymbol, BlokScriptSymbol RightSymbol)
		{
			return LeftSymbol.Type == BlokScriptSymbolType.JsonNull
				|| RightSymbol.Type == BlokScriptSymbolType.JsonNull
				|| LeftSymbol.Type == RightSymbol.Type;
		}

		public static bool Compare (BlokScriptSymbol LeftSymbol, BlokScriptSymbol RightSymbol)
		{
			if (LeftSymbol.Type == BlokScriptSymbolType.NotAssigned
				|| LeftSymbol.Type == BlokScriptSymbolType.JsonNull
				|| RightSymbol.Type == BlokScriptSymbolType.NotAssigned
				|| RightSymbol.Type == BlokScriptSymbolType.JsonNull)
				return false;

			if (LeftSymbol.Type == BlokScriptSymbolType.JsonUndefined && RightSymbol.Type == BlokScriptSymbolType.JsonUndefined)
				return true;

			if (LeftSymbol.Type == BlokScriptSymbolType.Int32)
				return ((int)LeftSymbol.Value) == ((int)RightSymbol.Value);

			if (LeftSymbol.Type == BlokScriptSymbolType.String)
				return ((string)LeftSymbol.Value) == ((string)RightSymbol.Value);

			if (LeftSymbol.Type == BlokScriptSymbolType.Regex)
				return ((Regex)LeftSymbol.Value) == ((Regex)RightSymbol.Value);

			throw new NotImplementedException();
		}
	}
}
