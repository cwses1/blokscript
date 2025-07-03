using System;
using System.Collections.Generic;

using BlokScript.Common;
using BlokScript.Entities;
using BlokScript.BlokScriptApp;
using BlokScript.Extractors;
using BlokScript.Comparators;

namespace BlokScript.ConstraintOperators
{
	public class IsNotJsonUndefinedContraintOperator
	{
		public static T[] Apply<T> (T[] Entities, string FieldName, object ConstraintData)
		{
			List<T> OutList = new List<T>();

			foreach (T Entity in Entities)
			{
				BlokScriptSymbol FieldSymbol = EntityValueExtractor.ExtractSymbolFromEntity(Entity, FieldName);

				if (FieldSymbol.Type != BlokScriptSymbolType.JsonUndefined)
					OutList.Add(Entity);
			}

			return OutList.ToArray();
		}
	}
}
