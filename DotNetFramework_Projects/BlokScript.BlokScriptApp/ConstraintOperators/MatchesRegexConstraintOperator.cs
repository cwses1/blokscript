using System.Collections.Generic;
using System.Text.RegularExpressions;

using BlokScript.BlokScriptApp;
using BlokScript.Common;
using BlokScript.Entities;
using BlokScript.Extractors;

namespace BlokScript.ConstraintOperators
{
	public class MatchesRegexConstraintOperator
	{
		public static T[] Apply<T> (T[] Entities, string FieldName, Regex ConstraintRegex)
		{
			List<T> OutList = new List<T>();

			foreach (T Entity in Entities)
			{
				//
				// EXTRACT THE FIELD VALUE FROM THE ENTITY.
				//
				string FieldValue = EntityValueExtractor.ExtractString(Entity, FieldName);

				if (ConstraintRegex.IsMatch(FieldValue))
				{
					OutList.Add(Entity);
				}
			}

			return OutList.ToArray();
		}
	}
}
