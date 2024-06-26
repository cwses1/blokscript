﻿using System;
using BlokScript.Entities;
using BlokScript.Common;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BlokScript.Filters
{
	public class SpaceConstraint
	{
		public SpaceEntity[] Evaluate (SpaceEntity[] Spaces)
		{
			List<SpaceEntity> InList = new List<SpaceEntity>();
			InList.AddRange(Spaces);
			List<SpaceEntity> OutList = new List<SpaceEntity>();

			if (Operator == SpaceConstraintOperator.Root)
			{
				//
				// THE ROOT OPERATOR IS SPECIAL.
				// IT HAS ONE CHILD AND ONLY THE CHILD IS EVALUATED.
				//
				return ChildConstraint.Evaluate(Spaces);
			}
			else if (Operator == SpaceConstraintOperator.Intersect)
			{
				//
				// THE INTERSECTION OPERATOR PERFORMS AN "AND" OPERATION BETWEEN SIBLINGS.
				//
				return Intersect(LeftChildConstraint.Evaluate(Spaces), RightChildConstraint.Evaluate(Spaces));
			}
			else if (Operator == SpaceConstraintOperator.Union)
			{
				//
				// THE UNION OPERATOR PERFORMS AN "OR" OPERATION BETWEEN SIBLINGS.
				//
				return Union(LeftChildConstraint.Evaluate(Spaces), RightChildConstraint.Evaluate(Spaces));
			}
			else if (Field == SpaceConstraintField.Id)
			{
				//
				// CONSTRAIN BY ID.
				//
				if (Operator == SpaceConstraintOperator.Equals)
				{
					int ContraintValue = (int)ConstraintData;

					foreach (SpaceEntity Space in InList)
					{
						if (Space.SpaceId != null && Int32.Parse(Space.SpaceId) == ContraintValue)
						{
							OutList.Add(Space);
						}
					}
				}
				else if (Operator == SpaceConstraintOperator.NotEquals)
				{
					int ContraintValue = (int)ConstraintData;

					foreach (SpaceEntity Space in InList)
					{
						if (Space.SpaceId != null && Int32.Parse(Space.SpaceId) != ContraintValue)
						{
							OutList.Add(Space);
						}
					}
				}
				else if (Operator == SpaceConstraintOperator.In)
				{
					int[] ContraintValues = (int[])ConstraintData;

					foreach (SpaceEntity Space in InList)
					{
						foreach (int CurrentConstraintValue in ContraintValues)
						{
							if (Space.SpaceId != null && CurrentConstraintValue == Int32.Parse(Space.SpaceId))
							{
								OutList.Add(Space);
								break;
							}
						}
					}
				}
				else if (Operator == SpaceConstraintOperator.NotIn)
				{
					int[] ContraintValues = (int[])ConstraintData;

					foreach (SpaceEntity Space in InList)
					{
						foreach (int CurrentConstraintValue in ContraintValues)
						{
							if (Space.SpaceId != null && CurrentConstraintValue != Int32.Parse(Space.SpaceId))
							{
								OutList.Add(Space);
								break;
							}
						}
					}
				}
				else
					throw new NotImplementedException("SpaceConstraint");
			}
			if (Field == SpaceConstraintField.Name)
			{
				//
				// CONSTRAIN BY NAME.
				//
				if (Operator == SpaceConstraintOperator.Equals)
				{
					string ContraintParam = (string)ConstraintData;

					foreach (SpaceEntity Space in InList)
					{
						if (Space.Name == ContraintParam)
						{
							OutList.Add(Space);
						}
					}
				}
				else if (Operator == SpaceConstraintOperator.NotEquals)
				{
					string ContraintParam = (string)ConstraintData;

					foreach (SpaceEntity Space in InList)
					{
						if (Space.Name != ContraintParam)
						{
							OutList.Add(Space);
						}
					}
				}
				else if (Operator == SpaceConstraintOperator.In)
				{
					if (ConstraintDataType == SpaceConstraintDataType.StringList)
					{
						string[] ContraintParamList = (string[])ConstraintData;

						foreach (SpaceEntity Space in InList)
						{
							foreach (string ContraintParam in ContraintParamList)
							{
								if (Space.Name == ContraintParam)
								{
									OutList.Add(Space);
									break;
								}
							}
						}
					}
					else if (ConstraintDataType == SpaceConstraintDataType.RegexList)
					{
						Regex[] ContraintParamList = (Regex[])ConstraintData;

						foreach (SpaceEntity Space in InList)
						{
							foreach (Regex ContraintParam in ContraintParamList)
							{
								if (ContraintParam.IsMatch(Space.Name))
								{
									OutList.Add(Space);
									break;
								}
							}
						}
					}
				}
				else if (Operator == SpaceConstraintOperator.NotIn)
				{
					if (ConstraintDataType == SpaceConstraintDataType.StringList)
					{
						string[] ContraintParamList = (string[])ConstraintData;

						foreach (SpaceEntity Space in InList)
						{
							foreach (string ContraintParam in ContraintParamList)
							{
								if (Space.Name != ContraintParam)
								{
									OutList.Add(Space);
									break;
								}
							}
						}
					}
					else if (ConstraintDataType == SpaceConstraintDataType.RegexList)
					{
						Regex[] ContraintParamList = (Regex[])ConstraintData;

						foreach (SpaceEntity Space in InList)
						{
							foreach (Regex ContraintParam in ContraintParamList)
							{
								if (!ContraintParam.IsMatch(Space.Name))
								{
									OutList.Add(Space);
									break;
								}
							}
						}
					}
				}
				else if (Operator == SpaceConstraintOperator.MatchesRegex)
				{
					Regex ContraintParam = (Regex)ConstraintData;

					foreach (SpaceEntity Space in InList)
					{
						if (ContraintParam.IsMatch(Space.Name))
						{
							OutList.Add(Space);
						}
					}
				}
				else if (Operator == SpaceConstraintOperator.DoesNotMatchRegex)
				{
					Regex ContraintParam = (Regex)ConstraintData;

					foreach (SpaceEntity Space in InList)
					{
						if (!ContraintParam.IsMatch(Space.Name))
						{
							OutList.Add(Space);
						}
					}
				}
				else if (Operator == SpaceConstraintOperator.Like)
				{
					string ContraintParam = (string)ConstraintData;

					//
					// oil_ -> oil. 
					//
					ContraintParam = ContraintParam.Replace("_", ".");

					//
					// oil% -> oil.* 
					//
					ContraintParam = ContraintParam.Replace("%", ".*");
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (SpaceEntity Space in InList)
					{
						if (ConstraintRegex.IsMatch(Space.Name))
						{
							OutList.Add(Space);
						}
					}
				}
				else if (Operator == SpaceConstraintOperator.NotLike)
				{
					string ContraintParam = (string)ConstraintData;

					//
					// oil_ -> oil. 
					//
					ContraintParam = ContraintParam.Replace("_", ".");

					//
					// oil% -> oil.* 
					//
					ContraintParam = ContraintParam.Replace("%", ".*");
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (SpaceEntity Space in InList)
					{
						if (!ConstraintRegex.IsMatch(Space.Name))
						{
							OutList.Add(Space);
						}
					}
				}
				else if (Operator == SpaceConstraintOperator.StartsWith)
				{
					string ContraintParam = "^" + (string)ConstraintData;
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (SpaceEntity Space in InList)
					{
						if (ConstraintRegex.IsMatch(Space.Name))
						{
							OutList.Add(Space);
						}
					}
				}
				else if (Operator == SpaceConstraintOperator.DoesNotStartWith)
				{
					string ContraintParam = "^" + (string)ConstraintData;
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (SpaceEntity Space in InList)
					{
						if (!ConstraintRegex.IsMatch(Space.Name))
						{
							OutList.Add(Space);
						}
					}
				}
				else if (Operator == SpaceConstraintOperator.EndsWith)
				{
					string ContraintParam = (string)ConstraintData + "$";
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (SpaceEntity Space in InList)
					{
						if (ConstraintRegex.IsMatch(Space.Name))
						{
							OutList.Add(Space);
						}
					}
				}
				else if (Operator == SpaceConstraintOperator.DoesNotEndWith)
				{
					string ContraintParam = (string)ConstraintData + "$";
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (SpaceEntity Space in InList)
					{
						if (!ConstraintRegex.IsMatch(Space.Name))
						{
							OutList.Add(Space);
						}
					}
				}
				else
					throw new NotImplementedException();
			}

			return OutList.ToArray();
		}

		public static SpaceEntity[] Intersect (SpaceEntity[] LeftSpaceEntries, SpaceEntity[] RightSpaceEntries)
		{
			Dictionary<string, SpaceEntity> LeftHash = new Dictionary<string, SpaceEntity>();
			Dictionary<string, SpaceEntity> RightHash = new Dictionary<string, SpaceEntity>();
			List<SpaceEntity> OutList = new List<SpaceEntity>();

			foreach (SpaceEntity LeftSpace in LeftSpaceEntries)
			{
				LeftHash[LeftSpace.SpaceId] = LeftSpace;
			}

			foreach (SpaceEntity RightSpace in RightSpaceEntries)
			{
				RightHash[RightSpace.SpaceId] = RightSpace;
			}

			foreach (string SpaceId in LeftHash.Keys)
			{
				if (RightHash.ContainsKey(SpaceId))
					OutList.Add(RightHash[SpaceId]);
			}

			return OutList.ToArray();
		}

		public static SpaceEntity[] Union (SpaceEntity[] LeftSpaceEntries, SpaceEntity[] RightSpaceEntries)
		{
			Dictionary<string, SpaceEntity> UnionHash = new Dictionary<string, SpaceEntity>();
			List<SpaceEntity> OutList = new List<SpaceEntity>();

			foreach (SpaceEntity LeftSpace in LeftSpaceEntries)
			{
				UnionHash[LeftSpace.SpaceId] = LeftSpace;
			}

			foreach (SpaceEntity RightSpace in RightSpaceEntries)
			{
				UnionHash[RightSpace.SpaceId] = RightSpace;
			}

			OutList.AddRange(UnionHash.Values);
			return OutList.ToArray();
		}

		public SpaceConstraintField Field;
		public SpaceConstraintOperator Operator;
		public object ConstraintData;
		public SpaceConstraintDataType ConstraintDataType;

		public SpaceConstraint ChildConstraint;
		public SpaceConstraint LeftChildConstraint;
		public SpaceConstraint RightChildConstraint;
	}
}
