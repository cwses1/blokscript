namespace BlokScript.Common
{
	public enum StoryConstraintOperator
	{
		Root,
		Intersect,
		Union,

		Equals,
		NotEquals,
		In,
		NotIn,
		MatchesRegex,
		DoesNotMatchRegex,
		Like,
		NotLike,
		StartsWith,
		DoesNotStartWith,
		EndsWith,
		DoesNotEndWith,
		NoTags,
		AnyTags
	}
}
