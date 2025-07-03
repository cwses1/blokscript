using System;
using System.Collections;
using System.Text.RegularExpressions;

using BlokScript.BlokScriptApp;
using BlokScript.Parsers;
using BlokScript.Common;

namespace BlokScript.Comparators
{
	public class BlokScriptSymbolConstraintRegexMatchComparator
	{
		public static bool IsMatch (BlokScriptSymbol LeftSymbol, BlokScriptSymbol RightSymbol)
		{
			//
			// VERIFY THAT WE CAN EVEN COMPARE THESE SYMBOLS.
			//
			if (!CanMatch(LeftSymbol, RightSymbol))
				throw new NotImplementedException($"Cannot compare left symbol type {LeftSymbol.Type} with right symbol type {RightSymbol.Type} for equality.");

			return Match(LeftSymbol, RightSymbol);
		}

		public static bool CanMatch (BlokScriptSymbol LeftSymbol, BlokScriptSymbol RightSymbol)
		{
			return LeftSymbol.Type == BlokScriptSymbolType.JsonNull
				|| LeftSymbol.Type == BlokScriptSymbolType.String && RightSymbol.Type == BlokScriptSymbolType.Regex
				|| LeftSymbol.Type == BlokScriptSymbolType.String && RightSymbol.Type == BlokScriptSymbolType.String;
		}

		public static bool Match (BlokScriptSymbol LeftSymbol, BlokScriptSymbol RightSymbol)
		{
			if (LeftSymbol.Type == BlokScriptSymbolType.JsonNull)
				return false;

			if (RightSymbol.Type == BlokScriptSymbolType.Regex)
				return ((Regex)RightSymbol.Value).IsMatch((string)LeftSymbol.Value);

			if (RightSymbol.Type == BlokScriptSymbolType.String)
				return new Regex((string)RightSymbol.Value).IsMatch((string)LeftSymbol.Value);

			throw new NotImplementedException();
		}
	}
}
