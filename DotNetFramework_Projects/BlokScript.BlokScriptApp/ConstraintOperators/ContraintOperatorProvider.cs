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

			if (Operator == ConstraintOperator.In)
				return InContraintOperator.Apply<T>(Entities, FieldName, ConstraintData);

			if (Operator == ConstraintOperator.NotIn)
				return NotInContraintOperator.Apply<T>(Entities, FieldName, ConstraintData);

			if (Operator == ConstraintOperator.StartsWith)
				return StartsWithContraintOperator.Apply<T>(Entities, FieldName, ConstraintData);
			
			if (Operator == ConstraintOperator.EndsWith)
				return EndsWithContraintOperator.Apply<T>(Entities, FieldName, ConstraintData);

			if (Operator == ConstraintOperator.IsJsonNull)
				return IsJsonNullContraintOperator.Apply<T>(Entities, FieldName, ConstraintData);

			if (Operator == ConstraintOperator.IsNotJsonNull)
				return IsNotJsonNullContraintOperator.Apply<T>(Entities, FieldName, ConstraintData);

			if (Operator == ConstraintOperator.IsJsonUndefined)
				return IsJsonUndefinedContraintOperator.Apply<T>(Entities, FieldName, ConstraintData);

			if (Operator == ConstraintOperator.IsNotJsonUndefined)
				return IsNotJsonUndefinedContraintOperator.Apply<T>(Entities, FieldName, ConstraintData);

			if (Operator == ConstraintOperator.MatchesRegex)
				return MatchesRegexConstraintOperator.Apply<T>(Entities, FieldName, ConstraintData);

			if (Operator == ConstraintOperator.DoesNotMatchRegex)
				return DoesNotMatchRegexConstraintOperator.Apply<T>(Entities, FieldName, ConstraintData);

			if (Operator == ConstraintOperator.Like)
				return LikeConstraintOperator.Apply<T>(Entities, FieldName, ConstraintData);

			if (Operator == ConstraintOperator.NotLike)
				return NotLikeConstraintOperator.Apply<T>(Entities, FieldName, ConstraintData);

			throw new NotImplementedException();
		}
	}
}
