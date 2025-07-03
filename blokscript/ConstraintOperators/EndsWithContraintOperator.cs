using System;
using System.Text.RegularExpressions;

using BlokScript.Common;
using BlokScript.Entities;
using BlokScript.BlokScriptApp;

namespace BlokScript.ConstraintOperators
{
	public class EndsWithContraintOperator
	{
		public static T[] Apply<T> (T[] Entities, string FieldName, object ConstraintData)
		{
			if (!(ConstraintData is BlokScriptSymbol))
				throw new NotImplementedException("Operator == ConstraintOperator.EndsWith");

			BlokScriptSymbol ConstraintSymbol = (BlokScriptSymbol)ConstraintData;

			if (ConstraintSymbol.Type != BlokScriptSymbolType.String && ConstraintSymbol.Type != BlokScriptSymbolType.Regex)
				throw new NotImplementedException("The ends with constraint operator accepts a string or regex argument only.");

			BlokScriptSymbol RegexSymbol = new BlokScriptSymbol();
			RegexSymbol.Type = BlokScriptSymbolType.Regex;

			Regex EndsWithRegex;

			if (ConstraintSymbol.Type == BlokScriptSymbolType.String)
				EndsWithRegex = new Regex((string)ConstraintSymbol.Value + "$");
			else
				EndsWithRegex = (Regex)ConstraintSymbol.Value;

			RegexSymbol.Value = EndsWithRegex;
			return MatchesRegexConstraintOperator.Apply<T>(Entities, FieldName, RegexSymbol);
		}
	}
}
