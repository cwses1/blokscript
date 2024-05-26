namespace BlokScript.Common
{
	public enum BlockConstraintOperator
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
		DoesNotEndWith
	}
}
