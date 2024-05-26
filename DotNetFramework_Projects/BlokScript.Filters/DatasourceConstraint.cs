using System;
using BlokScript.Entities;
using BlokScript.Common;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace BlokScript.Filters
{
	public class DatasourceConstraint
	{
		public DatasourceEntity[] Evaluate (DatasourceEntity[] DatasourceEntries)
		{
			List<DatasourceEntity> InList = new List<DatasourceEntity>();
			InList.AddRange(DatasourceEntries);
			List<DatasourceEntity> OutList = new List<DatasourceEntity>();

			if (Operator == DatasourceConstraintOperator.Root)
			{
				//
				// THE ROOT OPERATOR IS SPECIAL.
				// IT HAS ONE CHILD AND ONLY THE CHILD IS EVALUATED.
				//
				return ChildConstraint.Evaluate(DatasourceEntries);
			}
			else if (Operator == DatasourceConstraintOperator.Intersect)
			{
				//
				// THE INTERSECTION OPERATOR PERFORMS AN "AND" OPERATION BETWEEN SIBLINGS.
				//
				return Intersect(LeftChildConstraint.Evaluate(DatasourceEntries), RightChildConstraint.Evaluate(DatasourceEntries));
			}
			else if (Operator == DatasourceConstraintOperator.Union)
			{
				//
				// THE UNION OPERATOR PERFORMS AN "OR" OPERATION BETWEEN SIBLINGS.
				//
				return Union(LeftChildConstraint.Evaluate(DatasourceEntries), RightChildConstraint.Evaluate(DatasourceEntries));
			}
			else if (Field == DatasourceConstraintField.Id)
			{
				//
				// ID FIELD.
				//
				if (Operator == DatasourceConstraintOperator.Equals)
				{
					int ContraintValue = (int)ConstraintData;

					foreach (DatasourceEntity Datasource in InList)
					{
						if (Datasource.DatasourceId != null && Int32.Parse(Datasource.DatasourceId) == ContraintValue)
						{
							OutList.Add(Datasource);
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.NotEquals)
				{
					int ContraintValue = (int)ConstraintData;

					foreach (DatasourceEntity Datasource in InList)
					{
						if (Datasource.DatasourceId != null && Int32.Parse(Datasource.DatasourceId) != ContraintValue)
						{
							OutList.Add(Datasource);
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.In)
				{
					int[] ContraintValues = (int[])ConstraintData;

					foreach (DatasourceEntity Datasource in InList)
					{
						foreach (int CurrentConstraintValue in ContraintValues)
						{
							if (CurrentConstraintValue == Int32.Parse(Datasource.DatasourceId))
							{
								OutList.Add(Datasource);
								break;
							}
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.NotIn)
				{
					int[] ContraintValues = (int[])ConstraintData;

					foreach (DatasourceEntity Datasource in InList)
					{
						bool Found = false;

						foreach (int CurrentConstraintValue in ContraintValues)
						{
							if (CurrentConstraintValue == Int32.Parse(Datasource.DatasourceId))
							{
								Found = true;
								break;
							}
						}

						if (!Found)
							OutList.Add(Datasource);
					}
				}
				else
					throw new NotImplementedException("DatasourceConstraint");
			}
			else if (Field == DatasourceConstraintField.Name)
			{
				//
				// NAME FIELD CONSTRAINTS.
				//
				if (Operator == DatasourceConstraintOperator.Equals)
				{
					string ContraintParam = (string)ConstraintData;

					foreach (DatasourceEntity Datasource in InList)
					{
						if (Datasource.Name == ContraintParam)
						{
							OutList.Add(Datasource);
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.NotEquals)
				{
					string ContraintParam = (string)ConstraintData;

					foreach (DatasourceEntity Datasource in InList)
					{
						if (Datasource.Name != ContraintParam)
						{
							OutList.Add(Datasource);
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.In)
				{
					if (ConstraintDataType == DatasourceConstraintDataType.StringList)
					{
						string[] ContraintParamList = (string[])ConstraintData;

						foreach (DatasourceEntity Datasource in InList)
						{
							foreach (string ContraintParam in ContraintParamList)
							{
								if (Datasource.Name == ContraintParam)
								{
									OutList.Add(Datasource);
									break;
								}
							}
						}
					}
					else if (ConstraintDataType == DatasourceConstraintDataType.RegexList)
					{
						Regex[] ContraintParamList = (Regex[])ConstraintData;

						foreach (DatasourceEntity Datasource in InList)
						{
							foreach (Regex ContraintParam in ContraintParamList)
							{
								if (ContraintParam.IsMatch(Datasource.Name))
								{
									OutList.Add(Datasource);
									break;
								}
							}
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.NotIn)
				{
					if (ConstraintDataType == DatasourceConstraintDataType.StringList)
					{
						string[] ContraintParamList = (string[])ConstraintData;

						foreach (DatasourceEntity Datasource in InList)
						{
							bool NameFound = false;

							foreach (string ContraintParam in ContraintParamList)
							{
								if (Datasource.Name == ContraintParam)
								{
									NameFound = true;
									break;
								}
							}

							if (!NameFound)
								OutList.Add(Datasource);
						}
					}
					else if (ConstraintDataType == DatasourceConstraintDataType.RegexList)
					{
						Regex[] ContraintParamList = (Regex[])ConstraintData;

						foreach (DatasourceEntity Datasource in InList)
						{
							foreach (Regex ContraintParam in ContraintParamList)
							{
								if (!ContraintParam.IsMatch(Datasource.Name))
								{
									OutList.Add(Datasource);
									break;
								}
							}
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.MatchesRegex)
				{
					Regex ContraintParam = (Regex)ConstraintData;

					foreach (DatasourceEntity Datasource in InList)
					{
						if (ContraintParam.IsMatch(Datasource.Name))
						{
							OutList.Add(Datasource);
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.DoesNotMatchRegex)
				{
					Regex ContraintParam = (Regex)ConstraintData;

					foreach (DatasourceEntity Datasource in InList)
					{
						if (!ContraintParam.IsMatch(Datasource.Name))
						{
							OutList.Add(Datasource);
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.Like)
				{
					string ContraintParam = (string)ConstraintData;
					ContraintParam = ContraintParam.Replace("_", ".");
					ContraintParam = ContraintParam.Replace("%", ".*");
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (DatasourceEntity Datasource in InList)
					{
						if (ConstraintRegex.IsMatch(Datasource.Name))
						{
							OutList.Add(Datasource);
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.NotLike)
				{
					string ContraintParam = (string)ConstraintData;
					ContraintParam = ContraintParam.Replace("_", ".");
					ContraintParam = ContraintParam.Replace("%", ".*");
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (DatasourceEntity Datasource in InList)
					{
						if (!ConstraintRegex.IsMatch(Datasource.Name))
						{
							OutList.Add(Datasource);
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.StartsWith)
				{
					string ContraintParam = "^" + (string)ConstraintData;
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (DatasourceEntity Datasource in InList)
					{
						if (ConstraintRegex.IsMatch(Datasource.Name))
						{
							OutList.Add(Datasource);
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.DoesNotStartWith)
				{
					string ContraintParam = "^" + (string)ConstraintData;
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (DatasourceEntity Datasource in InList)
					{
						if (!ConstraintRegex.IsMatch(Datasource.Name))
						{
							OutList.Add(Datasource);
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.EndsWith)
				{
					Regex ConstraintRegex = new Regex((string)ConstraintData + "$");

					foreach (DatasourceEntity Datasource in InList)
					{
						if (ConstraintRegex.IsMatch(Datasource.Name))
						{
							OutList.Add(Datasource);
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.DoesNotEndWith)
				{
					Regex ConstraintRegex = new Regex((string)ConstraintData + "$");

					foreach (DatasourceEntity Datasource in InList)
					{
						if (!ConstraintRegex.IsMatch(Datasource.Name))
						{
							OutList.Add(Datasource);
						}
					}
				}
				else
					throw new NotImplementedException("DatasourceConstraint");
			}
			//
			// VALUE FIELD CONSTRAINTS
			//
			else if (Field == DatasourceConstraintField.Slug)
			{
				if (Operator == DatasourceConstraintOperator.Equals)
				{
					string ContraintParam = (string)ConstraintData;

					foreach (DatasourceEntity Datasource in InList)
					{
						if (Datasource.Slug == ContraintParam)
						{
							OutList.Add(Datasource);
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.NotEquals)
				{
					string ContraintParam = (string)ConstraintData;

					foreach (DatasourceEntity Datasource in InList)
					{
						if (Datasource.Slug != ContraintParam)
						{
							OutList.Add(Datasource);
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.In)
				{
					if (ConstraintDataType == DatasourceConstraintDataType.StringList)
					{
						string[] ContraintParamList = (string[])ConstraintData;

						foreach (DatasourceEntity Datasource in InList)
						{
							foreach (string ContraintParam in ContraintParamList)
							{
								if (Datasource.Slug == ContraintParam)
								{
									OutList.Add(Datasource);
									break;
								}
							}
						}
					}
					else if (ConstraintDataType == DatasourceConstraintDataType.RegexList)
					{
						Regex[] ContraintParamList = (Regex[])ConstraintData;

						foreach (DatasourceEntity Datasource in InList)
						{
							foreach (Regex ContraintParam in ContraintParamList)
							{
								if (ContraintParam.IsMatch(Datasource.Slug))
								{
									OutList.Add(Datasource);
									break;
								}
							}
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.NotIn)
				{
					if (ConstraintDataType == DatasourceConstraintDataType.StringList)
					{
						string[] ContraintParamList = (string[])ConstraintData;

						foreach (DatasourceEntity Datasource in InList)
						{
							bool Found = false;

							foreach (string ContraintParam in ContraintParamList)
							{
								if (Datasource.Slug == ContraintParam)
								{
									Found = true;
									break;
								}
							}

							if (!Found)
								OutList.Add(Datasource);
						}
					}
					else if (ConstraintDataType == DatasourceConstraintDataType.RegexList)
					{
						Regex[] ContraintParamList = (Regex[])ConstraintData;

						foreach (DatasourceEntity Datasource in InList)
						{
							bool Matches = false;

							foreach (Regex ContraintParam in ContraintParamList)
							{
								if (!ContraintParam.IsMatch(Datasource.Name))
								{
									Matches = true;
									break;
								}
							}

							if (!Matches)
								OutList.Add(Datasource);
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.MatchesRegex)
				{
					Regex ContraintParam = (Regex)ConstraintData;

					foreach (DatasourceEntity Datasource in InList)
					{
						if (ContraintParam.IsMatch(Datasource.Slug))
						{
							OutList.Add(Datasource);
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.DoesNotMatchRegex)
				{
					Regex ContraintParam = (Regex)ConstraintData;

					foreach (DatasourceEntity Datasource in InList)
					{
						if (!ContraintParam.IsMatch(Datasource.Slug))
						{
							OutList.Add(Datasource);
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.Like)
				{
					string ContraintParam = (string)ConstraintData;
					ContraintParam = ContraintParam.Replace("_", ".");
					ContraintParam = ContraintParam.Replace("%", ".*");
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (DatasourceEntity Datasource in InList)
					{
						if (ConstraintRegex.IsMatch(Datasource.Slug))
						{
							OutList.Add(Datasource);
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.NotLike)
				{
					string ContraintParam = (string)ConstraintData;
					ContraintParam = ContraintParam.Replace("_", ".");
					ContraintParam = ContraintParam.Replace("%", ".*");
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (DatasourceEntity Datasource in InList)
					{
						if (!ConstraintRegex.IsMatch(Datasource.Slug))
						{
							OutList.Add(Datasource);
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.StartsWith)
				{
					string ContraintParam = "^" + (string)ConstraintData;
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (DatasourceEntity Datasource in InList)
					{
						if (ConstraintRegex.IsMatch(Datasource.Slug))
						{
							OutList.Add(Datasource);
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.DoesNotStartWith)
				{
					string ContraintParam = "^" + (string)ConstraintData;
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (DatasourceEntity Datasource in InList)
					{
						if (!ConstraintRegex.IsMatch(Datasource.Slug))
						{
							OutList.Add(Datasource);
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.EndsWith)
				{
					string ContraintParam = (string)ConstraintData + "$";
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (DatasourceEntity Datasource in InList)
					{
						if (ConstraintRegex.IsMatch(Datasource.Slug))
						{
							OutList.Add(Datasource);
						}
					}
				}
				else if (Operator == DatasourceConstraintOperator.DoesNotEndWith)
				{
					string ContraintParam = (string)ConstraintData + "$";
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (DatasourceEntity Datasource in InList)
					{
						if (!ConstraintRegex.IsMatch(Datasource.Slug))
						{
							OutList.Add(Datasource);
						}
					}
				}
				else
					throw new NotImplementedException("DatasourceConstraint");
			}
			else
				throw new NotImplementedException("DatasourceConstraint");

			return OutList.ToArray();
		}

		public static DatasourceEntity[] Intersect (DatasourceEntity[] LeftDatasourceEntries, DatasourceEntity[] RightDatasourceEntries)
		{
			Dictionary<string, DatasourceEntity> LeftHash = new Dictionary<string, DatasourceEntity>();
			Dictionary<string, DatasourceEntity> RightHash = new Dictionary<string, DatasourceEntity>();
			List<DatasourceEntity> OutList = new List<DatasourceEntity>();

			foreach (DatasourceEntity LeftDatasource in LeftDatasourceEntries)
			{
				LeftHash[LeftDatasource.DatasourceId] = LeftDatasource;
			}

			foreach (DatasourceEntity RightDatasource in RightDatasourceEntries)
			{
				RightHash[RightDatasource.DatasourceId] = RightDatasource;
			}

			foreach (string DatasourceId in LeftHash.Keys)
			{
				if (RightHash.ContainsKey(DatasourceId))
					OutList.Add(RightHash[DatasourceId]);
			}

			return OutList.ToArray();
		}

		public static DatasourceEntity[] Union (DatasourceEntity[] LeftDatasourceEntries, DatasourceEntity[] RightDatasourceEntries)
		{
			Dictionary<string, DatasourceEntity> UnionHash = new Dictionary<string, DatasourceEntity>();
			List<DatasourceEntity> OutList = new List<DatasourceEntity>();

			foreach (DatasourceEntity LeftDatasource in LeftDatasourceEntries)
			{
				UnionHash[LeftDatasource.DatasourceId] = LeftDatasource;
			}

			foreach (DatasourceEntity RightDatasource in RightDatasourceEntries)
			{
				UnionHash[RightDatasource.DatasourceId] = RightDatasource;
			}

			OutList.AddRange(UnionHash.Values);
			return OutList.ToArray();
		}

		public DatasourceConstraintField Field;
		public DatasourceConstraintOperator Operator;
		public object ConstraintData;
		public DatasourceConstraintDataType ConstraintDataType;

		public DatasourceConstraint ChildConstraint;
		public DatasourceConstraint LeftChildConstraint;
		public DatasourceConstraint RightChildConstraint;

		public string ChildConstraintOperator;
	}
}
