using System;
using BlokScript.Entities;
using BlokScript.Common;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BlokScript.Filters
{
	public class BlockConstraint
	{
		public BlockSchemaEntity[] Evaluate (BlockSchemaEntity[] Blocks)
		{
			List<BlockSchemaEntity> InList = new List<BlockSchemaEntity>();
			InList.AddRange(AndChildConstraint != null ? AndChildConstraint.Evaluate(Blocks) : Blocks);

			List<BlockSchemaEntity> OutList = new List<BlockSchemaEntity>();

			if (Field == BlockConstraintField.Id)
			{
				if (Operator == BlockConstraintOperator.Equals)
				{
					int ContraintValue = (int)ConstraintData;

					foreach (BlockSchemaEntity Block in InList)
					{
						if (Block.BlockId != null && Int32.Parse(Block.BlockId) == ContraintValue)
						{
							OutList.Add(Block);
						}
					}
				}
				else if (Operator == BlockConstraintOperator.NotEquals)
				{
					int ContraintValue = (int)ConstraintData;

					foreach (BlockSchemaEntity Block in InList)
					{
						if (Block.BlockId != null && Int32.Parse(Block.BlockId) != ContraintValue)
						{
							OutList.Add(Block);
						}
					}
				}
				else if (Operator == BlockConstraintOperator.In)
				{
					int[] ContraintValues = (int[])ConstraintData;

					foreach (BlockSchemaEntity Block in InList)
					{
						foreach (int CurrentConstraintValue in ContraintValues)
						{
							if (Block.BlockId != null && CurrentConstraintValue == Int32.Parse(Block.BlockId))
							{
								OutList.Add(Block);
								break;
							}
						}
					}
				}
				else if (Operator == BlockConstraintOperator.NotIn)
				{
					int[] ContraintValues = (int[])ConstraintData;

					foreach (BlockSchemaEntity Block in InList)
					{
						foreach (int CurrentConstraintValue in ContraintValues)
						{
							if (Block.BlockId != null && CurrentConstraintValue != Int32.Parse(Block.BlockId))
							{
								OutList.Add(Block);
								break;
							}
						}
					}
				}
				else
					throw new NotImplementedException("BlockConstraint");
			}
			if (Field == BlockConstraintField.Name)
			{
				if (Operator == BlockConstraintOperator.Equals)
				{
					string ContraintParam = (string)ConstraintData;

					foreach (BlockSchemaEntity Block in InList)
					{
						if (Block.ComponentName == ContraintParam)
						{
							OutList.Add(Block);
						}
					}
				}
				if (Operator == BlockConstraintOperator.NotEquals)
				{
					string ContraintParam = (string)ConstraintData;

					foreach (BlockSchemaEntity Block in InList)
					{
						if (Block.ComponentName != ContraintParam)
						{
							OutList.Add(Block);
						}
					}
				}
				else if (Operator == BlockConstraintOperator.In)
				{
					if (ConstraintDataType == BlockConstraintDataType.StringList)
					{
						string[] ContraintParamList = (string[])ConstraintData;

						foreach (BlockSchemaEntity Block in InList)
						{
							foreach (string ContraintParam in ContraintParamList)
							{
								if (Block.ComponentName == ContraintParam)
								{
									OutList.Add(Block);
									break;
								}
							}
						}
					}
					else if (ConstraintDataType == BlockConstraintDataType.RegexList)
					{
						Regex[] ContraintParamList = (Regex[])ConstraintData;

						foreach (BlockSchemaEntity Block in InList)
						{
							foreach (Regex ContraintParam in ContraintParamList)
							{
								if (ContraintParam.IsMatch(Block.ComponentName))
								{
									OutList.Add(Block);
									break;
								}
							}
						}
					}
				}
				else if (Operator == BlockConstraintOperator.NotIn)
				{
					if (ConstraintDataType == BlockConstraintDataType.StringList)
					{
						string[] ContraintParamList = (string[])ConstraintData;

						foreach (BlockSchemaEntity Block in InList)
						{
							foreach (string ContraintParam in ContraintParamList)
							{
								if (Block.ComponentName != ContraintParam)
								{
									OutList.Add(Block);
									break;
								}
							}
						}
					}
					else if (ConstraintDataType == BlockConstraintDataType.RegexList)
					{
						Regex[] ContraintParamList = (Regex[])ConstraintData;

						foreach (BlockSchemaEntity Block in InList)
						{
							foreach (Regex ContraintParam in ContraintParamList)
							{
								if (!ContraintParam.IsMatch(Block.ComponentName))
								{
									OutList.Add(Block);
									break;
								}
							}
						}
					}
				}
				else if (Operator == BlockConstraintOperator.MatchesRegex)
				{
					Regex ContraintParam = (Regex)ConstraintData;

					foreach (BlockSchemaEntity Block in InList)
					{
						if (ContraintParam.IsMatch(Block.ComponentName))
						{
							OutList.Add(Block);
						}
					}
				}
				else if (Operator == BlockConstraintOperator.DoesNotMatchRegex)
				{
					Regex ContraintParam = (Regex)ConstraintData;

					foreach (BlockSchemaEntity Block in InList)
					{
						if (!ContraintParam.IsMatch(Block.ComponentName))
						{
							OutList.Add(Block);
						}
					}
				}
				else if (Operator == BlockConstraintOperator.Like)
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

					foreach (BlockSchemaEntity Block in InList)
					{
						if (ConstraintRegex.IsMatch(Block.ComponentName))
						{
							OutList.Add(Block);
						}
					}
				}
				else if (Operator == BlockConstraintOperator.NotLike)
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

					foreach (BlockSchemaEntity Block in InList)
					{
						if (!ConstraintRegex.IsMatch(Block.ComponentName))
						{
							OutList.Add(Block);
						}
					}
				}
				else if (Operator == BlockConstraintOperator.StartsWith)
				{
					string ContraintParam = "^" + (string)ConstraintData;
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (BlockSchemaEntity Block in InList)
					{
						if (ConstraintRegex.IsMatch(Block.ComponentName))
						{
							OutList.Add(Block);
						}
					}
				}
				else if (Operator == BlockConstraintOperator.DoesNotStartWith)
				{
					string ContraintParam = "^" + (string)ConstraintData;
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (BlockSchemaEntity Block in InList)
					{
						if (!ConstraintRegex.IsMatch(Block.ComponentName))
						{
							OutList.Add(Block);
						}
					}
				}
				else if (Operator == BlockConstraintOperator.EndsWith)
				{
					string ContraintParam = (string)ConstraintData + "$";
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (BlockSchemaEntity Block in InList)
					{
						if (ConstraintRegex.IsMatch(Block.ComponentName))
						{
							OutList.Add(Block);
						}
					}
				}
				else if (Operator == BlockConstraintOperator.DoesNotEndWith)
				{
					string ContraintParam = (string)ConstraintData + "$";
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (BlockSchemaEntity Block in InList)
					{
						if (!ConstraintRegex.IsMatch(Block.ComponentName))
						{
							OutList.Add(Block);
						}
					}
				}
				else
					throw new NotImplementedException("BlockConstraint");
			}

			if (OrChildConstraint != null)
				OutList.AddRange(OrChildConstraint.Evaluate(OutList.ToArray()));

			return OutList.ToArray();
		}

		public BlockConstraintField Field;
		public BlockConstraintOperator Operator;
		public object ConstraintData;
		public BlockConstraintDataType ConstraintDataType;
		public BlockConstraint AndChildConstraint;
		public BlockConstraint OrChildConstraint;
	}
}
