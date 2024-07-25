using System;
using System.Text.RegularExpressions;

using BlokScript.Common;
using BlokScript.Entities;
using BlokScript.BlokScriptApp;
using BlokScript.ConstraintOperators;

namespace BlokScript.ConstraintOperators
{
	public class ContraintOperatorProvider
	{
		public static T[] Apply<T> (T[] Entities, ConstraintOperator Operator, string FieldName, object ConstraintData)
		{
			if (Operator == ConstraintOperator.Equals)
				return EqualsContraintOperator.Apply<T>(Entities, FieldName, ConstraintData);

			if (Operator == ConstraintOperator.NotEquals)
				return NotEqualsContraintOperator.Apply<T>(Entities, FieldName, ConstraintData);

			if (Operator == ConstraintOperator.StartsWith)
				return StartsWithContraintOperator.Apply<T>(Entities, FieldName, ConstraintData);
			
			if (Operator == ConstraintOperator.EndsWith)
				return EndsWithContraintOperator.Apply<T>(Entities, FieldName, ConstraintData);

			throw new NotImplementedException();
		}
	}
}
