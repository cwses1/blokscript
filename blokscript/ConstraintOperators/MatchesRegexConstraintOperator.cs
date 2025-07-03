using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using BlokScript.BlokScriptApp;
using BlokScript.Common;
using BlokScript.Extractors;
using BlokScript.Comparators;

namespace BlokScript.ConstraintOperators
{
	public class MatchesRegexConstraintOperator
	{
		public static T[] Apply<T> (T[] Entities, string FieldName, object ConstraintData)
		{
			if (!(ConstraintData is BlokScriptSymbol))
				throw new NotImplementedException();

			BlokScriptSymbol ConstraintSymbol = (BlokScriptSymbol)ConstraintData;

			List<T> OutList = new List<T>();

			foreach (T Entity in Entities)
			{
				BlokScriptSymbol FieldSymbol = EntityValueExtractor.ExtractSymbolFromEntity(Entity, FieldName);

				if (BlokScriptSymbolConstraintRegexMatchComparator.IsMatch(FieldSymbol, ConstraintSymbol))
					OutList.Add(Entity);
			}

			return OutList.ToArray();
		}
	}
}
