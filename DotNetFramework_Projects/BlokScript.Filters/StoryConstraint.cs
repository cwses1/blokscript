using System;
using BlokScript.Entities;
using BlokScript.Common;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace BlokScript.Filters
{
	public class StoryConstraint
	{
		public StoryEntity[] Evaluate (StoryEntity[] Stories)
		{
			List<StoryEntity> InList = new List<StoryEntity>();
			InList.AddRange(Stories);

			List<StoryEntity> OutList = new List<StoryEntity>();

			if (Field == StoryConstraintField.Id)
			{
				if (Operator == StoryConstraintOperator.Equals)
				{
					int ContraintValue = (int)ConstraintData;

					foreach (StoryEntity Story in InList)
					{
						if (Story.StoryId != null && Int32.Parse(Story.StoryId) == ContraintValue)
						{
							OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.NotEquals)
				{
					int ContraintValue = (int)ConstraintData;

					foreach (StoryEntity Story in InList)
					{
						if (Story.StoryId != null && Int32.Parse(Story.StoryId) != ContraintValue)
						{
							OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.In)
				{
					int[] ContraintValues = (int[])ConstraintData;

					foreach (StoryEntity Story in InList)
					{
						foreach (int CurrentConstraintValue in ContraintValues)
						{
							if (CurrentConstraintValue == Int32.Parse(Story.StoryId))
							{
								OutList.Add(Story);
								break;
							}
						}
					}
				}
				else if (Operator == StoryConstraintOperator.NotIn)
				{
					int[] ContraintValues = (int[])ConstraintData;

					foreach (StoryEntity Story in InList)
					{
						bool Found = false;

						foreach (int CurrentConstraintValue in ContraintValues)
						{
							if (CurrentConstraintValue == Int32.Parse(Story.StoryId))
							{
								Found = true;
								break;
							}
						}

						if (!Found)
							OutList.Add(Story);
					}
				}
				else
					throw new NotImplementedException("StoryConstraint");
			}
			else if (Field == StoryConstraintField.Name)
			{
				if (Operator == StoryConstraintOperator.Equals)
				{
					string ContraintParam = (string)ConstraintData;

					foreach (StoryEntity Story in InList)
					{
						if (Story.Name == ContraintParam)
						{
							OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.NotEquals)
				{
					string ContraintParam = (string)ConstraintData;

					foreach (StoryEntity Story in InList)
					{
						if (Story.Name != ContraintParam)
						{
							OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.In)
				{
					if (ConstraintDataType == StoryConstraintDataType.StringList)
					{
						string[] ContraintParamList = (string[])ConstraintData;

						foreach (StoryEntity Story in InList)
						{
							foreach (string ContraintParam in ContraintParamList)
							{
								if (Story.Name == ContraintParam)
								{
									OutList.Add(Story);
									break;
								}
							}
						}
					}
					else if (ConstraintDataType == StoryConstraintDataType.RegexList)
					{
						Regex[] ContraintParamList = (Regex[])ConstraintData;

						foreach (StoryEntity Story in InList)
						{
							foreach (Regex ContraintParam in ContraintParamList)
							{
								if (ContraintParam.IsMatch(Story.Name))
								{
									OutList.Add(Story);
									break;
								}
							}
						}
					}
				}
				else if (Operator == StoryConstraintOperator.NotIn)
				{
					if (ConstraintDataType == StoryConstraintDataType.StringList)
					{
						string[] ContraintParamList = (string[])ConstraintData;

						foreach (StoryEntity Story in InList)
						{
							bool NameFound = false;

							foreach (string ContraintParam in ContraintParamList)
							{
								if (Story.Name == ContraintParam)
								{
									NameFound = true;
									break;
								}
							}

							if (!NameFound)
								OutList.Add(Story);
						}
					}
					else if (ConstraintDataType == StoryConstraintDataType.RegexList)
					{
						Regex[] ContraintParamList = (Regex[])ConstraintData;

						foreach (StoryEntity Story in InList)
						{
							foreach (Regex ContraintParam in ContraintParamList)
							{
								if (!ContraintParam.IsMatch(Story.Name))
								{
									OutList.Add(Story);
									break;
								}
							}
						}
					}
				}
				else if (Operator == StoryConstraintOperator.MatchesRegex)
				{
					Regex ContraintParam = (Regex)ConstraintData;

					foreach (StoryEntity Story in InList)
					{
						if (ContraintParam.IsMatch(Story.Name))
						{
							OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.DoesNotMatchRegex)
				{
					Regex ContraintParam = (Regex)ConstraintData;

					foreach (StoryEntity Story in InList)
					{
						if (!ContraintParam.IsMatch(Story.Name))
						{
							OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.Like)
				{
					string ContraintParam = (string)ConstraintData;
					ContraintParam = ContraintParam.Replace("_", ".");
					ContraintParam = ContraintParam.Replace("%", ".*");
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (StoryEntity Story in InList)
					{
						if (ConstraintRegex.IsMatch(Story.Name))
						{
							OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.NotLike)
				{
					string ContraintParam = (string)ConstraintData;
					ContraintParam = ContraintParam.Replace("_", ".");
					ContraintParam = ContraintParam.Replace("%", ".*");
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (StoryEntity Story in InList)
					{
						if (!ConstraintRegex.IsMatch(Story.Name))
						{
							OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.StartsWith)
				{
					string ContraintParam = "^" + (string)ConstraintData;
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (StoryEntity Story in InList)
					{
						if (ConstraintRegex.IsMatch(Story.Name))
						{
							OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.DoesNotStartWith)
				{
					string ContraintParam = "^" + (string)ConstraintData;
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (StoryEntity Story in InList)
					{
						if (!ConstraintRegex.IsMatch(Story.Name))
						{
							OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.EndsWith)
				{
					Regex ConstraintRegex = new Regex((string)ConstraintData + "$");

					foreach (StoryEntity Story in InList)
					{
						if (ConstraintRegex.IsMatch(Story.Name))
						{
							OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.DoesNotEndWith)
				{
					Regex ConstraintRegex = new Regex((string)ConstraintData + "$");

					foreach (StoryEntity Story in InList)
					{
						if (!ConstraintRegex.IsMatch(Story.Name))
						{
							OutList.Add(Story);
						}
					}
				}
				else
					throw new NotImplementedException("StoryConstraint");
			}
			else if (Field == StoryConstraintField.Url)
			{
				if (Operator == StoryConstraintOperator.Equals)
				{
					string ContraintParam = (string)ConstraintData;

					foreach (StoryEntity Story in InList)
					{
						if (Story.Url == ContraintParam)
						{
							OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.NotEquals)
				{
					string ContraintParam = (string)ConstraintData;

					foreach (StoryEntity Story in InList)
					{
						if (Story.Url != ContraintParam)
						{
							OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.In)
				{
					if (ConstraintDataType == StoryConstraintDataType.StringList)
					{
						string[] ContraintParamList = (string[])ConstraintData;

						foreach (StoryEntity Story in InList)
						{
							foreach (string ContraintParam in ContraintParamList)
							{
								if (Story.Url == ContraintParam)
								{
									OutList.Add(Story);
									break;
								}
							}
						}
					}
					else if (ConstraintDataType == StoryConstraintDataType.RegexList)
					{
						Regex[] ContraintParamList = (Regex[])ConstraintData;

						foreach (StoryEntity Story in InList)
						{
							foreach (Regex ContraintParam in ContraintParamList)
							{
								if (ContraintParam.IsMatch(Story.Url))
								{
									OutList.Add(Story);
									break;
								}
							}
						}
					}
				}
				else if (Operator == StoryConstraintOperator.NotIn)
				{
					if (ConstraintDataType == StoryConstraintDataType.StringList)
					{
						string[] ContraintParamList = (string[])ConstraintData;

						foreach (StoryEntity Story in InList)
						{
							bool Found = false;

							foreach (string ContraintParam in ContraintParamList)
							{
								if (Story.Url == ContraintParam)
								{
									Found = true;
									break;
								}
							}

							if (!Found)
								OutList.Add(Story);
						}
					}
					else if (ConstraintDataType == StoryConstraintDataType.RegexList)
					{
						Regex[] ContraintParamList = (Regex[])ConstraintData;

						foreach (StoryEntity Story in InList)
						{
							bool Matches = false;

							foreach (Regex ContraintParam in ContraintParamList)
							{
								if (!ContraintParam.IsMatch(Story.Name))
								{
									Matches = true;
									break;
								}
							}

							if (!Matches)
								OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.MatchesRegex)
				{
					Regex ContraintParam = (Regex)ConstraintData;

					foreach (StoryEntity Story in InList)
					{
						if (ContraintParam.IsMatch(Story.Url))
						{
							OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.DoesNotMatchRegex)
				{
					Regex ContraintParam = (Regex)ConstraintData;

					foreach (StoryEntity Story in InList)
					{
						if (!ContraintParam.IsMatch(Story.Url))
						{
							OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.Like)
				{
					string ContraintParam = (string)ConstraintData;
					ContraintParam = ContraintParam.Replace("_", ".");
					ContraintParam = ContraintParam.Replace("%", ".*");
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (StoryEntity Story in InList)
					{
						if (ConstraintRegex.IsMatch(Story.Url))
						{
							OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.NotLike)
				{
					string ContraintParam = (string)ConstraintData;
					ContraintParam = ContraintParam.Replace("_", ".");
					ContraintParam = ContraintParam.Replace("%", ".*");
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (StoryEntity Story in InList)
					{
						if (!ConstraintRegex.IsMatch(Story.Url))
						{
							OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.StartsWith)
				{
					string ContraintParam = "^" + (string)ConstraintData;
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (StoryEntity Story in InList)
					{
						if (ConstraintRegex.IsMatch(Story.Url))
						{
							OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.DoesNotStartWith)
				{
					string ContraintParam = "^" + (string)ConstraintData;
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (StoryEntity Story in InList)
					{
						if (!ConstraintRegex.IsMatch(Story.Url))
						{
							OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.EndsWith)
				{
					string ContraintParam = (string)ConstraintData + "$";
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (StoryEntity Story in InList)
					{
						if (ConstraintRegex.IsMatch(Story.Url))
						{
							OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.DoesNotEndWith)
				{
					string ContraintParam = (string)ConstraintData + "$";
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (StoryEntity Story in InList)
					{
						if (!ConstraintRegex.IsMatch(Story.Url))
						{
							OutList.Add(Story);
						}
					}
				}
				else
					throw new NotImplementedException("StoryConstraint");
			}
			else if (Field == StoryConstraintField.AnyTag)
			{
				if (Operator == StoryConstraintOperator.Equals)
				{
					string ContraintParam = (string)ConstraintData;

					foreach (StoryEntity Story in InList)
					{
						foreach (string Tag in Story.Tags)
						{
							if (Tag == ContraintParam)
							{
								OutList.Add(Story);
								break;
							}
						}
					}
				}
				else if (Operator == StoryConstraintOperator.NotEquals)
				{
					string ContraintParam = (string)ConstraintData;

					foreach (StoryEntity Story in InList)
					{
						foreach (string Tag in Story.Tags)
						{
							if (Tag != ContraintParam)
							{
								OutList.Add(Story);
								break;
							}
						}
					}
				}
				else if (Operator == StoryConstraintOperator.In)
				{
					if (ConstraintDataType == StoryConstraintDataType.StringList)
					{
						string[] ContraintParamList = (string[])ConstraintData;

						foreach (StoryEntity Story in InList)
						{
							bool TagFound = false;

							foreach (string ContraintParam in ContraintParamList)
							{
								foreach (string Tag in Story.Tags)
								{
									if (Tag == ContraintParam)
									{
										TagFound= true;
										break;
									}
								}

								if (TagFound)
									break;
							}

							if (TagFound)
								OutList.Add(Story);
						}
					}
					else if (ConstraintDataType == StoryConstraintDataType.RegexList)
					{
						Regex[] ContraintParamList = (Regex[])ConstraintData;

						foreach (StoryEntity Story in InList)
						{
							bool TagFound = false;

							foreach (Regex ContraintParam in ContraintParamList)
							{
								foreach (string Tag in Story.Tags)
								{
									if (!ContraintParam.IsMatch(Tag))
									{
										TagFound = true;
										break;
									}
								}

								if (TagFound)
									break;
							}

							if (TagFound)
								OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.NotIn)
				{
					if (ConstraintDataType == StoryConstraintDataType.StringList)
					{
						string[] ContraintParamList = (string[])ConstraintData;

						foreach (StoryEntity Story in InList)
						{
							bool TagFound = true;

							foreach (string Tag in Story.Tags)
							{
								foreach (string ContraintParam in ContraintParamList)
								{
									if (Tag != ContraintParam)
									{
										TagFound = false;
										break;
									}
								}

								if (!TagFound)
									break;
							}

							if (!TagFound)
								OutList.Add(Story);
						}
					}
					else if (ConstraintDataType == StoryConstraintDataType.RegexList)
					{
						Regex[] ContraintParamList = (Regex[])ConstraintData;

						foreach (StoryEntity Story in InList)
						{
							bool TagMatches = true;

							foreach (string Tag in Story.Tags)
							{
								foreach (Regex ContraintParam in ContraintParamList)
								{
									if (!ContraintParam.IsMatch(Tag))
									{
										TagMatches = false;
										break;
									}
								}

								if (!TagMatches)
									break;
							}

							if (!TagMatches)
								OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.MatchesRegex)
				{
					Regex ContraintParam = (Regex)ConstraintData;

					foreach (StoryEntity Story in InList)
					{
						foreach (string Tag in Story.Tags)
						{
							if (ContraintParam.IsMatch(Tag))
							{
								OutList.Add(Story);
								break;
							}
						}
					}
				}
				else if (Operator == StoryConstraintOperator.DoesNotMatchRegex)
				{
					Regex ContraintParam = (Regex)ConstraintData;

					foreach (StoryEntity Story in InList)
					{
						foreach (string Tag in Story.Tags)
						{
							if (!ContraintParam.IsMatch(Tag))
							{
								OutList.Add(Story);
								break;
							}
						}
					}
				}
				else if (Operator == StoryConstraintOperator.Like)
				{
					string ContraintParam = (string)ConstraintData;
					ContraintParam = ContraintParam.Replace("_", ".");
					ContraintParam = ContraintParam.Replace("%", ".*");
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (StoryEntity Story in InList)
					{
						foreach (string Tag in Story.Tags)
						{
							if (ConstraintRegex.IsMatch(Tag))
							{
								OutList.Add(Story);
								break;
							}
						}
					}
				}
				else if (Operator == StoryConstraintOperator.NotLike)
				{
					string ContraintParam = (string)ConstraintData;
					ContraintParam = ContraintParam.Replace("_", ".");
					ContraintParam = ContraintParam.Replace("%", ".*");
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (StoryEntity Story in InList)
					{
						foreach (string Tag in Story.Tags)
						{
							if (!ConstraintRegex.IsMatch(Tag))
							{
								OutList.Add(Story);
								break;
							}
						}
					}
				}
				else if (Operator == StoryConstraintOperator.StartsWith)
				{
					string ContraintParam = "^" + (string)ConstraintData;
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (StoryEntity Story in InList)
					{
						foreach (string Tag in Story.Tags)
						{
							if (ConstraintRegex.IsMatch(Tag))
							{
								OutList.Add(Story);
								break;
							}
						}
					}
				}
				else if (Operator == StoryConstraintOperator.DoesNotStartWith)
				{
					string ContraintParam = "^" + (string)ConstraintData;
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (StoryEntity Story in InList)
					{
						foreach (string Tag in Story.Tags)
						{
							if (!ConstraintRegex.IsMatch(Tag))
							{
								OutList.Add(Story);
								break;
							}
						}
					}
				}
				else if (Operator == StoryConstraintOperator.EndsWith)
				{
					string ContraintParam = (string)ConstraintData + "$";
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (StoryEntity Story in InList)
					{
						foreach (string Tag in Story.Tags)
						{
							if (ConstraintRegex.IsMatch(Tag))
							{
								OutList.Add(Story);
								break;
							}
						}
					}
				}
				else if (Operator == StoryConstraintOperator.DoesNotEndWith)
				{
					string ContraintParam = (string)ConstraintData + "$";
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (StoryEntity Story in InList)
					{
						foreach (string Tag in Story.Tags)
						{
							if (!ConstraintRegex.IsMatch(Tag))
							{
								OutList.Add(Story);
								break;
							}
						}
					}
				}
				else if (Operator == StoryConstraintOperator.AnyTags)
				{
					foreach (StoryEntity Story in InList)
					{
						if (Story.Tags.Length > 0)
							OutList.Add(Story);
					}
				}
				else if (Operator == StoryConstraintOperator.NoTags)
				{
					foreach (StoryEntity Story in InList)
					{
						if (Story.Tags.Length == 0)
							OutList.Add(Story);
					}
				}
				else
					throw new NotImplementedException("StoryConstraint");
			}
			else if (Field == StoryConstraintField.AllTags)
			{
				if (Operator == StoryConstraintOperator.Equals)
				{
					//
					// ALL TAGS MUST BE EQUAL TO THE CONSTRAINT.
					//
					string ContraintParam = (string)ConstraintData;

					foreach (StoryEntity Story in InList)
					{
						bool AllTagsEqual = true;

						foreach (string Tag in Story.Tags)
						{
							if (Tag != ContraintParam)
							{
								AllTagsEqual = false;
								break;
							}
						}

						if (AllTagsEqual)
							OutList.Add(Story);
					}
				}
				else if (Operator == StoryConstraintOperator.NotEquals)
				{
					//
					// ALL TAGS MUST NOT BE EQUAL TO THE CONSTRAINT.
					//
					string ContraintParam = (string)ConstraintData;

					foreach (StoryEntity Story in InList)
					{
						bool AllTagsEqual = false;

						foreach (string Tag in Story.Tags)
						{
							if (Tag == ContraintParam)
							{
								AllTagsEqual = true;
								break;
							}
						}

						if (!AllTagsEqual)
							OutList.Add(Story);
					}
				}
				else if (Operator == StoryConstraintOperator.In)
				{
					if (ConstraintDataType == StoryConstraintDataType.StringList)
					{
						//
						// ALL TAGS MUST BE IN THE LIST OF STRINGS.
						//
						string[] ContraintParamList = (string[])ConstraintData;

						foreach (StoryEntity Story in InList)
						{
							bool AllTagsAreIn = true;

							foreach (string Tag in Story.Tags)
							{
								foreach (string ContraintParam in ContraintParamList)
								{
									if (Tag != ContraintParam)
									{
										AllTagsAreIn = false;
										break;
									}
								}

								if (!AllTagsAreIn)
									OutList.Add(Story);
							}

							if (AllTagsAreIn)
								OutList.Add(Story);
						}
					}
					else if (ConstraintDataType == StoryConstraintDataType.RegexList)
					{
						//
						// ALL TAGS MUST MATCH EVERY REGEX.
						//
						Regex[] ContraintParamList = (Regex[])ConstraintData;

						foreach (StoryEntity Story in InList)
						{
							bool AllTagsMatch = true;

							foreach (string Tag in Story.Tags)
							{
								foreach (Regex ContraintParam in ContraintParamList)
								{
									if (!ContraintParam.IsMatch(Tag))
									{
										AllTagsMatch = false;
										break;
									}
								}

								if (!AllTagsMatch)
									break;
							}

							if (AllTagsMatch)
								OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.NotIn)
				{
					if (ConstraintDataType == StoryConstraintDataType.StringList)
					{
						//
						// ALL TAGS MUST NOT BE IN THE STRING LIST.
						//
						string[] ContraintParamList = (string[])ConstraintData;

						foreach (StoryEntity Story in InList)
						{
							bool TagFound = false;

							foreach (string Tag in Story.Tags)
							{
								foreach (string ContraintParam in ContraintParamList)
								{
									if (Tag == ContraintParam)
									{
										TagFound = true;
										break;
									}
								}

								if (TagFound)
									break;
							}

							if (!TagFound)
								OutList.Add(Story);
						}
					}
					else if (ConstraintDataType == StoryConstraintDataType.RegexList)
					{
						//
						// ALL TAGS MUST NOT BE IN THE REGEX LIST.
						//
						Regex[] ContraintParamList = (Regex[])ConstraintData;

						foreach (StoryEntity Story in InList)
						{
							bool TagMatches = false;

							foreach (string Tag in Story.Tags)
							{
								foreach (Regex ContraintParam in ContraintParamList)
								{
									if (ContraintParam.IsMatch(Tag))
									{
										TagMatches = true;
										break;
									}
								}

								if (TagMatches)
									break;
							}

							if (!TagMatches)
								OutList.Add(Story);
						}
					}
				}
				else if (Operator == StoryConstraintOperator.MatchesRegex)
				{
					//
					// ALL TAGS MUST MATCH REGEX.
					//
					Regex ContraintParam = (Regex)ConstraintData;

					foreach (StoryEntity Story in InList)
					{
						bool AllTagsMatch = true;

						foreach (string Tag in Story.Tags)
						{
							if (!ContraintParam.IsMatch(Tag))
							{
								AllTagsMatch = false;
								break;
							}
						}

						if (AllTagsMatch)
							OutList.Add(Story);
					}
				}
				else if (Operator == StoryConstraintOperator.DoesNotMatchRegex)
				{
					//
					// ALL TAGS MUST NOT MATCH REGEX.
					//
					Regex ContraintParam = (Regex)ConstraintData;

					foreach (StoryEntity Story in InList)
					{
						bool MatchFound = false;

						foreach (string Tag in Story.Tags)
						{
							if (ContraintParam.IsMatch(Tag))
							{
								MatchFound = true;
								break;
							}
						}

						if (!MatchFound)
							OutList.Add(Story);
					}
				}
				else if (Operator == StoryConstraintOperator.Like)
				{
					//
					// ALL TAGS MUST MATCH THE LIKE EXPRESSION.
					//
					string ContraintParam = (string)ConstraintData;
					ContraintParam = ContraintParam.Replace("_", ".");
					ContraintParam = ContraintParam.Replace("%", ".*");
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (StoryEntity Story in InList)
					{
						bool AllTagsMatch = true;

						foreach (string Tag in Story.Tags)
						{
							if (!ConstraintRegex.IsMatch(Tag))
							{
								AllTagsMatch = false;
								break;
							}
						}

						if (AllTagsMatch)
							OutList.Add(Story);
					}
				}
				else if (Operator == StoryConstraintOperator.NotLike)
				{
					//
					// ALL TAGS MUST NOT MATCH THE LIKE EXPRESSION.
					//
					string ContraintParam = (string)ConstraintData;
					ContraintParam = ContraintParam.Replace("_", ".");
					ContraintParam = ContraintParam.Replace("%", ".*");
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (StoryEntity Story in InList)
					{
						bool MatchFound = false;

						foreach (string Tag in Story.Tags)
						{
							if (ConstraintRegex.IsMatch(Tag))
							{
								MatchFound = true;
								break;
							}
						}

						if (!MatchFound)
							OutList.Add(Story);
					}
				}
				else if (Operator == StoryConstraintOperator.StartsWith)
				{
					//
					// ALL TAGS MUST START WITH.
					//
					string ContraintParam = "^" + (string)ConstraintData;
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (StoryEntity Story in InList)
					{
						bool AllTagsMatch = true;

						foreach (string Tag in Story.Tags)
						{
							if (!ConstraintRegex.IsMatch(Tag))
							{
								AllTagsMatch = false;
								break;
							}
						}

						if (AllTagsMatch)
							OutList.Add(Story);
					}
				}
				else if (Operator == StoryConstraintOperator.DoesNotStartWith)
				{
					string ContraintParam = "^" + (string)ConstraintData;
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (StoryEntity Story in InList)
					{
						bool MatchFound = false;

						foreach (string Tag in Story.Tags)
						{
							if (ConstraintRegex.IsMatch(Tag))
							{
								MatchFound = true;
								break;
							}
						}

						if (!MatchFound)
							OutList.Add(Story);
					}
				}
				else if (Operator == StoryConstraintOperator.EndsWith)
				{
					string ContraintParam = (string)ConstraintData + "$";
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (StoryEntity Story in InList)
					{
						bool AllTagsMatch = true;

						foreach (string Tag in Story.Tags)
						{
							if (!ConstraintRegex.IsMatch(Tag))
							{
								AllTagsMatch = false;
								break;
							}
						}

						if (AllTagsMatch)
							OutList.Add(Story);
					}
				}
				else if (Operator == StoryConstraintOperator.DoesNotEndWith)
				{
					string ContraintParam = (string)ConstraintData + "$";
					Regex ConstraintRegex = new Regex(ContraintParam);

					foreach (StoryEntity Story in InList)
					{
						bool MatchFound = false;

						foreach (string Tag in Story.Tags)
						{
							if (ConstraintRegex.IsMatch(Tag))
							{
								MatchFound = true;
								break;
							}
						}

						if (!MatchFound)
							OutList.Add(Story);
					}
				}
				else
					throw new NotImplementedException("StoryConstraint");
			}

			//
			// COMBINE WITH CHILD STORIES.
			//
			if (ChildConstraint != null)
			{
				StoryEntity[] ChildStories = ChildConstraint.Evaluate(Stories);

				if (ChildConstraintOperator == "and")
					return Intersect(OutList.ToArray(), ChildStories);
				else if (ChildConstraintOperator == "or")
					return Union(OutList.ToArray(), ChildStories);
				else
					throw new NotImplementedException("StoryConstraint");
			}

			return OutList.ToArray();
		}


		public static StoryEntity[] Intersect (StoryEntity[] LeftStories, StoryEntity[] RightStories)
		{
			Dictionary<string, StoryEntity> LeftHash = new Dictionary<string, StoryEntity>();
			Dictionary<string, StoryEntity> RightHash = new Dictionary<string, StoryEntity>();
			List<StoryEntity> OutList = new List<StoryEntity>();

			foreach (StoryEntity LeftStory in LeftStories)
			{
				LeftHash[LeftStory.StoryId] = LeftStory;
			}

			foreach (StoryEntity RightStory in RightStories)
			{
				RightHash[RightStory.StoryId] = RightStory;
			}

			foreach (string StoryId in LeftHash.Keys)
			{
				if (RightHash.ContainsKey(StoryId))
					OutList.Add(RightHash[StoryId]);
			}

			return OutList.ToArray();
		}


		public static StoryEntity[] Union (StoryEntity[] LeftStories, StoryEntity[] RightStories)
		{
			Dictionary<string, StoryEntity> UnionHash = new Dictionary<string, StoryEntity>();
			List<StoryEntity> OutList = new List<StoryEntity>();

			foreach (StoryEntity LeftStory in LeftStories)
			{
				UnionHash[LeftStory.StoryId] = LeftStory;
			}

			foreach (StoryEntity RightStory in RightStories)
			{
				UnionHash[RightStory.StoryId] = RightStory;
			}

			OutList.AddRange(UnionHash.Values);
			return OutList.ToArray();
		}

		public StoryConstraintField Field;
		public StoryConstraintOperator Operator;
		public object ConstraintData;
		public StoryConstraintDataType ConstraintDataType;
		public StoryConstraint ChildConstraint;
		public string ChildConstraintOperator;
	}
}
