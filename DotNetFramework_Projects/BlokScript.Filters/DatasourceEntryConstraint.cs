using System;
using BlokScript.Entities;
using BlokScript.Common;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace BlokScript.Filters
{
	public class DatasourceEntryConstraint
	{
		public DatasourceEntryEntity[] Evaluate (DatasourceEntryEntity[] DatasourceEntries)
		{
			List<DatasourceEntryEntity> InList = new List<DatasourceEntryEntity>();
			InList.AddRange(DatasourceEntries);
			List<DatasourceEntryEntity> OutList = new List<DatasourceEntryEntity>();

			if (Operator == DatasourceEntryConstraintOperator.Root)
			{
				//
				// THE ROOT OPERATOR IS SPECIAL.
				// IT HAS ONE CHILD AND ONLY THE CHILD IS EVALUATED.
				//
				return ChildConstraint.Evaluate(DatasourceEntries);
			}
			else if (Operator == DatasourceEntryConstraintOperator.Intersect)
			{
				//
				// THE INTERSECTION OPERATOR PERFORMS AN "AND" OPERATION BETWEEN SIBLINGS.
				//
				return Intersect(LeftChildConstraint.Evaluate(DatasourceEntries), RightChildConstraint.Evaluate(DatasourceEntries));
			}
			else if (Operator == DatasourceEntryConstraintOperator.Union)
			{
				//
				// THE UNION OPERATOR PERFORMS AN "OR" OPERATION BETWEEN SIBLINGS.
				//
				return Union(LeftChildConstraint.Evaluate(DatasourceEntries), RightChildConstraint.Evaluate(DatasourceEntries));
			}
			else if (Field == DatasourceEntryConstraintField.Id)
			{
				//
				// ID FIELD.
				//
				if (Operator == DatasourceEntryConstraintOperator.Equals)
				{
					int ContraintValue = (int)ConstraintData;

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						if (DatasourceEntry.DatasourceEntryId != null && Int32.Parse(DatasourceEntry.DatasourceEntryId) == ContraintValue)
						{
							OutList.Add(DatasourceEntry);
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.NotEquals)
				{
					int ContraintValue = (int)ConstraintData;

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						if (DatasourceEntry.DatasourceEntryId != null && Int32.Parse(DatasourceEntry.DatasourceEntryId) != ContraintValue)
						{
							OutList.Add(DatasourceEntry);
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.In)
				{
					int[] ContraintValues = (int[])ConstraintData;

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						foreach (int CurrentConstraintValue in ContraintValues)
						{
							if (CurrentConstraintValue == Int32.Parse(DatasourceEntry.DatasourceEntryId))
							{
								OutList.Add(DatasourceEntry);
								break;
							}
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.NotIn)
				{
					int[] ContraintValues = (int[])ConstraintData;

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						bool Found = false;

						foreach (int CurrentConstraintValue in ContraintValues)
						{
							if (CurrentConstraintValue == Int32.Parse(DatasourceEntry.DatasourceEntryId))
							{
								Found = true;
								break;
							}
						}

						if (!Found)
							OutList.Add(DatasourceEntry);
					}
				}
				else
					throw new NotImplementedException("DatasourceEntryConstraint");
			}
			else if (Field == DatasourceEntryConstraintField.Name)
			{
				//
				// NAME FIELD CONSTRAINTS.
				//
				if (Operator == DatasourceEntryConstraintOperator.Equals)
				{
					string ContraintParam = (string)ConstraintData;

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						if (DatasourceEntry.Name == ContraintParam)
						{
							OutList.Add(DatasourceEntry);
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.NotEquals)
				{
					string ContraintParam = (string)ConstraintData;

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						if (DatasourceEntry.Name != ContraintParam)
						{
							OutList.Add(DatasourceEntry);
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.In)
				{
					if (ConstraintDataType == DatasourceEntryConstraintDataType.StringList)
					{
						string[] ContraintParamList = (string[])ConstraintData;

						foreach (DatasourceEntryEntity DatasourceEntry in InList)
						{
							foreach (string ContraintParam in ContraintParamList)
							{
								if (DatasourceEntry.Name == ContraintParam)
								{
									OutList.Add(DatasourceEntry);
									break;
								}
							}
						}
					}
					else if (ConstraintDataType == DatasourceEntryConstraintDataType.RegexList)
					{
						Regex[] ContraintParamList = (Regex[])ConstraintData;

						foreach (DatasourceEntryEntity DatasourceEntry in InList)
						{
							foreach (Regex ContraintParam in ContraintParamList)
							{
								if (ContraintParam.IsMatch(DatasourceEntry.Name))
								{
									OutList.Add(DatasourceEntry);
									break;
								}
							}
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.NotIn)
				{
					if (ConstraintDataType == DatasourceEntryConstraintDataType.StringList)
					{
						string[] ContraintParamList = (string[])ConstraintData;

						foreach (DatasourceEntryEntity DatasourceEntry in InList)
						{
							bool NameFound = false;

							foreach (string ContraintParam in ContraintParamList)
							{
								if (DatasourceEntry.Name == ContraintParam)
								{
									NameFound = true;
									break;
								}
							}

							if (!NameFound)
								OutList.Add(DatasourceEntry);
						}
					}
					else if (ConstraintDataType == DatasourceEntryConstraintDataType.RegexList)
					{
						Regex[] ContraintParamList = (Regex[])ConstraintData;

						foreach (DatasourceEntryEntity DatasourceEntry in InList)
						{
							foreach (Regex ContraintParam in ContraintParamList)
							{
								if (!ContraintParam.IsMatch(DatasourceEntry.Name))
								{
									OutList.Add(DatasourceEntry);
									break;
								}
							}
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.MatchesRegex)
				{
					Regex ContraintParam = (Regex)ConstraintData;

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						if (ContraintParam.IsMatch(DatasourceEntry.Name))
						{
							OutList.Add(DatasourceEntry);
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.DoesNotMatchRegex)
				{
					Regex ContraintParam = (Regex)ConstraintData;

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						if (!ContraintParam.IsMatch(DatasourceEntry.Name))
						{
							OutList.Add(DatasourceEntry);
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.Like)
				{
					string ContraintParam = (string)ConstraintData;
					ContraintParam = ContraintParam.Replace("_", ".");
					ContraintParam = ContraintParam.Replace("%", ".*");
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						if (ConstraintRegex.IsMatch(DatasourceEntry.Name))
						{
							OutList.Add(DatasourceEntry);
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.NotLike)
				{
					string ContraintParam = (string)ConstraintData;
					ContraintParam = ContraintParam.Replace("_", ".");
					ContraintParam = ContraintParam.Replace("%", ".*");
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						if (!ConstraintRegex.IsMatch(DatasourceEntry.Name))
						{
							OutList.Add(DatasourceEntry);
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.StartsWith)
				{
					string ContraintParam = "^" + (string)ConstraintData;
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						if (ConstraintRegex.IsMatch(DatasourceEntry.Name))
						{
							OutList.Add(DatasourceEntry);
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.DoesNotStartWith)
				{
					string ContraintParam = "^" + (string)ConstraintData;
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						if (!ConstraintRegex.IsMatch(DatasourceEntry.Name))
						{
							OutList.Add(DatasourceEntry);
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.EndsWith)
				{
					Regex ConstraintRegex = new Regex((string)ConstraintData + "$");

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						if (ConstraintRegex.IsMatch(DatasourceEntry.Name))
						{
							OutList.Add(DatasourceEntry);
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.DoesNotEndWith)
				{
					Regex ConstraintRegex = new Regex((string)ConstraintData + "$");

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						if (!ConstraintRegex.IsMatch(DatasourceEntry.Name))
						{
							OutList.Add(DatasourceEntry);
						}
					}
				}
				else
					throw new NotImplementedException("DatasourceEntryConstraint");
			}
			//
			// VALUE FIELD CONSTRAINTS
			//
			else if (Field == DatasourceEntryConstraintField.Value)
			{
				if (Operator == DatasourceEntryConstraintOperator.Equals)
				{
					string ContraintParam = (string)ConstraintData;

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						if (DatasourceEntry.Value == ContraintParam)
						{
							OutList.Add(DatasourceEntry);
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.NotEquals)
				{
					string ContraintParam = (string)ConstraintData;

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						if (DatasourceEntry.Value != ContraintParam)
						{
							OutList.Add(DatasourceEntry);
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.In)
				{
					if (ConstraintDataType == DatasourceEntryConstraintDataType.StringList)
					{
						string[] ContraintParamList = (string[])ConstraintData;

						foreach (DatasourceEntryEntity DatasourceEntry in InList)
						{
							foreach (string ContraintParam in ContraintParamList)
							{
								if (DatasourceEntry.Value == ContraintParam)
								{
									OutList.Add(DatasourceEntry);
									break;
								}
							}
						}
					}
					else if (ConstraintDataType == DatasourceEntryConstraintDataType.RegexList)
					{
						Regex[] ContraintParamList = (Regex[])ConstraintData;

						foreach (DatasourceEntryEntity DatasourceEntry in InList)
						{
							foreach (Regex ContraintParam in ContraintParamList)
							{
								if (ContraintParam.IsMatch(DatasourceEntry.Value))
								{
									OutList.Add(DatasourceEntry);
									break;
								}
							}
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.NotIn)
				{
					if (ConstraintDataType == DatasourceEntryConstraintDataType.StringList)
					{
						string[] ContraintParamList = (string[])ConstraintData;

						foreach (DatasourceEntryEntity DatasourceEntry in InList)
						{
							bool Found = false;

							foreach (string ContraintParam in ContraintParamList)
							{
								if (DatasourceEntry.Value == ContraintParam)
								{
									Found = true;
									break;
								}
							}

							if (!Found)
								OutList.Add(DatasourceEntry);
						}
					}
					else if (ConstraintDataType == DatasourceEntryConstraintDataType.RegexList)
					{
						Regex[] ContraintParamList = (Regex[])ConstraintData;

						foreach (DatasourceEntryEntity DatasourceEntry in InList)
						{
							bool Matches = false;

							foreach (Regex ContraintParam in ContraintParamList)
							{
								if (!ContraintParam.IsMatch(DatasourceEntry.Name))
								{
									Matches = true;
									break;
								}
							}

							if (!Matches)
								OutList.Add(DatasourceEntry);
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.MatchesRegex)
				{
					Regex ContraintParam = (Regex)ConstraintData;

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						if (ContraintParam.IsMatch(DatasourceEntry.Value))
						{
							OutList.Add(DatasourceEntry);
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.DoesNotMatchRegex)
				{
					Regex ContraintParam = (Regex)ConstraintData;

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						if (!ContraintParam.IsMatch(DatasourceEntry.Value))
						{
							OutList.Add(DatasourceEntry);
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.Like)
				{
					string ContraintParam = (string)ConstraintData;
					ContraintParam = ContraintParam.Replace("_", ".");
					ContraintParam = ContraintParam.Replace("%", ".*");
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						if (ConstraintRegex.IsMatch(DatasourceEntry.Value))
						{
							OutList.Add(DatasourceEntry);
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.NotLike)
				{
					string ContraintParam = (string)ConstraintData;
					ContraintParam = ContraintParam.Replace("_", ".");
					ContraintParam = ContraintParam.Replace("%", ".*");
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						if (!ConstraintRegex.IsMatch(DatasourceEntry.Value))
						{
							OutList.Add(DatasourceEntry);
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.StartsWith)
				{
					string ContraintParam = "^" + (string)ConstraintData;
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						if (ConstraintRegex.IsMatch(DatasourceEntry.Value))
						{
							OutList.Add(DatasourceEntry);
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.DoesNotStartWith)
				{
					string ContraintParam = "^" + (string)ConstraintData;
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						if (!ConstraintRegex.IsMatch(DatasourceEntry.Value))
						{
							OutList.Add(DatasourceEntry);
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.EndsWith)
				{
					string ContraintParam = (string)ConstraintData + "$";
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						if (ConstraintRegex.IsMatch(DatasourceEntry.Value))
						{
							OutList.Add(DatasourceEntry);
						}
					}
				}
				else if (Operator == DatasourceEntryConstraintOperator.DoesNotEndWith)
				{
					string ContraintParam = (string)ConstraintData + "$";
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (DatasourceEntryEntity DatasourceEntry in InList)
					{
						if (!ConstraintRegex.IsMatch(DatasourceEntry.Value))
						{
							OutList.Add(DatasourceEntry);
						}
					}
				}
				else
					throw new NotImplementedException("DatasourceEntryConstraint");
			}
			else
				throw new NotImplementedException("DatasourceEntryConstraint");


			return OutList.ToArray();
		}

		public static DatasourceEntryEntity[] Intersect (DatasourceEntryEntity[] LeftDatasourceEntries, DatasourceEntryEntity[] RightDatasourceEntries)
		{
			Dictionary<string, DatasourceEntryEntity> LeftHash = new Dictionary<string, DatasourceEntryEntity>();
			Dictionary<string, DatasourceEntryEntity> RightHash = new Dictionary<string, DatasourceEntryEntity>();
			List<DatasourceEntryEntity> OutList = new List<DatasourceEntryEntity>();

			foreach (DatasourceEntryEntity LeftDatasourceEntry in LeftDatasourceEntries)
			{
				LeftHash[LeftDatasourceEntry.DatasourceEntryId] = LeftDatasourceEntry;
			}

			foreach (DatasourceEntryEntity RightDatasourceEntry in RightDatasourceEntries)
			{
				RightHash[RightDatasourceEntry.DatasourceEntryId] = RightDatasourceEntry;
			}

			foreach (string DatasourceEntryId in LeftHash.Keys)
			{
				if (RightHash.ContainsKey(DatasourceEntryId))
					OutList.Add(RightHash[DatasourceEntryId]);
			}

			return OutList.ToArray();
		}

		public static DatasourceEntryEntity[] Union (DatasourceEntryEntity[] LeftDatasourceEntries, DatasourceEntryEntity[] RightDatasourceEntries)
		{
			Dictionary<string, DatasourceEntryEntity> UnionHash = new Dictionary<string, DatasourceEntryEntity>();
			List<DatasourceEntryEntity> OutList = new List<DatasourceEntryEntity>();

			foreach (DatasourceEntryEntity LeftDatasourceEntry in LeftDatasourceEntries)
			{
				UnionHash[LeftDatasourceEntry.DatasourceEntryId] = LeftDatasourceEntry;
			}

			foreach (DatasourceEntryEntity RightDatasourceEntry in RightDatasourceEntries)
			{
				UnionHash[RightDatasourceEntry.DatasourceEntryId] = RightDatasourceEntry;
			}

			OutList.AddRange(UnionHash.Values);
			return OutList.ToArray();
		}

		public DatasourceEntryConstraintField Field;
		public DatasourceEntryConstraintOperator Operator;
		public object ConstraintData;
		public DatasourceEntryConstraintDataType ConstraintDataType;

		public DatasourceEntryConstraint ChildConstraint;
		public DatasourceEntryConstraint LeftChildConstraint;
		public DatasourceEntryConstraint RightChildConstraint;

		public string ChildConstraintOperator;
	}
}
