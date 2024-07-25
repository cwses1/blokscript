using System;
using System.Collections.Generic;

using BlokScript.Common;
using BlokScript.Entities;
using BlokScript.BlokScriptApp;
using BlokScript.Extractors;
using BlokScript.Comparators;

namespace BlokScript.ConstraintOperators
{
	public class EqualsContraintOperator
	{
		public static T[] Apply<T> (T[] Entities, string FieldName, object ConstraintData)
		{
			//
			// THE EQUALS OPERATOR CAN ONLY ACCEPT A SINGLE SYMBOL FOR THE CONSTRAINT.
			// IT CANNOT ACCEPT AN ARRAY.
			// WE DON'T CARE IF THE SYMBOL IS ANONYMOUS OR NAMED - WE USE THE SYMBOL TYPE AND VALUE.
			//
			if (!(ConstraintData is BlokScriptSymbol))
				throw new NotImplementedException();

			//
			// GET THE CONSTRAINT SYMBOL.
			//
			BlokScriptSymbol ConstraintSymbol = (BlokScriptSymbol)ConstraintData;

			List<T> OutList = new List<T>();

			foreach (T Entity in Entities)
			{
				//
				// GET THE FIELD AS A BLOKSCRIPT SYMBOL.
				//
				BlokScriptSymbol FieldSymbol = EntityValueExtractor.ExtractSymbolFromEntity(Entity, FieldName);

				if (BlokScriptSymbolConstraintComparator.AreEqual(FieldSymbol, ConstraintSymbol))
					OutList.Add(Entity);
			}

			return OutList.ToArray();
		}
	}
}
