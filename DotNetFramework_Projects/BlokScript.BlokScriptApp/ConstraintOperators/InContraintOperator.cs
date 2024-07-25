using System;
using System.Collections.Generic;

using BlokScript.Common;
using BlokScript.Entities;
using BlokScript.BlokScriptApp;
using BlokScript.Extractors;
using BlokScript.Comparators;

namespace BlokScript.ConstraintOperators
{
	public class InContraintOperator
	{
		public static T[] Apply<T> (T[] Entities, string FieldName, object ConstraintData)
		{
			if (!(ConstraintData is BlokScriptSymbol[]))
				throw new NotImplementedException();

			BlokScriptSymbol[] ConstraintSymbolsArray = (BlokScriptSymbol[])ConstraintData;

			List<T> OutList = new List<T>();

			foreach (T Entity in Entities)
			{
				foreach (BlokScriptSymbol ConstraintSymbol in ConstraintSymbolsArray)
				{
					BlokScriptSymbol FieldSymbol = EntityValueExtractor.ExtractSymbolFromEntity(Entity, FieldName);

					if (BlokScriptSymbolConstraintComparator.AreEqual(FieldSymbol, ConstraintSymbol))
					{
						OutList.Add(Entity);
						break;
					}
				}
			}

			return OutList.ToArray();
		}
	}
}
