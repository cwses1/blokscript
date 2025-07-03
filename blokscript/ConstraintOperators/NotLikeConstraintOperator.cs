using System;
using System.Text.RegularExpressions;

using BlokScript.Common;
using BlokScript.Entities;
using BlokScript.BlokScriptApp;

namespace BlokScript.ConstraintOperators
{
	public class NotLikeConstraintOperator
	{
		public static T[] Apply<T> (T[] Entities, string FieldName, object ConstraintData)
		{
			//
			// THE STARTS WITH FUNCTION CAN ONLY ACCEPT A SINGLE SYMBOL FOR THE CONSTRAINT.
			// IT CANNOT ACCEPT AN ARRAY.
			// WE DON'T CARE IF THE SYMBOL IS ANONYMOUS OR A NAMED - WE JUST NEED THE SYMBOL TYPE AND VALUE.
			//
			if (!(ConstraintData is BlokScriptSymbol))
				throw new NotImplementedException("Operator == ConstraintOperator.StartsWith");

			//
			// GET THE CONSTRAINT SYMBOL.
			//
			BlokScriptSymbol ConstraintSymbol = (BlokScriptSymbol)ConstraintData;

			//
			// THE CONSTRAINT MUST BE A CERTAIN TYPE.
			//
			if (ConstraintSymbol.Type != BlokScriptSymbolType.String && ConstraintSymbol.Type != BlokScriptSymbolType.Regex)
				throw new NotImplementedException("The starts with constraint accepts a string or regex only.");

			//
			// CREATE THE REGULAR EXPRESSION THAT IMPLEMENTS THE STARTS WITH FUNCTION.
			//
			BlokScriptSymbol RegexSymbol = new BlokScriptSymbol();
			RegexSymbol.Type = BlokScriptSymbolType.Regex;

			Regex StartsWithRegex;

			if (ConstraintSymbol.Type == BlokScriptSymbolType.String)
				StartsWithRegex = new Regex("^" + (string)ConstraintSymbol.Value);
			else
				StartsWithRegex = (Regex)ConstraintSymbol.Value;

			RegexSymbol.Value = StartsWithRegex;

			return DoesNotMatchRegexConstraintOperator.Apply<T>(Entities, FieldName, RegexSymbol);
		}
	}
}
