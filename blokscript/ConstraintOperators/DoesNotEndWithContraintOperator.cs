using System;
using System.Text.RegularExpressions;

using BlokScript.Common;
using BlokScript.Entities;
using BlokScript.BlokScriptApp;

namespace BlokScript.ConstraintOperators
{
	public class DoesNotEndWithContraintOperator
	{
		public static T[] Apply<T> (T[] Entities, string FieldName, object ConstraintData)
		{
			if (!(ConstraintData is BlokScriptSymbol))
				throw new NotImplementedException("Operator == ConstraintOperator.EndsWith");

			BlokScriptSymbol ConstraintSymbol = (BlokScriptSymbol)ConstraintData;

			if (ConstraintSymbol.Type != BlokScriptSymbolType.String)
				throw new NotImplementedException("The 'does not end with' constraint operator accepts a string argument only.");

			BlokScriptSymbol RegexSymbol = new BlokScriptSymbol();
			RegexSymbol.Type = BlokScriptSymbolType.Regex;

			Regex EndsWithRegex;

			if (ConstraintSymbol.Type == BlokScriptSymbolType.String)
				EndsWithRegex = new Regex((string)ConstraintSymbol.Value + "$");
			else
				EndsWithRegex = (Regex)ConstraintSymbol.Value;

			RegexSymbol.Value = EndsWithRegex;
			return DoesNotMatchRegexConstraintOperator.Apply<T>(Entities, FieldName, RegexSymbol);
		}
	}
}
